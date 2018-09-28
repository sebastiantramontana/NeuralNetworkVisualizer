using NeuralNetworkVisualizer.Model.Layers;
using System;

namespace NeuralNetworkVisualizer.Model.Nodes
{
    public abstract class NodeBase : Element
    {
        public NodeBase(string id) : base(id)
        {

        }
        public double? OutputValue { get; set; }
        public LayerBase Layer { get; internal set; }
    }
}
