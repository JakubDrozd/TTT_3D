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

        private void BtnPlayLocal_Click(object sender, EventArgs e)
        {
            var localGameForm = new LocalGameForm();
            if (localGameForm.ShowDialog() == DialogResult.OK)
            {
                var gameForm = new Form1(localGameForm.GridSize, false, false, localGameForm.IsAI);
                gameForm.Show();
            }
        }

        private void BtnHostGame_Click(object sender, EventArgs e)
        {
            var networkGameForm = new NetworkGameForm();
            if (networkGameForm.ShowDialog() == DialogResult.OK)
            {
                var gameForm = new Form1(networkGameForm.GridSize, true, false, false, port: 12345);
                gameForm.Show();
            }
        }

        private void BtnJoinGame_Click(object sender, EventArgs e)
        {
            var networkGameForm = new NetworkGameForm();
            if (networkGameForm.ShowDialog() == DialogResult.OK)
            {
                var joinGameForm = new JoinGameForm();
                if (joinGameForm.ShowDialog() == DialogResult.OK)
                {
                    var gameForm = new Form1(networkGameForm.GridSize, false, true, false, ipAddress: joinGameForm.IPAddress, port: joinGameForm.Port);
                    gameForm.Show();
                }
            }
        }
    }
}
