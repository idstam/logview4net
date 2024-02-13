/*
 * This file is part of logview4net (logview4net.sourceforge.net)
 * Copyright 2024 Johan Idstam
 *
 *
 * This source code is released under the Artistic License 2.0.
 */

using logview4net.Listeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
