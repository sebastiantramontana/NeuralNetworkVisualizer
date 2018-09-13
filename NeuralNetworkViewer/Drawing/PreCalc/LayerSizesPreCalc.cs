using NeuralNetworkVisualizer.Preferences;
using System.Drawing;

namespace NeuralNetworkVisualizer.Drawing.Cache
{
    internal class LayerSizesPreCalc
    {
        internal LayerSizesPreCalc(Size canvasSize, int maxNodes, Preference preferences)
        {
            var doubleNodeMargin = preferences.NodeMargins * 2;

            this.TotalNodesHeight = canvasSize.Height - (preferences.Layers.Title.Height + doubleNodeMargin);
            this.NodeHeight = this.TotalNodesHeight / maxNodes;
            this.NodeEllipseHeight = this.NodeHeight - preferences.NodeMargins / maxNodes;
            this.StartingY = preferences.Layers.Title.Height + preferences.NodeMargins;
            this.NodeWidth = canvasSize.Width - doubleNodeMargin;
        }

        internal int NodeWidth { get; private set; }
        internal int NodeHeight { get; private set; }
        internal int NodeEllipseHeight { get; private set; }
        internal int TotalNodesHeight { get; private set; }
        internal int StartingY { get; private set; }
    }
}
