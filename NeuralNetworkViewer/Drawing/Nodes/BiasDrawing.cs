using NeuralNetworkVisualizer.Drawing.Cache;
using NeuralNetworkVisualizer.Model.Nodes;
using NeuralNetworkVisualizer.Preferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Drawing.Nodes
{
    internal class BiasDrawing : SimpleNodeDrawing<Bias>
    {
        internal BiasDrawing(Bias element, Preference preferences, SimpleNodeSizesPreCalc cache) : base(element, preferences.Biases, cache)
        {
        }
    }
}
