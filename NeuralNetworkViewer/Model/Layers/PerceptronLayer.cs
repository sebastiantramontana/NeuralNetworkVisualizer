using NeuralNetworkVisualizer.Exceptions;
using NeuralNetworkVisualizer.Model.Nodes;

namespace NeuralNetworkVisualizer.Model.Layers
{
    public class PerceptronLayer : LayerBase<Perceptron>
    {
        public PerceptronLayer(string id) : base(id)
        {

        }

        public LayerBase Previous { get; internal set; }

        public void Disconnect()
        {
            if (this.Previous != null)
            {
                if (this.Next != null)
                {
                    this.Previous.Next = this.Next;
                    this.Next.Previous = this.Previous;
                    this.Previous.Reconnect();
                }
                else
                {
                    this.Previous.Next = null;
                }
            }
            else
            {
                if (this.Next != null)
                {
                    this.Next.Previous = null;
                    this.Next.RemoveEdgesLayer();
                }
            }

            RemoveEdgesLayer();
            this.Previous = null;
            this.Next = null;
        }

        internal void RemoveEdgesLayer()
        {
            foreach (var node in this.Nodes)
            {
                node.EdgesInternal.Clear();
            }
        }

        private protected override void AddNodeChild(Perceptron perceptron)
        {
            ConnectPerceptronToPreviousLayer(perceptron);
        }

        private protected override void RemoveNodeChild(Perceptron perceptron)
        {
            perceptron.EdgesInternal.Clear();
        }

        private void ConnectPerceptronToPreviousLayer(Perceptron perceptron)
        {
            perceptron.EdgesInternal.Clear();

            if (this.Previous == null)
                return;

            foreach (var previousNode in this.Previous.GetAllNodes())
            {
                perceptron.EdgesInternal.Add(Edge.Create(previousNode, perceptron));
            }
        }
    }
}
