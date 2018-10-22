using System;

namespace NeuralNetworkVisualizer.Preferences.Formatting
{
    /// <summary>
    /// Build a NEW custom formatter by passed value.
    /// </summary>
    public class CustomFormatter<T> : FormatterBase<T>
    {
        public CustomFormatter(Func<double?, T> formaterFunc) : base(formaterFunc)
        {
            this.FormaterFunc = formaterFunc;
        }

        public Func<double?, T> FormaterFunc { get; private set; }
    }
}
