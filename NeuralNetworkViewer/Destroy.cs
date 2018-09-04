using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer
{
    internal static class Destroy
    {
        internal static void Disposable<TDisposable>(ref TDisposable disposable) where TDisposable : class, IDisposable
        {
            if (disposable != null)
            {
                disposable.Dispose();
                disposable = null;
            }
        }
    }
}
