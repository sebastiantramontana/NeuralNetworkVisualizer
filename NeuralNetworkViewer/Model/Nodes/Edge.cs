using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Model.Nodes
{
    public class Edge : Element
    {
        private Edge(string id) : base(id)
        {
        }

        public NodeBase Source { get; private set; }
        public Perceptron Destination { get; private set; }
        public double? Weight { get; set; }

        internal static Edge Create(NodeBase source, Perceptron destination)
        {
            if (source == null || destination == null)
                throw new ArgumentNullException("source and/or destination arguments are null");

            var id = source.Layer.Id + "." + source.Id + " - " + destination.Layer.Id + "." + destination.Id;
            return new Edge(id) { Source = source, Destination = destination };
        }

        internal protected override Element FindByIdRecursive(string id)
        {
            if (this.Source?.Id == id)
                return this.Source;

            if (this.Destination?.Id == id)
                return this.Destination;

            return null;
        }

        protected internal override bool ValidateDuplicatedIdRecursive(string id)
        {
            return (this.Id != id);
        }
    }
}
