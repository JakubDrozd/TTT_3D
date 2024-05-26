using System;
using System.Windows.Forms;

namespace TicTacToe3DApp
{
    public partial class NetworkGameForm : Form
    {
        public int GridSize { get; private set; }

        public NetworkGameForm()
        {
            InitializeComponent();
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            GridSize = (int)numericUpDownGridSize.Value;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
