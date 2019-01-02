using NeuralNetworkVisualizer.Model;
using NeuralNetworkVisualizer.Selection;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace NeuralNetworkVisualizer.Drawing.Controls
{
    internal class SelectionEventFiring
    {
        private readonly NeuralNetworkVisualizerControl _control;
        private readonly IElementSelector _selector;

        internal SelectionEventFiring(NeuralNetworkVisualizerControl control, IElementSelector selector)
        {
            _control = control;
            _selector = selector;
        }

        private void FireSelectionEvent(object sender, MouseEventArgs e)
        {
            if (!_control.Selectable)
                return;

            Func<Point, Element> selectFunc;
            bool isSelected;

            switch (Control.ModifierKeys)
            {
                case Keys.Control:
                    selectFunc = _selector.Unselect;
                    isSelected = false;
                    break;
                case Keys.Shift:
                    selectFunc = _selector.AddToSelection;
                    isSelected = true;
                    break;
                default:
                    selectFunc = _selector.SelectOnly;
                    isSelected = true;
                    break;
            }

            var element = selectFunc(e.Location);

            if (element == null)
            {
                return;
            }

            FireSelectionEvent(element, isSelected);
            _control.Redraw();
        }

        private void FireSelectionEvent(Element element, bool isSelected)
        {
            if (_control.FireSelectionEvent(element, isSelected, _control.SelectInputLayer))
                return;

            if (FireSelectionEvent(element, isSelected, SelectPerceptronLayer))
                return;

            if (FireSelectionEvent(element, isSelected, SelectBias))
                return;

            if (FireSelectionEvent(element, isSelected, SelectInput))
                return;

            if (FireSelectionEvent(element, isSelected, SelectPerceptron))
                return;

            if (FireSelectionEvent(element, isSelected, SelectEdge))
                return;
        }
    }
}
