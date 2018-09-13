using NeuralNetworkVisualizer.Preferences.Text;
using System.Drawing;

namespace NeuralNetworkVisualizer.Preferences
{
    public class EdgePreference
    {
        private Formatter<TextPreference> _valueFormatter;
        public Formatter<TextPreference> ValueFormatter
        {
            get => _valueFormatter ?? (_valueFormatter = new Formatter<TextPreference>(() => new TextPreference()));
            set => _valueFormatter = value;
        }

        private Formatter<Pen> _connectorFormatter = new Formatter<Pen>(() => new Pen(Color.Black));
        /// <summary>
        /// The Pen for connector: Don't use a System Pen, but clone it!
        /// </summary>
        public Formatter<Pen> Connector
        {
            get => _connectorFormatter ?? (_connectorFormatter = new Formatter<Pen>(() => new Pen(Color.Transparent)));
            set => _connectorFormatter = value;
        }

        public byte RoundingDigits { get; set; } = 3;
    }
}
