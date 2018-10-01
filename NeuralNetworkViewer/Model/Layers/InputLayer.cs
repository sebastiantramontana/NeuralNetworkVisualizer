using NeuralNetworkVisualizer.Exceptions;
using NeuralNetworkVisualizer.Model.Nodes;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworkVisualizer.Model.Layers
{
    public class InputLayer : LayerBase<Input>
    {
        private IDictionary<string, Element> _allElements;

        public InputLayer(string id) : base(id)
        {
            
        }

        /// <summary>
        /// Find an element by id throughout the entire model
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The found element or null if it is not exist</returns>
        public Element Find(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            _allElements.TryGetValue(id, out Element element);
            return element;
        }

        /// <summary>
        /// Find an typed element by id throughout the entire model
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The found element or null if itsn't exist or type doesn't match</returns>
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

        public int CountLayers()
        {
            int count = 0;

            for (LayerBase layer = this; layer != null; layer = layer.Next, ++count) ; //nice! :)

            return count;
        }

        public LayerBase SearchOutputLayer()
        {
            LayerBase outputlayer;
            for (outputlayer = this; outputlayer.Next != null; outputlayer = outputlayer.Next) ;

            return outputlayer;
        }

        internal void Validate()
        {
            LayerBase outputlayer = SearchOutputLayer();

            if (outputlayer == this)
            {
                throw new MissingOutputException(this);
            }

            ///Bias doesn´t make sense to be in the output layer
            ///TODO: Fix Model, make an OutputLayer
            if (outputlayer.Bias != null)
            {
                throw new InvalidOutputBiasException(outputlayer);
            }

            //Don't reuse _allElements! 
            //if filled, it will throw a DuplicatedIdException for each call
            var acumulatedIds = new Dictionary<string, Element>();
            ValidateId(acumulatedIds);

            _allElements = acumulatedIds; //model could have been changed
        }
    }
}
