using NeuralNetworkVisualizer.Drawing.Controls;
using NeuralNetworkVisualizer.Model;
using NeuralNetworkVisualizer.Model.Layers;
using NeuralNetworkVisualizer.Model.Nodes;
using NeuralNetworkVisualizer.Preferences;
using NeuralNetworkVisualizer.Selection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNetworkVisualizer
{
    public partial class NeuralNetworkVisualizerControl : UserControl
    {
        private readonly IControlDrawing _controlDrawing;
        private readonly IElementSelector _selector;
        private readonly ISelectionEventFiring _selectionEventFiring;
        private readonly IToolTipFiring _toolTipFiring;

        public event EventHandler<SelectionEventArgs<InputLayer>> SelectInputLayer;
        public event EventHandler<SelectionEventArgs<PerceptronLayer>> SelectPerceptronLayer;
        public event EventHandler<SelectionEventArgs<Bias>> SelectBias;
        public event EventHandler<SelectionEventArgs<Input>> SelectInput;
        public event EventHandler<SelectionEventArgs<Perceptron>> SelectPerceptron;
        public event EventHandler<SelectionEventArgs<Edge>> SelectEdge;

        public NeuralNetworkVisualizerControl()
        {
            InitializeComponent();

            var selectableElementRegisterResolver = new SelectableElementRegister();
            _selector = new ElementSelector(selectableElementRegisterResolver);

            _controlDrawing = new ControlDrawing(new ControlCanvas(this.picCanvas, this), _selector, selectableElementRegisterResolver, selectableElementRegisterResolver);
            _toolTipFiring = new ToolTipFiring(this, picCanvas, selectableElementRegisterResolver);
            _selectionEventFiring = new SelectionEventFiring(this, _selector,
                                       () => this.SelectInputLayer,
                                       () => this.SelectPerceptronLayer,
                                       () => this.SelectBias,
                                       () => this.SelectInput,
                                       () => this.SelectPerceptron,
                                       () => this.SelectEdge);

            Control.CheckForIllegalCrossThreadCalls = true;
            this.BackColor = Color.White;

            picCanvas.MouseDown += PicCanvas_MouseDown;
            picCanvas.MouseMove += PicCanvas_MouseMove;
            picCanvas.MouseLeave += PicCanvas_MouseLeave;
        }

        private Preference _preferences = new Preference();
        [Browsable(false)]
        public Preference Preferences
        {
            get { return _preferences; }
        }

        private InputLayer _InputLayer = null;
        [Browsable(false)]
        public InputLayer InputLayer
        {
            get
            {
                return _InputLayer;
            }
            set
            {
                _InputLayer = value;
                _zoom = 1f; //restart zoom
                Redraw();
            }
        }

        public IEnumerable<Element> SelectedElements => _selector.SelectedElements;

        private bool _selectable = false;
        public bool Selectable
        {
            get => _selectable;
            set
            {
                _selectable = value;

                if (!_selectable)
                {
                    _selector.UnselectAll();
                    Redraw();
                }
            }
        }

        private float _zoom = 1;
        [Browsable(false)]
        public float Zoom
        {
            get => _zoom;
            set
            {
                if (_InputLayer == null)
                {
                    return; //nothing to do
                }

                _zoom = Constrain(0.1f, value, 10.0f); //limit the zoom value: Graphics will throw exception if not.
                Redraw();
            }
        }

        [Browsable(false)]
        public Image Image => _controlDrawing.GetImage();

        public void Redraw()
        {
            _controlDrawing?.Redraw();
        }

        public async Task RedrawAsync()
        {
            await _controlDrawing?.RedrawAsync();
        }

        private Size _previousSize = Size.Empty;
        protected override async void OnSizeChanged(EventArgs e)
        {
            _previousSize = this.ClientSize;

            if (!this.ClientSize.IsEmpty)
            {
                if (!_previousSize.IsEmpty)
                {
                    if (_preferences.AsyncRedrawOnResize)
                    {
                        await RedrawAsync();
                    }
                    else
                    {
                        Redraw();
                    }
                }
            }

            base.OnSizeChanged(e);
        }

        private void PicCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            _selectionEventFiring.FireSelectionEvent(e.Location);
        }

        private void PicCanvas_MouseLeave(object sender, EventArgs e)
        {
            _toolTipFiring.Hide();
        }

        private void PicCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            _toolTipFiring.Show(e.Location);
        }

        private T Constrain<T>(T low, T value, T max) where T : IComparable<T>
        {
            return (value.CompareTo(low) < 0 ? low : (value.CompareTo(max) > 0 ? max : value));
        }

        protected override void Dispose(bool disposing)
        {
            Destroy.Disposable(ref _preferences);

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
