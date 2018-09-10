using NeuralNetworkVisualizer.Preferences.Formatting;

namespace NeuralNetworkVisualizer.Preferences
{
    public class PerceptronPreference : NodePreference
    {
        private Formatter<TextPreference> _sumValueFormatter;
        public Formatter<TextPreference> SumValueFormatter
        {
            get => _sumValueFormatter ?? (_sumValueFormatter = new Formatter<TextPreference>(() => new TextPreference()));
            set => _sumValueFormatter = value;
        }
    }
}
