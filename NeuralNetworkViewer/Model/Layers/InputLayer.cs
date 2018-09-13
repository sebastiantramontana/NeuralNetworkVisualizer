using NeuralNetworkVisualizer.Exceptions;
using NeuralNetworkVisualizer.Model.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworkVisualizer.Model.Layers
{
    public class InputLayer : LayerBase<Input>
    {
        public InputLayer(string id) : base(id) { }

        public Element Find(string id)
        {
            Element elem = null;

            for (LayerBase layer = this; layer != null; layer = layer.Next)
            {
                if (layer.Id == id)
                    return layer;

                elem = layer.FindByIdRecursive(id);

                if (elem != null)
                    break;
            }

            return elem;
        }

        public TElement Find<TElement>(string id) where TElement : Element
        {
            return Find(id) as TElement;
        }

        public int GetMaxNodeCountInLayer()
        {
            int max = 0;

            for (LayerBase layer = this; layer != null; layer = layer.Next)
            {
                var count = layer.GetAllNodes().Count();

                if (count > max)
                    max = count;
            }

            return max;
        }

        public int LayersCount()
        {
            int count = 0;

            for (LayerBase layer = this; layer != null; layer = layer.Next, ++count) ; //nice! :)

            return count;
        }

        private protected override void ValidateId(string id)
        {
            for (LayerBase layer = this; layer != null; layer = layer.Next)
            {
                if (!layer.ValidateDuplicatedIdRecursive(id))
                {
                    throw new DuplicatedIdException(id);
                }
            }
        }
    }
}
