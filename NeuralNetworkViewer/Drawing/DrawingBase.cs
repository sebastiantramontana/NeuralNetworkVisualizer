using NeuralNetworkVisualizer.Drawing.Canvas;
using NeuralNetworkVisualizer.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Drawing
{
    internal interface IDrawing
    {
        void Draw(ICanvas canvas);
    }

    internal abstract class DrawingBase<TElement> : IDrawing where TElement : Element
    {
        internal DrawingBase(TElement element)
        {
            this.Element = element;
        }

        internal TElement Element { get; private set; }
        public abstract void Draw(ICanvas canvas);
    }
}
