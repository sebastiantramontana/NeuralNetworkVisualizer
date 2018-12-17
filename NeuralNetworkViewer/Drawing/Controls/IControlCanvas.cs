using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Drawing.Controls
{
    internal interface IControlCanvas
    {
        Size Size { get; set; }
        Image Image { get; set; }
        NeuralNetworkVisualizerControl Control { get; }
        bool IsReady { get; }

        void SetBlank();
        (Graphics Graph, Image Image) GetGraphics();
        Size GetLayersDrawingSize();

        void SafeInvoke(Action action);
        T SafeInvoke<T>(Func<T> action);
    }
}
