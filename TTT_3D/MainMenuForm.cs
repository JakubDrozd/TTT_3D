using System;
using System.Windows.Forms;

namespace TicTacToe3DApp
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            int gridSize = (int)numericUpDownSize.Value;
            bool playerVsAI = rbtnPlayerVsAI.Checked;
            Form1 gameForm = new Form1(gridSize, playerVsAI);
            gameForm.Show();
            this.Hide();
        }
    }
}
