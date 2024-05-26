using System;
using System.Windows.Forms;

namespace TicTacToe3DApp
{
    public partial class JoinGameForm : Form
    {
        public string IPAddress { get; private set; }
        public int Port { get; private set; }

        public JoinGameForm()
        {
            InitializeComponent();
        }

        private void BtnJoin_Click(object sender, EventArgs e)
        {
            IPAddress = txtIPAddress.Text;
            Port = int.Parse(txtPort.Text);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
