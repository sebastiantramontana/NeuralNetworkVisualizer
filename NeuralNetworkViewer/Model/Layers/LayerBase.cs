using NeuralNetworkVisualizer.Model.Nodes;
using System.Collections.Generic;

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

        private protected override void ValidateDuplicatedIChild(IDictionary<string, Element> acumulatedIds)
        {
            foreach (var node in this.GetAllNodes())
                node.ValidateId(acumulatedIds);

            if (this.Next != null)
                this.Next.ValidateId(acumulatedIds);
        }

        private protected abstract void ConnectChild();
        private protected abstract void SetNewBias(Bias bias);

        public abstract IEnumerable<NodeBase> GetAllNodes();
    }
}
