namespace TicTacToe3DApp
{
    partial class MainMenuForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.RadioButton rbtnPlayerVsPlayer;
        private System.Windows.Forms.RadioButton rbtnPlayerVsAI;
        private System.Windows.Forms.NumericUpDown numericUpDownSize;
        private System.Windows.Forms.Label lblSize;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.btnPlay = new System.Windows.Forms.Button();
            this.rbtnPlayerVsPlayer = new System.Windows.Forms.RadioButton();
            this.rbtnPlayerVsAI = new System.Windows.Forms.RadioButton();
            this.numericUpDownSize = new System.Windows.Forms.NumericUpDown();
            this.lblSize = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSize)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(120, 180);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(120, 30);
            this.btnPlay.TabIndex = 0;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.BtnPlay_Click);
            // 
            // rbtnPlayerVsPlayer
            // 
            this.rbtnPlayerVsPlayer.AutoSize = true;
            this.rbtnPlayerVsPlayer.Location = new System.Drawing.Point(120, 60);
            this.rbtnPlayerVsPlayer.Name = "rbtnPlayerVsPlayer";
            this.rbtnPlayerVsPlayer.Size = new System.Drawing.Size(134, 24);
            this.rbtnPlayerVsPlayer.TabIndex = 1;
            this.rbtnPlayerVsPlayer.TabStop = true;
            this.rbtnPlayerVsPlayer.Text = "Player vs Player";
            this.rbtnPlayerVsPlayer.UseVisualStyleBackColor = true;
            // 
            // rbtnPlayerVsAI
            // 
            this.rbtnPlayerVsAI.AutoSize = true;
            this.rbtnPlayerVsAI.Location = new System.Drawing.Point(120, 90);
            this.rbtnPlayerVsAI.Name = "rbtnPlayerVsAI";
            this.rbtnPlayerVsAI.Size = new System.Drawing.Size(114, 24);
            this.rbtnPlayerVsAI.TabIndex = 2;
            this.rbtnPlayerVsAI.TabStop = true;
            this.rbtnPlayerVsAI.Text = "Player vs AI";
            this.rbtnPlayerVsAI.UseVisualStyleBackColor = true;
            // 
            // numericUpDownSize
            // 
            this.numericUpDownSize.Location = new System.Drawing.Point(200, 120);
            this.numericUpDownSize.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numericUpDownSize.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownSize.Name = "numericUpDownSize";
            this.numericUpDownSize.Size = new System.Drawing.Size(40, 26);
            this.numericUpDownSize.TabIndex = 3;
            this.numericUpDownSize.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(120, 122);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(74, 20);
            this.lblSize.TabIndex = 4;
            this.lblSize.Text = "Grid Size:";
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 240);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.numericUpDownSize);
            this.Controls.Add(this.rbtnPlayerVsAI);
            this.Controls.Add(this.rbtnPlayerVsPlayer);
            this.Controls.Add(this.btnPlay);
            this.Name = "MainMenuForm";
            this.Text = "Main Menu";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}