using NeuralNetworkVisualizer.Model;
using System.Collections.Generic;
using System.Drawing;

namespace NeuralNetworkVisualizer.Selection
{
    internal interface IElementSelector : IElementSelectionChecker
    {
        IEnumerable<Element> SelectedElements { get; }

        Element SelectOnly(Point location);
        Element AddToSelection(Point location);
        Element Unselect(Point location);
    }
}
