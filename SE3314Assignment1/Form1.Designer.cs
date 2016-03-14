namespace SE3314Assignment1
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
            this.portLabel = new System.Windows.Forms.Label();
            this.ipInput = new System.Windows.Forms.TextBox();
            this.portInput = new System.Windows.Forms.NumericUpDown();
            this.listenButton = new System.Windows.Forms.Button();
            this.printHeaderCheckBox = new System.Windows.Forms.CheckBox();
            this.ipLabel = new System.Windows.Forms.Label();
            this.requestLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.statusBox = new System.Windows.Forms.ListBox();
            this.requestTextBox = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.portInput)).BeginInit();
            this.SuspendLayout();
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(45, 30);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(72, 13);
            this.portLabel.TabIndex = 0;
            this.portLabel.Text = "Listen on Port";
            // 
            // ipInput
            // 
            this.ipInput.Location = new System.Drawing.Point(143, 69);
            this.ipInput.Name = "ipInput";
            this.ipInput.Size = new System.Drawing.Size(100, 20);
            this.ipInput.TabIndex = 1;
            this.ipInput.TextChanged += new System.EventHandler(this.ipInput_TextChanged);
            // 
            // portInput
            // 
            this.portInput.Location = new System.Drawing.Point(123, 30);
            this.portInput.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.portInput.Name = "portInput";
            this.portInput.Size = new System.Drawing.Size(120, 20);
            this.portInput.TabIndex = 2;
            this.portInput.ValueChanged += new System.EventHandler(this.portInput_ValueChanged);
            // 
            // listenButton
            // 
            this.listenButton.Location = new System.Drawing.Point(266, 30);
            this.listenButton.Name = "listenButton";
            this.listenButton.Size = new System.Drawing.Size(75, 23);
            this.listenButton.TabIndex = 3;
            this.listenButton.Text = "Listen";
            this.listenButton.UseVisualStyleBackColor = true;
            this.listenButton.Click += new System.EventHandler(this.listenButton_Click);
            // 
            // printHeaderCheckBox
            // 
            this.printHeaderCheckBox.AutoSize = true;
            this.printHeaderCheckBox.Location = new System.Drawing.Point(384, 31);
            this.printHeaderCheckBox.Name = "printHeaderCheckBox";
            this.printHeaderCheckBox.Size = new System.Drawing.Size(110, 17);
            this.printHeaderCheckBox.TabIndex = 4;
            this.printHeaderCheckBox.Text = "Print RTP Header";
            this.printHeaderCheckBox.UseVisualStyleBackColor = true;
            // 
            // ipLabel
            // 
            this.ipLabel.AutoSize = true;
            this.ipLabel.Location = new System.Drawing.Point(45, 69);
            this.ipLabel.Name = "ipLabel";
            this.ipLabel.Size = new System.Drawing.Size(92, 13);
            this.ipLabel.TabIndex = 5;
            this.ipLabel.Text = "Server IP Address";
            // 
            // requestLabel
            // 
            this.requestLabel.AutoSize = true;
            this.requestLabel.Location = new System.Drawing.Point(56, 300);
            this.requestLabel.Name = "requestLabel";
            this.requestLabel.Size = new System.Drawing.Size(81, 13);
            this.requestLabel.TabIndex = 8;
            this.requestLabel.Text = "Client Requests";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(59, 101);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(71, 13);
            this.statusLabel.TabIndex = 11;
            this.statusLabel.Text = "Server Status";
            // 
            // statusBox
            // 
            this.statusBox.FormattingEnabled = true;
            this.statusBox.Location = new System.Drawing.Point(48, 117);
            this.statusBox.Name = "statusBox";
            this.statusBox.Size = new System.Drawing.Size(426, 173);
            this.statusBox.TabIndex = 12;
            // 
            // requestTextBox
            // 
            this.requestTextBox.FormattingEnabled = true;
            this.requestTextBox.Location = new System.Drawing.Point(48, 316);
            this.requestTextBox.Name = "requestTextBox";
            this.requestTextBox.Size = new System.Drawing.Size(426, 147);
            this.requestTextBox.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 487);
            this.Controls.Add(this.requestTextBox);
            this.Controls.Add(this.statusBox);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.requestLabel);
            this.Controls.Add(this.ipLabel);
            this.Controls.Add(this.printHeaderCheckBox);
            this.Controls.Add(this.listenButton);
            this.Controls.Add(this.portInput);
            this.Controls.Add(this.ipInput);
            this.Controls.Add(this.portLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.portInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.TextBox ipInput;
        private System.Windows.Forms.NumericUpDown portInput;
        private System.Windows.Forms.Button listenButton;
        private System.Windows.Forms.CheckBox printHeaderCheckBox;
        private System.Windows.Forms.Label ipLabel;
        private System.Windows.Forms.Label requestLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.ListBox requestTextBox;
        private System.Windows.Forms.ListBox statusBox;
    }
}

