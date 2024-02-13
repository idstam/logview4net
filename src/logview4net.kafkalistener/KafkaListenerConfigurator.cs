using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using logview4net.Listeners;
using System.Data.Common;
using System.IO;
using System.Xml.Serialization;

namespace logview4net.kafkalistener
{
    public partial class KafkaListenerConfigurator: UserControl, IListenerConfigurator
    {
        private ILog _log = Logger.GetLogger("logview4net.Listeners.KafkaListenerConfigurator");
        private KafkaListener _listener = new KafkaListener();

        public KafkaListenerConfigurator()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "KafkaListenerConfigurator");
            InitializeComponent();
        }

        public KafkaListenerConfigurator(ListenerBase listner)
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "KafkaListenerConfigurator(ListenerBase)");
            InitializeComponent();
            _listener = (KafkaListener)listner;
            UpdateControls();


        }

        public string Caption =>  "Kafka Listener: " + _listener.MessagePrefix ;

        public string Configuration
        {
            get { return _listener.GetConfiguration(); }
            set
            {
                if (_log.Enabled) _log.Debug(GetHashCode(), "Configuration Set");
                var xs = new XmlSerializer(_listener.GetType());
                var sr = new StringReader(value);
                _listener = (KafkaListener)xs.Deserialize(sr);

                UpdateControls();
            }
        }
        public ListenerBase ListenerBase
        {
            get { return _listener; }
            set { _listener = (KafkaListener)value; }
        }

        public void UpdateControls()
        {
            if (_log.Enabled) _log.Debug(GetHashCode(), "UpdateControls");
        }

        private void KafkaListenerConfigurator_Load(object sender, EventArgs e)
        {

        }
    }
}
