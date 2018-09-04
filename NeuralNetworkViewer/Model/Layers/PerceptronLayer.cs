using NeuralNetworkVisualizer.Exceptions;
using NeuralNetworkVisualizer.Model.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Model.Layers
{
    public class PerceptronLayer : LayerBase<Perceptron>
    {
        public PerceptronLayer(string id) : base(id)
        {

        }

        public LayerBase Previous { get; internal set; }

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

        private protected override void ValidateId(string id)
        {
            for (LayerBase layer = this; layer != null; layer = layer.Next)
            {
                if (!layer.ValidateDuplicatedIdRecursive(id))
                {
                    throw new DuplicatedIdException(id);
                }
            }

            for (LayerBase layer = this.Previous; layer != null; layer = (layer as PerceptronLayer)?.Previous)
            {
                if (!layer.ValidateDuplicatedIdRecursive(id))
                {
                    throw new DuplicatedIdException(id);
                }
            }
        }
    }
}
