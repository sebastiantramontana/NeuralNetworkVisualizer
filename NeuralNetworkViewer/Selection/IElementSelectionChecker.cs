using NeuralNetworkVisualizer.Model;

namespace NeuralNetworkVisualizer.Selection
{
    internal interface IElementSelectionChecker
    {
        bool IsSelected(Element element);
    }
}
