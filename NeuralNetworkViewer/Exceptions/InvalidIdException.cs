using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Exceptions
{
    public class InvalidIdException : IdExceptionBase
    {
        internal InvalidIdException(string id) : base(id, "Id canot be null or empty")
        {

        }
    }
}
