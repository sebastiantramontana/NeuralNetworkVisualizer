using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworkVisualizer.Model.Nodes
{
    public class Perceptron : NodeBase
    {
        public Perceptron(string id) : base(id)
        {
            EdgesInternal = new List<Edge>();
        }

        public double? SumValue { get; set; }
        public ActivationFunction ActivationFunction { get; set; }
        public IEnumerable<Edge> Edges { get => this.EdgesInternal; } //get read-only access to externals
        internal ICollection<Edge> EdgesInternal { get; private set; } //for internal use. Hide to externals.

        internal protected override Element FindByIdRecursive(string id)
        {
            var edges = this.Edges;
            Element elem = edges.SingleOrDefault(e => e.Id == id);

            if (elem != null)
                return elem;

            foreach (var edge in edges)
            {
                elem = edge.FindByIdRecursive(id);

                if (elem != null)
                    break;
            }

            return elem;
        }

        protected internal override bool ValidateDuplicatedIdRecursive(string id)
        {
            return (this.Id != id && this.Edges.All(e => e.ValidateDuplicatedIdRecursive(id)));
        }
    }
}
