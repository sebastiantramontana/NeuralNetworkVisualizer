using NeuralNetworkVisualizer.Model;
using NeuralNetworkVisualizer.Selection;
using System;
using System.Drawing;
using System.Windows.Forms;
using SelectBias = System.Func<System.EventHandler<NeuralNetworkVisualizer.Selection.SelectionEventArgs<NeuralNetworkVisualizer.Model.Nodes.Bias>>>;
using SelectEdge = System.Func<System.EventHandler<NeuralNetworkVisualizer.Selection.SelectionEventArgs<NeuralNetworkVisualizer.Model.Nodes.Edge>>>;
using SelectInput = System.Func<System.EventHandler<NeuralNetworkVisualizer.Selection.SelectionEventArgs<NeuralNetworkVisualizer.Model.Nodes.Input>>>;
using SelectInputLayer = System.Func<System.EventHandler<NeuralNetworkVisualizer.Selection.SelectionEventArgs<NeuralNetworkVisualizer.Model.Layers.InputLayer>>>;
using SelectPerceptron = System.Func<System.EventHandler<NeuralNetworkVisualizer.Selection.SelectionEventArgs<NeuralNetworkVisualizer.Model.Nodes.Perceptron>>>;
using SelectPerceptronLayer = System.Func<System.EventHandler<NeuralNetworkVisualizer.Selection.SelectionEventArgs<NeuralNetworkVisualizer.Model.Layers.PerceptronLayer>>>;


namespace NeuralNetworkVisualizer.Drawing.Controls
{
    internal class SelectionEventFiring : ISelectionEventFiring
    {
        private readonly NeuralNetworkVisualizerControl _control;
        private readonly IElementSelector _selector;

        private readonly SelectInputLayer _selectInputLayer;
        private readonly SelectPerceptronLayer _selectPerceptronLayer;
        private readonly SelectBias _selectBias;
        private readonly SelectInput _selectInput;
        private readonly SelectPerceptron _selectPerceptron;
        private readonly SelectEdge _selectEdge;

        internal SelectionEventFiring(NeuralNetworkVisualizerControl control, IElementSelector selector,
            SelectInputLayer selectInputLayer,
            SelectPerceptronLayer selectPerceptronLayer,
            SelectBias selectBias,
            SelectInput selectInput,
            SelectPerceptron selectPerceptron,
            SelectEdge selectEdge)
        {
            _control = control;
            _selector = selector;

            _selectInputLayer = selectInputLayer;
            _selectPerceptronLayer = selectPerceptronLayer;
            _selectBias = selectBias;
            _selectInput = selectInput;
            _selectPerceptron = selectPerceptron;
            _selectEdge = selectEdge;
        }

        public void FireSelectionEvent(Point location)
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

            var element = selectFunc(location);

            if (element == null)
            {
                return;
            }

            FireSelectionEvent(element, isSelected);
            _control.Redraw();
        }

        private void FireSelectionEvent(Element element, bool isSelected)
        {
            if (FireSelectionEvent(element, isSelected, _selectInputLayer))
                return;

            if (FireSelectionEvent(element, isSelected, _selectPerceptronLayer))
                return;

            if (FireSelectionEvent(element, isSelected, _selectBias))
                return;

            if (FireSelectionEvent(element, isSelected, _selectInput))
                return;

            if (FireSelectionEvent(element, isSelected, _selectPerceptron))
                return;

            if (FireSelectionEvent(element, isSelected, _selectEdge))
                return;
        }

        private bool FireSelectionEvent<TElement>(Element element, bool isSelected, Func<EventHandler<SelectionEventArgs<TElement>>> eventFunc) where TElement : Element
        {
            var fired = false;

            if (element is TElement typedElement)
            {
                var eventHandler = eventFunc();
                eventHandler?.Invoke(_control, new SelectionEventArgs<TElement>(typedElement, isSelected));
                fired = true;
            }

            return fired;
        }
    }
}
