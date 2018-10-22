using System;

namespace NeuralNetworkVisualizer.Preferences.Formatting
{
    /// <summary>
    /// Build a default formatter
    /// </summary>
    public class BasicFormatter<T> : FormatterBase<T>
    {
        public BasicFormatter(Func<T> formaterFunc) : base((v) => formaterFunc())
        {
            this.FormaterFunc = formaterFunc;
        }

        public Func<T> FormaterFunc { get; }
    }
}
