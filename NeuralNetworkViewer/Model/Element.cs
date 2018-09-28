using NeuralNetworkVisualizer.Exceptions;
using System;
using System.Collections.Generic;

namespace NeuralNetworkVisualizer.Model
{
    public abstract class Element : IEquatable<Element>
    {
        internal Element(string id)
        {
            this.Id = id;
            this.ValidateId();
        }

        public string Id { get; set; }
        public object Object { get; set; }

        internal void ValidateId(IDictionary<string, Element> acumulatedIds)
        {
            ValidateId();

            if (acumulatedIds == null)
                throw new ArgumentNullException($"Internal Error: {nameof(acumulatedIds)} is null.");

            if (acumulatedIds.ContainsKey(this.Id))
                throw new DuplicatedIdException(this.Id);

            acumulatedIds.Add(this.Id, this);

            ValidateDuplicatedIChild(acumulatedIds);
        }

        private void ValidateId()
        {
            if (string.IsNullOrWhiteSpace(this.Id))
                throw new InvalidIdException(this.Id);
        }

        private protected virtual void ValidateDuplicatedIChild(IDictionary<string, Element> acumulatedIds)
        {

        }

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
