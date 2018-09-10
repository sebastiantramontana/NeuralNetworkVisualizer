using NeuralNetworkVisualizer.Model;
using NeuralNetworkVisualizer.Model.Layers;
using NeuralNetworkVisualizer.Model.Nodes;
using NeuralNetworkVisualizer.Preferences.Brushes;
using NeuralNetworkVisualizer.Preferences.Formatting;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
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
            cboQuality.Items.Add(RenderQuality.Low);
            cboQuality.Items.Add(RenderQuality.Medium);
            cboQuality.Items.Add(RenderQuality.High);

            cboQuality.SelectedItem = NeuralNetworkVisualizerControl1.Preferences.Quality;

            NeuralNetworkVisualizerControl1.Preferences.Inputs.OutputValueFormatter = new Formatter<TextPreference>(
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Red) },
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Gray) },
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Black) }
            );

            NeuralNetworkVisualizerControl1.Preferences.Perceptrons.OutputValueFormatter = new Formatter<TextPreference>(
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Red) },
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Gray) },
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Black) }
            );

            NeuralNetworkVisualizerControl1.Preferences.Edges.ValueFormatter = new Formatter<TextPreference>(
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Red) },
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Gray) },
                () => new TextPreference { Brush = new SolidBrushPreference(Color.Black) }
            );

            NeuralNetworkVisualizerControl1.Preferences.Edges.Connector = new Formatter<Pen>((v) => v == 0.0 ? new Pen(Color.LightGray) : new Pen(Color.Black));
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _input = new InputLayer("Input")
            {
                Bias = new Bias("bias") { OutputValue = 1.234 }
            };

            _input.AddNode(new Input("e2") { OutputValue = 0.455 });
            _input.AddNode(new Input("e3") { OutputValue = -0.78967656 });
            _input.AddNode(new Input("e4") { OutputValue = 0.0 });

            var hidden = new PerceptronLayer("Hidden");

            hidden.AddNode(new Perceptron("o1") { ActivationFunction = ActivationFunction.LeakyRelu, OutputValue = 2.364, SumValue = 2.364 });
            hidden.AddNode(new Perceptron("o2") { ActivationFunction = ActivationFunction.Tanh, OutputValue = -0.552, SumValue = 55.44 });
            hidden.AddNode(new Perceptron("o3") { ActivationFunction = ActivationFunction.Sigmoid, OutputValue = 0.0, SumValue = 11.22 });

            _input.Connect(hidden);

            var output = new PerceptronLayer("Output");
            output.AddNode(new Perceptron("s1") { ActivationFunction = ActivationFunction.BinaryStep, OutputValue = 0.78967656, SumValue = 0.5544 });
            output.AddNode(new Perceptron("s2") { ActivationFunction = ActivationFunction.Softmax, OutputValue = 0.876545, SumValue = 0.5644 });

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
