using System;
using System.Windows.Controls;
using TicTacToe3DApp;

namespace TTT_3D
{
    public class HumanPlayer
    {
        public string Sign { get; private set; }
        private Form1 form;

        public HumanPlayer(string sign, Form1 form)
        {
            Sign = sign;
            this.form = form;
        }

        public void MakeMove(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            var (x, y, z) = (Tuple<int, int, int>)button.Tag;

            if (form.Game.Board[x, y, z] != CellState.Empty) return;

            form.PerformMove(x, y, z, form.Player1Turn ? CellState.Opponent : CellState.AI);
            form.Player1Turn = !form.Player1Turn; // Zmiana tury
            form.NextMove();
        }
    }






}
