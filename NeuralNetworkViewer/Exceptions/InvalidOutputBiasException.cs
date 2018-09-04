using NeuralNetworkVisualizer.Model.Layers;
using System;

namespace NeuralNetworkVisualizer.Exceptions
{
    public class InvalidOutputBiasException : Exception
    {
        internal InvalidOutputBiasException(LayerBase outputLayer) : base("Output layer cannot contain bias node")
        {
            this.OutputLayer = outputLayer;
        }

        public LayerBase OutputLayer { get; private set; }
    }
}
