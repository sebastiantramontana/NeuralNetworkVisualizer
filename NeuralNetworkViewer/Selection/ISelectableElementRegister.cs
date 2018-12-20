using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Selection
{
    internal interface ISelectableElementRegister
    {
        void Register(RegistrationInfo info);
    }
}
