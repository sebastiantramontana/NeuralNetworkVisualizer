using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworkVisualizer.Model.Nodes
{
    public class Perceptron : NodeBase
    {
        public Perceptron(string id) : base(id)
        {
            this.EdgesInternal = new List<Edge>();
        }

        public double? SumValue { get; set; }
        public ActivationFunction ActivationFunction { get; set; }
        public IEnumerable<Edge> Edges { get => this.EdgesInternal; } //get read-only access to externals
        internal ICollection<Edge> EdgesInternal { get; private set; } //for internal use. Hide to externals.

        private protected override void ValidateDuplicatedIChild(IDictionary<string, Element> acumulatedIds)
        {
            foreach (var edge in this.Edges)
                edge.ValidateId(acumulatedIds);
        }
    }
}
