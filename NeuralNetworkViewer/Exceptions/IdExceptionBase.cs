using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Exceptions
{
    public abstract class IdExceptionBase : Exception
    {
        internal IdExceptionBase(string id, string message) : base(message)
        {
            this.Id = id;
        }

        public string Id { get; private set; }
    }
}
