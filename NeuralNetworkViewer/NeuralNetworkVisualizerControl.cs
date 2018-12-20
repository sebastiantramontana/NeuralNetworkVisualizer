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
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNetworkVisualizer
{
    public partial class NeuralNetworkVisualizerControl : UserControl
    {
        private readonly IControlDrawing _controlDrawing;
        private readonly IElementSelector _selector;

        public event EventHandler<SelectionEventArgs<InputLayer>> SelectInputLayer;
        public event EventHandler<SelectionEventArgs<PerceptronLayer>> SelectPerceptronLayer;
        public event EventHandler<SelectionEventArgs<Bias>> SelectBias;
        public event EventHandler<SelectionEventArgs<Input>> SelectInput;
        public event EventHandler<SelectionEventArgs<Perceptron>> SelectPerceptron;
        public event EventHandler<SelectionEventArgs<Edge>> SelectEdge;

        public NeuralNetworkVisualizerControl()
        {
            InitializeComponent();

            var selectionResolver = new SelectableElementRegister();
            _selector = new ElementSelector(selectionResolver);

            _controlDrawing = new ControlDrawing(new ControlCanvas(this.picCanvas, this), _selector, selectionResolver, selectionResolver);

            Control.CheckForIllegalCrossThreadCalls = true;
            this.BackColor = Color.White;

            picCanvas.MouseDown += FireSelectionEvent;
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

        public IEnumerable<Element> SelectedElements => _selector.SelectedElements.ToArray(); //ToArray() to avoid hacking

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

        private void FireSelectionEvent(object sender, MouseEventArgs e)
        {
            if (!_preferences.Selectable)
                return;

            Func<Point, Element> selectFunc;
            bool isSelected;

            switch (Control.ModifierKeys)
            {
                case Keys.Control:
                    selectFunc = _selector.Unselect;
                    isSelected = false;
                    break;
                case Keys.Shift:
                    selectFunc = _selector.AddToSelection;
                    isSelected = true;
                    break;
                default:
                    selectFunc = _selector.SelectOnly;
                    isSelected = true;
                    break;
            }

            var element = selectFunc(e.Location);

            if (element == null)
            {
                return;
            }

            FireSelectionEvent(element, isSelected);
            Redraw();

            base.OnMouseDown(e);
        }

        private void FireSelectionEvent(Element element, bool isSelected)
        {
            if (FireSelectionEvent(element, isSelected, SelectInputLayer))
                return;

            if (FireSelectionEvent(element, isSelected, SelectPerceptronLayer))
                return;

            if (FireSelectionEvent(element, isSelected, SelectBias))
                return;

            if (FireSelectionEvent(element, isSelected, SelectInput))
                return;

            if (FireSelectionEvent(element, isSelected, SelectPerceptron))
                return;

            if (FireSelectionEvent(element, isSelected, SelectEdge))
                return;
        }

        private bool FireSelectionEvent<TElement>(Element element, bool isSelected, EventHandler<SelectionEventArgs<TElement>> eventHandler) where TElement : Element
        {
            var fired = false;

            if (element is TElement typedElement)
            {
                eventHandler?.Invoke(this, new SelectionEventArgs<TElement>(typedElement, isSelected));
                fired = true;
            }

            return fired;
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
