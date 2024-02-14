/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2024 Johan Idstam
 *
 *
 * This source code is released under the Artistic License 2.0.
 */

using Confluent.Kafka;
using logview4net.Listeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace logview4net.kafkalistener
{
    public class KafkaListener : ListenerBase
    {
        private Thread _listenerThread;

        public string BootstrapServer;
        public string Topic;
        public int StartingOffset;

        public override void Dispose()
        {
            _log.Debug(GetHashCode(), "Dispoding a SQL Listener (nothing to dispose)");
        }

        public override string GetConfigValue(string name)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, ListenerConfigField> GetConfigValueFields()
        {
            throw new NotImplementedException();
        }

        public override string SetConfigValue(string name, string value)
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "Start");
            var ts = new ThreadStart(tail);
            _listenerThread = new Thread(ts);
            _listenerThread.Start();

            IsRunning = true;
        }
        public void tail()
        {
            var brokerList = "";
            var groupId = "";
            var topics = new List<string> { };
            var cancellationToken = new CancellationToken(false);

            var config = new ConsumerConfig
            {
                BootstrapServers = brokerList,
                GroupId = groupId,
                EnableAutoOffsetStore = false,
                EnableAutoCommit = true,
                StatisticsIntervalMs = 5000,
                SessionTimeoutMs = 6000,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnablePartitionEof = true,
                PartitionAssignmentStrategy = PartitionAssignmentStrategy.CooperativeSticky
            };


            using (var consumer = new ConsumerBuilder<Ignore, string>(config)
                // Note: All handlers are called on the main .Consume thread.
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .SetStatisticsHandler((_, json) => Console.WriteLine($"Statistics: {json}"))
                .SetPartitionsAssignedHandler((c, partitions) =>
                {
                    // Since a cooperative assignor (CooperativeSticky) has been configured, the
                    // partition assignment is incremental (adds partitions to any existing assignment).
                    Console.WriteLine(
                        "Partitions incrementally assigned: [" +
                        string.Join(",", partitions.Select(p => p.Partition.Value)) +
                        "], all: [" +
                        string.Join(",", c.Assignment.Concat(partitions).Select(p => p.Partition.Value)) +
                        "]");

                    // Possibly manually specify start offsets by returning a list of topic/partition/offsets
                    // to assign to, e.g.:
                    // return partitions.Select(tp => new TopicPartitionOffset(tp, externalOffsets[tp]));
                })
                .SetPartitionsRevokedHandler((c, partitions) =>
                {
                    // Since a cooperative assignor (CooperativeSticky) has been configured, the revoked
                    // assignment is incremental (may remove only some partitions of the current assignment).
                    var remaining = c.Assignment.Where(atp => partitions.Where(rtp => rtp.TopicPartition == atp).Count() == 0);
                    Console.WriteLine(
                        "Partitions incrementally revoked: [" +
                        string.Join(",", partitions.Select(p => p.Partition.Value)) +
                        "], remaining: [" +
                        string.Join(",", remaining.Select(p => p.Partition.Value)) +
                        "]");
                })
                .SetPartitionsLostHandler((c, partitions) =>
                {
                    // The lost partitions handler is called when the consumer detects that it has lost ownership
                    // of its assignment (fallen out of the group).
                    Console.WriteLine($"Partitions were lost: [{string.Join(", ", partitions)}]");
                })
                .Build())
            {
                consumer.Subscribe(topics);


                try
                {
                    while (true)
                    {
                        try
                        {
                            var consumeResult = consumer.Consume(cancellationToken);

                            if (consumeResult.IsPartitionEOF)
                            {
                                Console.WriteLine(
                                    $"Reached end of topic {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");

                                continue;
                            }

                            Console.WriteLine($"Received message at {consumeResult.TopicPartitionOffset}: {consumeResult.Message.Value}");
                            try
                            {
                                // Store the offset associated with consumeResult to a local cache. Stored offsets are committed to Kafka by a background thread every AutoCommitIntervalMs. 
                                // The offset stored is actually the offset of the consumeResult + 1 since by convention, committed offsets specify the next message to consume. 
                                // If EnableAutoOffsetStore had been set to the default value true, the .NET client would automatically store offsets immediately prior to delivering messages to the application. 
                                // Explicitly storing offsets after processing gives at-least once semantics, the default behavior does not.
                                consumer.StoreOffset(consumeResult);
                            }
                            catch (KafkaException e)
                            {
                                Console.WriteLine($"Store Offset error: {e.Error.Reason}");
                            }
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Consume error: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Closing consumer.");
                    consumer.Close();
                }
            }


        }


        public override void Stop()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "Stop");
            if (_listenerThread != null)
            {
                _listenerThread.Abort();
                _log.Info(GetHashCode(), "Stopped subscribing to topic ");
            }

            IsRunning = false;

        }
    }
}
