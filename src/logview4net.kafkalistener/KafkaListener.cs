using logview4net.Listeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logview4net.kafkalistener
{
    public class KafkaListener : ListenerBase
    {
        public override void Dispose()
        {
            throw new NotImplementedException();
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
