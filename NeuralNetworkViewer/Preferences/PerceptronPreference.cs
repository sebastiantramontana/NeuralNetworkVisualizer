using NeuralNetworkVisualizer.Preferences.Text;

namespace NeuralNetworkVisualizer.Preferences
{
    public class PerceptronPreference : NodePreference
    {
        private TextValueFormatter _sumValueFormatter;
        public TextValueFormatter SumValueFormatter
        {
            get => _sumValueFormatter ?? (_sumValueFormatter = new TextValueFormatter());
            set => _sumValueFormatter = value;
        }
    }
}
