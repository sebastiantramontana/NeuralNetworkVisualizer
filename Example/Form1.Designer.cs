namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.btnAddBias = new System.Windows.Forms.Button();
            this.trackZoom = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.cboQuality = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.NeuralNetworkVisualizerControl1 = new NeuralNetworkVisualizer.NeuralNetworkVisualizerControl();
            this.btnChangeValue = new System.Windows.Forms.Button();
            this.chSelectable = new System.Windows.Forms.CheckBox();
            this.txtSelectedElements = new System.Windows.Forms.TextBox();
            this.txtLastSelected = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStart.Location = new System.Drawing.Point(12, 708);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnAddBias
            // 
            this.btnAddBias.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddBias.Enabled = false;
            this.btnAddBias.Location = new System.Drawing.Point(174, 709);
            this.btnAddBias.Name = "btnAddBias";
            this.btnAddBias.Size = new System.Drawing.Size(75, 23);
            this.btnAddBias.TabIndex = 2;
            this.btnAddBias.Text = "Add Bias";
            this.btnAddBias.UseVisualStyleBackColor = true;
            this.btnAddBias.Click += new System.EventHandler(this.btnAddBias_Click);
            // 
            // trackZoom
            // 
            this.trackZoom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trackZoom.AutoSize = false;
            this.trackZoom.Enabled = false;
            this.trackZoom.Location = new System.Drawing.Point(336, 708);
            this.trackZoom.Maximum = 100;
            this.trackZoom.Minimum = 10;
            this.trackZoom.Name = "trackZoom";
            this.trackZoom.Size = new System.Drawing.Size(229, 23);
            this.trackZoom.TabIndex = 5;
            this.trackZoom.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackZoom.Value = 10;
            this.trackZoom.Scroll += new System.EventHandler(this.trackZoom_Scroll);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(419, 719);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Zoom";
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.Enabled = false;
            this.btnClear.Location = new System.Drawing.Point(255, 708);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // cboQuality
            // 
            this.cboQuality.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboQuality.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cboQuality.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboQuality.Enabled = false;
            this.cboQuality.FormattingEnabled = true;
            this.cboQuality.Location = new System.Drawing.Point(618, 711);
            this.cboQuality.Name = "cboQuality";
            this.cboQuality.Size = new System.Drawing.Size(121, 21);
            this.cboQuality.TabIndex = 8;
            this.cboQuality.SelectedIndexChanged += new System.EventHandler(this.cboQuality_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(573, 713);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Quality";
            // 
            // NeuralNetworkVisualizerControl1
            // 
            this.NeuralNetworkVisualizerControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NeuralNetworkVisualizerControl1.AutoScroll = true;
            this.NeuralNetworkVisualizerControl1.BackColor = System.Drawing.Color.White;
            this.NeuralNetworkVisualizerControl1.InputLayer = null;
            this.NeuralNetworkVisualizerControl1.Location = new System.Drawing.Point(12, 12);
            this.NeuralNetworkVisualizerControl1.Margin = new System.Windows.Forms.Padding(0);
            this.NeuralNetworkVisualizerControl1.Name = "NeuralNetworkVisualizerControl1";
            this.NeuralNetworkVisualizerControl1.Size = new System.Drawing.Size(1257, 690);
            this.NeuralNetworkVisualizerControl1.TabIndex = 10;
            this.NeuralNetworkVisualizerControl1.Zoom = 1F;
            // 
            // btnChangeValue
            // 
            this.btnChangeValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnChangeValue.Enabled = false;
            this.btnChangeValue.Location = new System.Drawing.Point(93, 708);
            this.btnChangeValue.Name = "btnChangeValue";
            this.btnChangeValue.Size = new System.Drawing.Size(75, 23);
            this.btnChangeValue.TabIndex = 11;
            this.btnChangeValue.Text = "Ch. Value";
            this.btnChangeValue.UseVisualStyleBackColor = true;
            this.btnChangeValue.Click += new System.EventHandler(this.btnChangeValue_Click);
            // 
            // chSelectable
            // 
            this.chSelectable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chSelectable.AutoSize = true;
            this.chSelectable.Location = new System.Drawing.Point(745, 714);
            this.chSelectable.Name = "chSelectable";
            this.chSelectable.Size = new System.Drawing.Size(98, 17);
            this.chSelectable.TabIndex = 12;
            this.chSelectable.Text = "Allow Selection";
            this.chSelectable.UseVisualStyleBackColor = true;
            this.chSelectable.CheckedChanged += new System.EventHandler(this.chSelectable_CheckedChanged);
            // 
            // txtSelectedElements
            // 
            this.txtSelectedElements.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSelectedElements.Location = new System.Drawing.Point(849, 712);
            this.txtSelectedElements.Name = "txtSelectedElements";
            this.txtSelectedElements.ReadOnly = true;
            this.txtSelectedElements.Size = new System.Drawing.Size(277, 20);
            this.txtSelectedElements.TabIndex = 13;
            // 
            // txtLastSelected
            // 
            this.txtLastSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLastSelected.Location = new System.Drawing.Point(1132, 713);
            this.txtLastSelected.Name = "txtLastSelected";
            this.txtLastSelected.ReadOnly = true;
            this.txtLastSelected.Size = new System.Drawing.Size(137, 20);
            this.txtLastSelected.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1281, 741);
            this.Controls.Add(this.txtLastSelected);
            this.Controls.Add(this.txtSelectedElements);
            this.Controls.Add(this.chSelectable);
            this.Controls.Add(this.btnChangeValue);
            this.Controls.Add(this.NeuralNetworkVisualizerControl1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboQuality);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackZoom);
            this.Controls.Add(this.btnAddBias);
            this.Controls.Add(this.btnStart);
            this.Name = "Form1";
            this.Text = "Example";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackZoom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnAddBias;
        private System.Windows.Forms.TrackBar trackZoom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ComboBox cboQuality;
        private System.Windows.Forms.Label label2;
        private NeuralNetworkVisualizer.NeuralNetworkVisualizerControl NeuralNetworkVisualizerControl1;
        private System.Windows.Forms.Button btnChangeValue;
        private System.Windows.Forms.CheckBox chSelectable;
        private System.Windows.Forms.TextBox txtSelectedElements;
        private System.Windows.Forms.TextBox txtLastSelected;
    }
}

