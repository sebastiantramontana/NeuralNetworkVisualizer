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

        internal protected override Element FindByIdRecursive(string id)
        {
            return null;
        }

        protected internal override bool ValidateDuplicatedIdRecursive(string id)
        {
            return (this.Id != id);
        }
    }
}
