using System;

namespace NeuralNetworkVisualizer.Preferences.Formatting
{
    public abstract class FormatterBase<T> : IFormatter<T>
    {
        /// <summary>
        /// It needs to be a builder, because T object can be create, then disposed. Disposed reference object couldn´t be reused.
        /// </summary>
        private readonly Func<double?, T> _formaterFunc;

        internal protected FormatterBase(Func<double?, T> formaterFunc)
        {
            _formaterFunc = formaterFunc;
        }

        public T GetFormat(double? value)
        {
            return _formaterFunc(value);
        }
    }
}
