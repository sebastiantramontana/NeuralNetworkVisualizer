using NeuralNetworkVisualizer.Preferences.Formatting;
using NeuralNetworkVisualizer.Preferences.Text;

namespace NeuralNetworkVisualizer.Preferences
{
    public class PerceptronPreference : NodePreference
    {
        private IFormatter<TextPreference> _sumValueFormatter;
        public IFormatter<TextPreference> SumValueFormatter
        {
            get => _sumValueFormatter ?? (_sumValueFormatter = new BasicFormatter<TextPreference>(() => new TextPreference()));
            set => _sumValueFormatter = value;
        }
    }
}
