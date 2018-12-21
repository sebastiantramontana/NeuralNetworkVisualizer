# NeuralNetworkVisualizer
Easy neural network visualizer winform control for .Net

## Screenshots
![Normal](https://github.com/sebastiantramontana/NeuralNetworkVisualizer/raw/master/docs/Normal.PNG)
![Little Size](https://github.com/sebastiantramontana/NeuralNetworkVisualizer/raw/master/docs/NormalLittle.PNG)
![Layers Titles](https://github.com/sebastiantramontana/NeuralNetworkVisualizer/raw/master/docs/NormalWithTitles.PNG)
![Several Nodes](https://github.com/sebastiantramontana/NeuralNetworkVisualizer/raw/master/docs/SeveralNodes.PNG)
![Zoomed](https://github.com/sebastiantramontana/NeuralNetworkVisualizer/raw/master/docs/SeveralNodesZoomed.png)
![Elements Selection](https://github.com/sebastiantramontana/NeuralNetworkVisualizer/raw/master/docs/NormalSelectedElements.png)

## Installing
Install NeuralNetworkVisualizer from [Nuget](https://www.nuget.org/packages/NeuralNetworkVisualizer).

## Example

```C#
            /******** Configure Some Preferences: ********/
            
            //Drawing resize behavior
            NeuralNetworkVisualizerControl1.Preferences.AsyncRedrawOnResize = false; //default is true
            
            //Font, Colors, etc.
            NeuralNetworkVisualizerControl1.Preferences.Inputs.OutputValueFormatter = new ByValueSignFormatter<TextPreference>(
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Red) },
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Gray) },
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Black) },
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Black) }
            );

            NeuralNetworkVisualizerControl1.Preferences.Perceptrons.OutputValueFormatter = new ByValueSignFormatter<TextPreference>(
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Red) },
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Gray) },
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Black) },
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Black) }
            );

            NeuralNetworkVisualizerControl1.Preferences.Edges.ValueFormatter = new ByValueSignFormatter<TextPreference>(
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Red) },
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Gray) },
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Black) },
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Black) }
            );

            NeuralNetworkVisualizerControl1.Preferences.Edges.Connector = new CustomFormatter<Pen>((v) => v == 0.0 ? new Pen(Color.LightGray) : new Pen(Color.Black));

            //Graphics quality
            NeuralNetworkVisualizerControl1.Preferences.Quality = RenderQuality.High; //Low, Medium, High. Medium is default

            //To remove layer titles
            //NeuralNetworkVisualizerControl1.Preferences.Layers = null;

            //** NOTE: ** Preferences setting don't redraw the control automatically. If you need to redraw the current rendered NN, call to Redraw() method after all setting 
            //NeuralNetworkVisualizerControl1.Redraw();


            
            /***** Some Functionalities *****/

            //Adjust zoom
            NeuralNetworkVisualizerControl1.Zoom = 2.0f; //1.0 is 'normal' and default, fit the whole drawing to control size

            //Get the current rendered NN to save to disk or whatever
            Image img = NeuralNetworkVisualizerControl1.Image;



            /*************** Set the NN Model *****************/

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

            _input.Connect(hidden); //Connect(...) method creates nodes connections

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

            NeuralNetworkVisualizerControl1.InputLayer = _input; //Automatic rendering
            //NeuralNetworkVisualizerControl1.InputLayer = null; //Leave blank when needed
            
            
            
            
            
            /*************** Make NN Elements Selectable *****************/
            //Drawing resize behavior: The selectable elements are: Layers, Nodes (all types) and Edge connectors
            
                        
            NeuralNetworkVisualizerControl1.Selectable = true; //default is false
            
            //Each selectable element has its own typed-safe "Select" event
            NeuralNetworkVisualizerControl1.SelectBias += NeuralNetworkVisualizerControl1_SelectBias;
            NeuralNetworkVisualizerControl1.SelectEdge += NeuralNetworkVisualizerControl1_SelectEdge;
            NeuralNetworkVisualizerControl1.SelectInput += NeuralNetworkVisualizerControl1_SelectInput;
            NeuralNetworkVisualizerControl1.SelectInputLayer += NeuralNetworkVisualizerControl1_SelectInputLayer;
            NeuralNetworkVisualizerControl1.SelectPerceptron += NeuralNetworkVisualizerControl1_SelectPerceptron;
            NeuralNetworkVisualizerControl1.SelectPerceptronLayer += NeuralNetworkVisualizerControl1_SelectPerceptronLayer;
            
            private void NeuralNetworkVisualizerControl1_SelectPerceptronLayer(object sender, SelectionEventArgs<PerceptronLayer> e)
            {
                //...
            }

            private void NeuralNetworkVisualizerControl1_SelectPerceptron(object sender, SelectionEventArgs<Perceptron> e)
            {
                //...
            }

            private void NeuralNetworkVisualizerControl1_SelectInputLayer(object sender, SelectionEventArgs<InputLayer> e)
            {
                //...
            }

            private void NeuralNetworkVisualizerControl1_SelectInput(object sender, SelectionEventArgs<Input> e)
            {
                //...
            }

            private void NeuralNetworkVisualizerControl1_SelectEdge(object sender, SelectionEventArgs<Edge> e)
            {
                //...
            }

            private void NeuralNetworkVisualizerControl1_SelectBias(object sender, SelectionEventArgs<Bias> e)
            {
                //...
            }
            
            
