using System;

namespace NeuralNetworkVisualizer.Preferences.Text
{
    public class TextValueFormatter
    {
        private readonly Func<double, TextPreference> _formaterFunc;

        /// <summary>
        /// Build a default formatter
        /// </summary>
        public TextValueFormatter()
        {
            _formaterFunc = (v) => new TextPreference();
        }

        /// <summary>
        /// Build a NEW custom formatter by passed value. Don't reuse it, will be disposed
        /// </summary>
        /// <param name="customFormaterFunc"></param>
        public TextValueFormatter(Func<double, TextPreference> customFormaterFunc)
        {
            _formaterFunc = customFormaterFunc;
        }

        /// <summary>
        /// Build a formatter by value sign. The three args are buiders: Create a new instance for each. Don't reuse them, they will be disposed
        /// </summary>
        /// <param name="whenNegative"></param>
        /// <param name="whenZero"></param>
        /// <param name="whenPositive"></param>
        public TextValueFormatter(Func<TextPreference> whenNegative, Func<TextPreference> whenZero, Func<TextPreference> whenPositive)
        {
            _formaterFunc = (v) => (v < 0.0 ? whenNegative() :
                                    (v == 0.0 ? whenZero() : whenPositive()));
        }

        public TextPreference GetFormat(double value)
        {
            return _formaterFunc(value);
        }
    }
}
