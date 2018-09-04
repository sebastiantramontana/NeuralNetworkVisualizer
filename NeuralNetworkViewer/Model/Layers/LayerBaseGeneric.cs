using NeuralNetworkVisualizer.Model.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Model.Layers
{
    public abstract class LayerBase<TNode> : LayerBase where TNode : NodeBase
    {
        private readonly IDictionary<string, TNode> _nodes;
        internal LayerBase(string id) : base(id)
        {
            _nodes = new Dictionary<string, TNode>();
        }
        public IEnumerable<TNode> Nodes { get => _nodes.Values; }

        public void AddNode(TNode node)
        {
            SetNodeToLayer(node);
            _nodes.Add(node.Id, node);

            ConnectNodeToNextLayer(node);
            AddNodeChild(node);
        }

        private void SetNodeToLayer(NodeBase node)
        {
            if (node.Layer != null)
                throw new InvalidOperationException($"Node '{node.Id}' already belongs to another layer. Remove the node from the other layer, then add to this one.");

            node.Layer = this;
        }

        public void RemoveNode(TNode node)
        {
            _nodes.Remove(node.Id);

            DisconnectNodeFromNextLayer(node);
            RemoveNodeChild(node);
        }
        public void RemoveNode(string nodeId)
        {
            if (!_nodes.TryGetValue(nodeId, out TNode node))
                return;

            RemoveNode(node);
        }
        public override IEnumerable<NodeBase> GetAllNodes()
        {
            var nodes = new List<NodeBase>(_nodes.Values);

            if (this.Bias != null)
                nodes.Add(this.Bias);

            return nodes;
        }

        private void ConnectNodeToNextLayer(NodeBase node)
        {
            //Connect the edges... they are perceptrons
            ConnectNodeToNextLayer(node, this.Next);
        }

        private void ConnectNodeToNextLayer(NodeBase node, PerceptronLayer nextLayer)
        {
            //Connect the edges...
            if (nextLayer == null)
                return;

            var nextPerceptrons = nextLayer.Nodes; //they are perceptrons

            foreach (var nextPerceptron in nextPerceptrons)
            {
                nextPerceptron.EdgesInternal.Add(Edge.Create(node, nextPerceptron));
            }
        }

        private void DisconnectNodeFromNextLayer(TNode node)
        {
            //Disconnect the edges...
            if (this.Next == null)
                return;

            var nextPerceptrons = this.Next.Nodes;

            foreach (var nextPerceptron in nextPerceptrons)
            {
                nextPerceptron.EdgesInternal.Remove(nextPerceptron.Edges.Single(e => e.Source == node));
            }
        }

        private protected override void ConnectChild(PerceptronLayer nextLayer)
        {
            foreach (var node in this.GetAllNodes())
            {
                ConnectNodeToNextLayer(node, nextLayer);
            }
        }

        private protected virtual void AddNodeChild(TNode node) { }
        private protected virtual void RemoveNodeChild(TNode node) { }

        private protected override void SetNewBias(Bias bias)
        {
            if (bias != null)
            {
                ValidateId(bias.Id);
            }

            SetNodeToLayer(bias);
            ConnectNodeToNextLayer(bias);
        }
    }
}
