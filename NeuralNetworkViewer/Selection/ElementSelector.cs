using NeuralNetworkVisualizer.Model;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NeuralNetworkVisualizer.Selection
{
    internal class ElementSelector : IElementSelector
    {
        private readonly ICollection<Element> _selectedElements;
        private readonly ISelectionResolver _selectionResolver;

        internal ElementSelector(ISelectionResolver selectionResolver)
        {
            _selectedElements = new List<Element>();
            _selectionResolver = selectionResolver;
        }

        public IEnumerable<Element> SelectedElements => _selectedElements.ToArray();

        public bool IsSelected(Element element)
        {
            return _selectedElements.Contains(element);
        }

        public Element AddToSelection(Point location)
        {
            var elem = _selectionResolver.GetElementFromLocation(location);

            if (elem != null && !_selectedElements.Contains(elem))
            {
                _selectedElements.Add(elem);
            }

            return elem;
        }

        public Element SelectOnly(Point location)
        {
            _selectedElements.Clear();

            return AddToSelection(location);
        }

        public Element Unselect(Point location)
        {
            var elem = _selectionResolver.GetElementFromLocation(location);

            if (elem == null || !_selectedElements.Contains(elem))
            {
                return null;
            }

            _selectedElements.Remove(elem);
            return elem;
        }

        public void UnselectAll()
        {
            _selectedElements.Clear();
        }
    }
}
