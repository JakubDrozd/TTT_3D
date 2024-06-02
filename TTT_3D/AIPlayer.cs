using TicTacToe3DApp;

namespace TTT_3D
{
    public class AIPlayer
    {
        public string Sign { get; private set; }
        private Form1 form;

        public AIPlayer(string sign, Form1 form)
        {
            Sign = sign;
            this.form = form;
        }

        public void MakeMove()
        {
            var bestMove = form.Game.FindBestMove();
            if (bestMove != null)
            {
                form.PerformMove(bestMove.Item1, bestMove.Item2, bestMove.Item3, CellState.AI);
            }
            form.Player1Turn = !form.Player1Turn; // Zmiana tury
            form.NextMove();
        }
    }


}
