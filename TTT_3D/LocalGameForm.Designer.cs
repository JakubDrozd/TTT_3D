namespace TicTacToe3DApp
{
    partial class LocalGameForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.numericUpDownGridSize = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonPVP = new System.Windows.Forms.RadioButton();
            this.radioButtonAI = new System.Windows.Forms.RadioButton();
            this.btnStart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGridSize)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDownGridSize
            // 
            this.numericUpDownGridSize.Location = new System.Drawing.Point(12, 29);
            this.numericUpDownGridSize.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numericUpDownGridSize.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownGridSize.Name = "numericUpDownGridSize";
            this.numericUpDownGridSize.Size = new System.Drawing.Size(120, 26);
            this.numericUpDownGridSize.TabIndex = 0;
            this.numericUpDownGridSize.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Grid Size";
            // 
            // radioButtonPVP
            // 
            this.radioButtonPVP.AutoSize = true;
            this.radioButtonPVP.Checked = true;
            this.radioButtonPVP.Location = new System.Drawing.Point(12, 61);
            this.radioButtonPVP.Name = "radioButtonPVP";
            this.radioButtonPVP.Size = new System.Drawing.Size(64, 24);
            this.radioButtonPVP.TabIndex = 2;
            this.radioButtonPVP.TabStop = true;
            this.radioButtonPVP.Text = "PVP";
            this.radioButtonPVP.UseVisualStyleBackColor = true;
            // 
            // radioButtonAI
            // 
            this.radioButtonAI.AutoSize = true;
            this.radioButtonAI.Location = new System.Drawing.Point(12, 91);
            this.radioButtonAI.Name = "radioButtonAI";
            this.radioButtonAI.Size = new System.Drawing.Size(47, 24);
            this.radioButtonAI.TabIndex = 3;
            this.radioButtonAI.Text = "AI";
            this.radioButtonAI.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 121);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(120, 34);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // LocalGameForm
            // 
            this.ClientSize = new System.Drawing.Size(154, 167);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.radioButtonAI);
            this.Controls.Add(this.radioButtonPVP);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownGridSize);
            this.Name = "LocalGameForm";
            this.Text = "Local Game";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGridSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.NumericUpDown numericUpDownGridSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButtonPVP;
        private System.Windows.Forms.RadioButton radioButtonAI;
        private System.Windows.Forms.Button btnStart;
    }
}
