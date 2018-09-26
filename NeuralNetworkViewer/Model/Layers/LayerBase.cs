using NeuralNetworkVisualizer.Model.Nodes;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworkVisualizer.Model.Layers
{
    public abstract class LayerBase : Element
    {
        internal LayerBase(string id) : base(id)
        {

        }

        public PerceptronLayer Next { get; internal set; }

        private Bias _Bias = null;
        public Bias Bias
        {
            get { return _Bias; }
            set
            {
                SetNewBias(value);
                _Bias = value;
            }
        }

        public void Connect(PerceptronLayer nextLayer)
        {
            if (nextLayer == null) //Cut off the layer link
            {
                if (this.Next != null)
                {
                    this.Next.Previous = null; //cancel the Next's Previous layer (this) 
                }

                this.Next = null;
                return;
            }

            ValidateId(nextLayer.Id);

            if (this.Next != null)
            {
                this.Next.Previous = nextLayer;
                nextLayer.Next = this.Next;
            }

            this.Next = nextLayer;
            nextLayer.Previous = this;

            ConnectChild();
        }

        internal void Reconnect()
        {
            ConnectChild();
        }

        private protected abstract void ConnectChild();
        private protected abstract void ValidateId(string id);
        private protected abstract void SetNewBias(Bias bias);

        internal protected override Element FindByIdRecursive(string id)
        {
            var nodes = GetAllNodes();
            Element elem = nodes.SingleOrDefault(n => n.Id == id);

            if (elem != null)
                return elem;

            foreach (var node in nodes)
            {
                elem = node.FindByIdRecursive(id);

                if (elem != null)
                    break;
            }

            return elem;
        }

        protected internal override bool ValidateDuplicatedIdRecursive(string id)
        {
            return (this.Id != id && this.GetAllNodes().All(n => n.ValidateDuplicatedIdRecursive(id)));
        }

        public abstract IEnumerable<NodeBase> GetAllNodes();
    }
}
