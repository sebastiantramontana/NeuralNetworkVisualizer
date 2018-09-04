using NeuralNetworkVisualizer.Model.Layers;
using System;

namespace NeuralNetworkVisualizer.Exceptions
{
    public class MissingOutputException : Exception
    {
        internal MissingOutputException(InputLayer inputLayer) : base($"Missing output layer for input layer: {inputLayer.Id}")
        {
            this.InputLayer = inputLayer;
        }

        public InputLayer InputLayer { get; private set; }
    }
}
