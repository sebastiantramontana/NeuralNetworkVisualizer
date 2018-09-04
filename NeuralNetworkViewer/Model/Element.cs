using NeuralNetworkVisualizer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Model
{
    public abstract class Element : IEquatable<Element>
    {
        internal Element(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new InvalidIdException(id);

            this.Id = id;
        }
        public string Id { get; private set; }
        public object Object { get; set; }

        internal protected abstract Element FindByIdRecursive(string id);
        internal protected abstract bool ValidateDuplicatedIdRecursive(string id);

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Element);
        }
        public bool Equals(Element other)
        {
            return !(other is null) && (Object.ReferenceEquals(this, other) || this.Id == other.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override string ToString()
        {
            return this.Id;
        }
        public static bool operator ==(Element b1, Element b2) => Object.Equals(b1, b2);
        public static bool operator !=(Element b1, Element b2) => !Object.Equals(b1, b2);

        
    }
}
