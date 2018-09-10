using System;

namespace NeuralNetworkVisualizer.Preferences.Formatting
{
    public class Formatter<T>
    {
        private readonly Func<double, T> _formaterFunc;

        /// <summary>
        /// Build a default formatter
        /// </summary>
        public Formatter(Func<T> basicBuilder)
        {
            _formaterFunc = (v) => basicBuilder();
        }

        /// <summary>
        /// Build a NEW custom formatter by passed value. Don't reuse it, will be disposed
        /// </summary>
        /// <param name="customFormaterFunc"></param>
        public Formatter(Func<double, T> customFormaterFunc)
        {
            _formaterFunc = customFormaterFunc;
        }

        /// <summary>
        /// Build a formatter by value sign. The three args are buiders: Create a new instance for each. Don't reuse them, they will be disposed
        /// </summary>
        /// <param name="whenNegative"></param>
        /// <param name="whenZero"></param>
        /// <param name="whenPositive"></param>
        public Formatter(Func<T> whenNegative, Func<T> whenZero, Func<T> whenPositive)
        {
            _formaterFunc = (v) => (v < 0.0 ? whenNegative() :
                                    (v == 0.0 ? whenZero() : whenPositive()));
        }

        public T GetFormat(double value)
        {
            return _formaterFunc(value);
        }
    }
}
