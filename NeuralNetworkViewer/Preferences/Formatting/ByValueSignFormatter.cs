using System;

namespace NeuralNetworkVisualizer.Preferences.Formatting
{
    /// <summary>
    /// Build a formatter by value sign. The three args are buiders: Create a new instance for each. Don't reuse them, they could be disposed
    /// </summary>
    public class ByValueSignFormatter<T> : FormatterBase<T>
    {
        public ByValueSignFormatter(Func<T> whenNegative, Func<T> whenZero, Func<T> whenPositive, Func<T> whenNull)
            : base((v) => (!v.HasValue ? whenNull() :
                     (v.Value < 0.0 ? whenNegative() :
                     (v.Value == 0.0 ? whenZero() : whenPositive()))))
        {
            this.WhenNegative = whenNegative;
            this.WhenZero = whenZero;
            this.WhenPositive = whenPositive;
            this.WhenNull = whenNull;
        }

        public Func<T> WhenNegative { get; private set; }
        public Func<T> WhenZero { get; private set; }
        public Func<T> WhenPositive { get; private set; }
        public Func<T> WhenNull { get; private set; }
    }
}
