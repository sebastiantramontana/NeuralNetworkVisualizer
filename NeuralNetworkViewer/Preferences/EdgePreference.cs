using NeuralNetworkVisualizer.Preferences.Formatting;
using NeuralNetworkVisualizer.Preferences.Text;
using System.Drawing;

namespace NeuralNetworkVisualizer.Preferences
{
    public class EdgePreference
    {
        private IFormatter<TextPreference> _valueFormatter;
        public IFormatter<TextPreference> ValueFormatter
        {
            get => _valueFormatter ?? (_valueFormatter = new BasicFormatter<TextPreference>(() => new TextPreference()));
            set => _valueFormatter = value;
        }

        private IFormatter<Pen> _connectorFormatter = new BasicFormatter<Pen>(() => new Pen(Color.Black));
        /// <summary>
        /// The Pen for connector: Don't use a System Pen, but clone it!
        /// </summary>
        public IFormatter<Pen> Connector
        {
            get => _connectorFormatter ?? (_connectorFormatter = new BasicFormatter<Pen>(() => new Pen(Color.Transparent)));
            set => _connectorFormatter = value;
        }

        public byte RoundingDigits { get; set; } = 3;
    }
}
