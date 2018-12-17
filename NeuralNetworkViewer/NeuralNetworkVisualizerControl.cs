using NeuralNetworkVisualizer.Drawing.Controls;
using NeuralNetworkVisualizer.Model.Layers;
using NeuralNetworkVisualizer.Preferences;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNetworkVisualizer
{
    public partial class NeuralNetworkVisualizerControl : UserControl
    {
        private readonly IControlDrawing _controlDrawing;

        public NeuralNetworkVisualizerControl()
        {
            InitializeComponent();

            _controlDrawing = new ControlDrawing(new ControlCanvas(this.picCanvas, this));

            Control.CheckForIllegalCrossThreadCalls = true;
            this.BackColor = Color.White;
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
