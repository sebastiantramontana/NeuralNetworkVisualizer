using NeuralNetworkVisualizer.Drawing.Canvas;
using NeuralNetworkVisualizer.Model;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NeuralNetworkVisualizer.Selection
{
    internal class SelectableElementRegister : ISelectableElementRegister, ISelectionResolver
    {
        private readonly IDictionary<Element, RegistrationInfo> _registeredElements;
        private ICanvas _currentRootCanvas;

        public SelectableElementRegister()
        {
            _registeredElements = new Dictionary<Element, RegistrationInfo>();
        }

        public Element GetElementFromLocation(Point location)
        {
            return _registeredElements
                .Values
                .Where(ri => ri.Region
                    .IsVisible(_currentRootCanvas
                        .Translate(location, ri.Canvas)))
                .OrderByDescending(ri => ri.ZIndex)
                .FirstOrDefault()?
                .Element;
        }

        public void Register(RegistrationInfo info)
        {
            _registeredElements[info.Element] = info;
        }

        public void SetCurrentRootCanvas(ICanvas currentRootCanvas)
        {
            _currentRootCanvas = currentRootCanvas;
        }
    }
}
