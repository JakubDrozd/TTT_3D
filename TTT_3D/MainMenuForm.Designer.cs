namespace TicTacToe3DApp
{
    partial class MainMenuForm
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
            this.btnPlayLocal = new System.Windows.Forms.Button();
            this.btnHostGame = new System.Windows.Forms.Button();
            this.btnJoinGame = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPlayLocal
            // 
            this.btnPlayLocal.Location = new System.Drawing.Point(12, 12);
            this.btnPlayLocal.Name = "btnPlayLocal";
            this.btnPlayLocal.Size = new System.Drawing.Size(260, 40);
            this.btnPlayLocal.TabIndex = 0;
            this.btnPlayLocal.Text = "Play Locally";
            this.btnPlayLocal.UseVisualStyleBackColor = true;
            this.btnPlayLocal.Click += new System.EventHandler(this.BtnPlayLocal_Click);
            // 
            // btnHostGame
            // 
            this.btnHostGame.Location = new System.Drawing.Point(12, 58);
            this.btnHostGame.Name = "btnHostGame";
            this.btnHostGame.Size = new System.Drawing.Size(260, 40);
            this.btnHostGame.TabIndex = 1;
            this.btnHostGame.Text = "Host Game";
            this.btnHostGame.UseVisualStyleBackColor = true;
            this.btnHostGame.Click += new System.EventHandler(this.BtnHostGame_Click);
            // 
            // btnJoinGame
            // 
            this.btnJoinGame.Location = new System.Drawing.Point(12, 104);
            this.btnJoinGame.Name = "btnJoinGame";
            this.btnJoinGame.Size = new System.Drawing.Size(260, 40);
            this.btnJoinGame.TabIndex = 2;
            this.btnJoinGame.Text = "Join Game";
            this.btnJoinGame.UseVisualStyleBackColor = true;
            this.btnJoinGame.Click += new System.EventHandler(this.BtnJoinGame_Click);
            // 
            // MainMenuForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 161);
            this.Controls.Add(this.btnJoinGame);
            this.Controls.Add(this.btnHostGame);
            this.Controls.Add(this.btnPlayLocal);
            this.Name = "MainMenuForm";
            this.Text = "Main Menu";
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button btnPlayLocal;
        private System.Windows.Forms.Button btnHostGame;
        private System.Windows.Forms.Button btnJoinGame;
    }
}
