using NeuralNetworkVisualizer.Model;
using System;

namespace NeuralNetworkVisualizer.Selection
{
    public class SelectionEventArgs<TElement> : EventArgs where TElement : Element
    {
        public SelectionEventArgs(TElement element, bool isSelected)
        {
            this.Element = element;
            this.IsSelected = isSelected;
        }

        public TElement Element { get; }
        public bool IsSelected { get; }
    }
}
