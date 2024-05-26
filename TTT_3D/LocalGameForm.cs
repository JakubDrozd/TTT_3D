using System;
using System.Windows.Forms;

namespace TicTacToe3DApp
{
    public partial class LocalGameForm : Form
    {
        public int GridSize { get; private set; }
        public bool IsAI { get; private set; }

        public LocalGameForm()
        {
            InitializeComponent();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            GridSize = (int)numericUpDownGridSize.Value;
            IsAI = radioButtonAI.Checked;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
