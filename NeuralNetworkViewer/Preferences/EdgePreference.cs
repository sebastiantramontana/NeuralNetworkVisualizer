using NeuralNetworkVisualizer.Preferences.Formatting;
using NeuralNetworkVisualizer.Preferences.Pens;
using NeuralNetworkVisualizer.Preferences.Text;
using System.Drawing;
using Draw = System.Drawing;

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

        private IPen _whenSelected = new SimplePen(Draw.Pens.Orange);
        public IPen WhenSelected
        {
            get => _whenSelected ?? (_whenSelected = new SimplePen(Draw.Pens.Transparent));
            set => _whenSelected = value;
        }

        public byte RoundingDigits { get; set; } = 3;
    }
}
