# NeuralNetworkVisualizer
Easy neural network visualizer control for .Net

## screenshot: Regular
![Regular](https://github.com/sebastiantramontana/NeuralNetworkVisualizer/raw/master/docs/screen4.PNG)

**With layer titles**
![Regular](https://github.com/sebastiantramontana/NeuralNetworkVisualizer/raw/master/docs/screen1.PNG)

## Depending on objects size, labels can be hidden

**screenshot: Auto sized**
![Auto size](https://github.com/sebastiantramontana/NeuralNetworkVisualizer/raw/master/docs/screen2.PNG)

**screenshot: Auto sized**
![Auto size](https://github.com/sebastiantramontana/NeuralNetworkVisualizer/raw/master/docs/screen3.PNG)

## Example

```C#
            var _input = new InputLayer("Input")
            {
                Bias = new Bias("bias") { OutputValue = 1.234 }
            };

            _input.AddNode(new Input("e2") { OutputValue = 0.455 });
            _input.AddNode(new Input("e3") { OutputValue = 0.78967656 });
            _input.AddNode(new Input("e4") { OutputValue = 0.876545 });

            var hidden = new PerceptronLayer("Hidden");

            hidden.AddNode(new Perceptron("o1") { ActivationFunction = ActivationFunction.LeakyRelu, OutputValue = 2.364, SumValue = 2.364 });
            hidden.AddNode(new Perceptron("o2") { ActivationFunction = ActivationFunction.Tanh, OutputValue = 0.552, SumValue = 55.44 });
            hidden.AddNode(new Perceptron("o3") { ActivationFunction = ActivationFunction.Sigmoid, OutputValue = 0.876545, SumValue = 11.22 });

            _input.Connect(hidden);

            var output = new PerceptronLayer("Output");
            output.AddNode(new Perceptron("s1") { ActivationFunction = ActivationFunction.BinaryStep, OutputValue = 0.78967656, SumValue = 0.5544 });
            output.AddNode(new Perceptron("s2") { ActivationFunction = ActivationFunction.Softmax, OutputValue = 0.876545, SumValue = 0.5644 });

            hidden.Connect(output);

            var aleatorio = new Random(31);

            foreach (var p in hidden.Nodes)
            {
                foreach (var edge in p.Edges)
                {
                    edge.Weight = aleatorio.NextDouble();
                }
            }

            foreach (var p in output.Nodes)
            {
                foreach (var edge in p.Edges)
                {
                    edge.Weight = aleatorio.NextDouble();
                }
            }

            NeuralNetworkVisualizerControl1.InputLayer = _input;
