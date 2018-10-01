using NeuralNetworkVisualizer.Model.Nodes;
using System;
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

        protected void ConnectNodeToNextLayer(NodeBase node)
        {
            //Connect the edges... they are perceptrons
            ConnectNodeToNextLayer(node, this.Next);
        }

        protected void ConnectNodeToNextLayer(NodeBase previousNode, PerceptronLayer nextLayer)
        {
            //Connect the edges...
            if (nextLayer == null)
                return;

            var nextPerceptrons = nextLayer.Nodes; //they are perceptrons

            foreach (var nextPerceptron in nextPerceptrons)
            {
                nextPerceptron.EdgesInternal.Add(Edge.Create(previousNode, nextPerceptron));
            }
        }

        protected void DisconnectNodeFromNextLayer(NodeBase node)
        {
            //Disconnect the edges...
            if (this.Next == null || node == null)
                return;

            var nextPerceptrons = this.Next.Nodes;

            foreach (var nextPerceptron in nextPerceptrons)
            {
                nextPerceptron.EdgesInternal.Remove(nextPerceptron.Edges.Single(e => e.Source == node));
            }
        }

        protected void SetNodeToLayer(NodeBase node)
        {
            if (node.Layer != null)
                throw new InvalidOperationException($"Node '{node.Id}' already belongs to another layer. Remove the node from the other layer, then add to this one.");

            node.Layer = this;
        }

        protected void SetNewBias(Bias bias)
        {
            if (bias == null)
            {

                DisconnectNodeFromNextLayer(this.Bias);
            }
            else
            {
                SetNodeToLayer(bias);
                ConnectNodeToNextLayer(bias);
            }
        }

        private protected override void ValidateDuplicatedIChild(IDictionary<string, Element> acumulatedIds)
        {
            foreach (var node in this.GetAllNodes())
                node.ValidateId(acumulatedIds);

            if (this.Next != null)
                this.Next.ValidateId(acumulatedIds);
        }

        public abstract IEnumerable<NodeBase> GetAllNodes();
        public abstract void RemoveNode(string nodeId);

        private protected abstract void ConnectChild();

    }
}
