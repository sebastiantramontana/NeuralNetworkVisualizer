using NeuralNetworkVisualizer.Exceptions;
using NeuralNetworkVisualizer.Model.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Model.Layers
{
    public abstract class LayerBase : Element
    {
        internal LayerBase(string id) : base(id)
        {

        }

        public PerceptronLayer Next { get; private set; }

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
            if (nextLayer == null)
                throw new ArgumentNullException("nextLayer argument is null");

            ValidateId(nextLayer.Id);

            if (nextLayer.Next != null)
            {
                nextLayer.Next.Previous = nextLayer.Previous;
            }

            if (nextLayer.Previous != null)
            {
                nextLayer.Previous.Next = nextLayer.Next;
            }

            if (this.Next != null)
            {
                this.Next.Previous = nextLayer;
                nextLayer.Next = this.Next;
            }

            this.Next = nextLayer;
            nextLayer.Previous = this;

            ConnectChild(nextLayer);
        }

        private protected abstract void ConnectChild(PerceptronLayer nextLayer);
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
