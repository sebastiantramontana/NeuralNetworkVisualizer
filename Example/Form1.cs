using NeuralNetworkVisualizer.Model;
using NeuralNetworkVisualizer.Model.Layers;
using NeuralNetworkVisualizer.Model.Nodes;
using NeuralNetworkVisualizer.Preferences.Brushes;
using NeuralNetworkVisualizer.Preferences.Formatting;
using NeuralNetworkVisualizer.Preferences.Text;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private InputLayer _input;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NeuralNetworkVisualizerControl1.Preferences.Quality = RenderQuality.High;

            cboQuality.Items.Add(RenderQuality.Low);
            cboQuality.Items.Add(RenderQuality.Medium);
            cboQuality.Items.Add(RenderQuality.High);

            cboQuality.SelectedItem = NeuralNetworkVisualizerControl1.Preferences.Quality;

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

            //To remove layer's titles
            //NeuralNetworkVisualizerControl1.Preferences.Layers = null;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _input = new InputLayer("Input")
            {
                Bias = new Bias("bias") { OutputValue = 1.234 }
            };

            _input.AddNode(new Input("e1") { OutputValue = 0.255 });
            _input.AddNode(new Input("e2") { OutputValue = 0.455 });
            _input.AddNode(new Input("e3") { OutputValue = -0.78967656 });
            _input.AddNode(new Input("e4") { OutputValue = 0.0 });
            //_input.AddNode(new Input("e5") { OutputValue = 0.255 });
            //_input.AddNode(new Input("e6") { OutputValue = 0.455 });
            //_input.AddNode(new Input("e7") { OutputValue = -0.78967656 });
            //_input.AddNode(new Input("e8") { OutputValue = 0.011 });
            //_input.AddNode(new Input("e9") { OutputValue = 0.2255 });
            //_input.AddNode(new Input("e10") { OutputValue = 43.455 });
            //_input.AddNode(new Input("e11") { OutputValue = -11.67656 });
            //_input.AddNode(new Input("e12") { OutputValue = -1.001 });

            var hidden = new PerceptronLayer("Hidden");

            hidden.AddNode(new Perceptron("o1") { ActivationFunction = ActivationFunction.BinaryStep, OutputValue = 2.364, SumValue = 2.364 });
            hidden.AddNode(new Perceptron("o2") { ActivationFunction = ActivationFunction.LeakyRelu, OutputValue = -0.552, SumValue = 55.44 });
            hidden.AddNode(new Perceptron("o4") { ActivationFunction = ActivationFunction.Relu, OutputValue = 1.324, SumValue = 4.34 });
            //hidden.AddNode(new Perceptron("o3") { ActivationFunction = ActivationFunction.Linear, OutputValue = 0.0, SumValue = 19.22 });
            //hidden.AddNode(new Perceptron("o5") { ActivationFunction = ActivationFunction.Sigmoid, OutputValue = -0.12, SumValue = 25.224 });
            //hidden.AddNode(new Perceptron("o6") { ActivationFunction = ActivationFunction.Tanh, OutputValue = 10.3, SumValue = 1.222 });

            _input.Connect(hidden);

            var output = new PerceptronLayer("Output");
            output.AddNode(new Perceptron("s1") { ActivationFunction = ActivationFunction.Softmax, OutputValue = 0.567656, SumValue = 0.454 });
            output.AddNode(new Perceptron("s2") { ActivationFunction = ActivationFunction.Sigmoid, OutputValue = 0.176545, SumValue = 0.54 });
            //output.AddNode(new Perceptron("s3") { ActivationFunction = ActivationFunction.Softmax, OutputValue = 0.9545, SumValue = 0.133 });
            //output.AddNode(new Perceptron("s4") { ActivationFunction = ActivationFunction.Softmax, OutputValue = 0.145, SumValue = 0.88 });

            hidden.Connect(output);

            var aleatorio = new Random(2);

            foreach (var p in hidden.Nodes)
            {
                foreach (var edge in p.Edges)
                {
                    int sign = aleatorio.Next(-1, 2);
                    edge.Weight = aleatorio.NextDouble() * sign;
                }
            }

            foreach (var p in output.Nodes)
            {
                foreach (var edge in p.Edges)
                {
                    int sign = aleatorio.Next(-1, 1);
                    edge.Weight = aleatorio.NextDouble() * sign;
                }
            }

            NeuralNetworkVisualizerControl1.InputLayer = _input;

            btnChangeValue.Enabled = btnAddBias.Enabled = btnClear.Enabled = trackZoom.Enabled = cboQuality.Enabled = true;
        }

        private void btnChangeValue_Click(object sender, EventArgs e)
        {
            var node = _input.Nodes.Single(n => n.Id == "e3");
            node.OutputValue = 1.44444;
            NeuralNetworkVisualizerControl1.Redraw();
        }

        private void btnAddBias_Click(object sender, EventArgs e)
        {
            AddHiddenBias();
            NeuralNetworkVisualizerControl1.Redraw();
        }

        private void AddHiddenBias()
        {
            var newbias = new Bias("HiddenBias") { OutputValue = 0.777 };
            _input.Next.Bias = newbias;

            var outputs = _input.Next.Next.Nodes;
            var edges = outputs.SelectMany(o => o.Edges.Where(e => e.Source == newbias));

            double weight = 1.99;
            foreach (var edge in edges)
            {
                edge.Weight = weight;
                weight++;
            }
        }

        private void trackZoom_Scroll(object sender, EventArgs e)
        {
            NeuralNetworkVisualizerControl1.Zoom = trackZoom.Value / 10f;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            NeuralNetworkVisualizerControl1.InputLayer = null;
            btnChangeValue.Enabled = btnAddBias.Enabled = btnClear.Enabled = trackZoom.Enabled = cboQuality.Enabled = false;
        }

        private void cboQuality_SelectedIndexChanged(object sender, EventArgs e)
        {
            NeuralNetworkVisualizerControl1.Preferences.Quality = (RenderQuality)cboQuality.SelectedItem;
            NeuralNetworkVisualizerControl1.Redraw();
        }
    }
}
