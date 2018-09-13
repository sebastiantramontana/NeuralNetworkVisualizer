using NeuralNetworkVisualizer.Preferences;
using System.Drawing;

namespace NeuralNetworkVisualizer.Drawing.Cache
{
    internal class PerceptronSizesPreCalc : NodeSizesPreCalc
    {
        private readonly Preference _preferences;

        public PerceptronSizesPreCalc(Preference preferences)
        {
            _preferences = preferences;
        }

        private Size? _sumSize = null;
        internal Size SumSize
        {
            get
            {
                if (!_sumSize.HasValue)
                {
                    var valuesHeight = this.Div3 / 2 - _preferences.NodeMargins;
                    var valuesWidth = valuesHeight * 5.23; //buena proporción
                    _sumSize = new Size((int)valuesWidth, (int)valuesHeight);
                }

                return _sumSize.Value;
            }
        }

        private Size? _activationFunctionSize = null;
        internal Size ActivationFunctionSize
        {
            get
            {
                if (!_activationFunctionSize.HasValue)
                {
                    var div_3 = (int)this.Div3;
                    _activationFunctionSize = new Size(div_3, div_3);
                }

                return _activationFunctionSize.Value;
            }
        }


        private double? _ellipseHeightDiv2 = null;
        internal int GetInputPositionY(int fromY)
        {
            if (!_ellipseHeightDiv2.HasValue)
            {
                _ellipseHeightDiv2 = this.EllipseRectangle.Value.Height / 2;
            }

            return (int)(fromY + _ellipseHeightDiv2.Value);
        }

        private double? _div3 = null;
        private double Div3
        {
            get
            {
                if (!_div3.HasValue)
                {
                    _div3 = this.EllipseRectangle.Value.Height / 3;
                }

                return _div3.Value;
            }
        }

        internal Point GetActivationFunctionPosition(Rectangle origin)
        {
            return new Point((int)(origin.X + this.Div3), (int)(origin.Y + this.Div3));
        }

        private double? _ouputPositionYOffset = null;
        internal int GetOutputPositionY(int fromY)
        {
            if (!_ouputPositionYOffset.HasValue)
            {
                _ouputPositionYOffset = this.Div3 * 2 + _preferences.NodeMargins;
            }

            return (int)(fromY + _ouputPositionYOffset.Value);
        }
    }
}
