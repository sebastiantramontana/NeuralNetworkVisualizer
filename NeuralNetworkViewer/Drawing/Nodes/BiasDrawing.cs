using NeuralNetworkVisualizer.Drawing.Cache;
using NeuralNetworkVisualizer.Model.Nodes;
using NeuralNetworkVisualizer.Preferences;
using NeuralNetworkVisualizer.Selection;

namespace NeuralNetworkVisualizer.Drawing.Nodes
{
    internal class BiasDrawing : SimpleNodeDrawing<Bias>
    {
        internal BiasDrawing(Bias element, Preference preferences, SimpleNodeSizesPreCalc cache, ISelectableElementRegister selectableElementRegister, IElementSelectionChecker selectionChecker) : base(element, preferences.Biases, cache, selectionChecker, selectableElementRegister)
        {
        }
    }
}
