using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Exceptions
{
    public class DuplicatedIdException : IdExceptionBase
    {
        internal DuplicatedIdException(string id) : base(id, $"Id '{id}' already exists. The Ids cannot be duplicated")
        {
        }
    }
}
