using System.Windows.Forms;

namespace TicTacToe3DApp
{
    abstract class Player
    {
        public CellState CellState { get; protected set; }
        public abstract void MakeMove(TicTacToe3D game, Button[,,] buttons);
    }

    class HumanPlayer : Player
    {
        public HumanPlayer(CellState cellState)
        {
            CellState = cellState;
        }

        public override void MakeMove(TicTacToe3D game, Button[,,] buttons)
        {
            // Human player move logic is handled by Button_Click event in Form1
        }
    }

    class AIPlayer : Player
    {
        public AIPlayer(CellState cellState)
        {
            CellState = cellState;
        }

        public override void MakeMove(TicTacToe3D game, Button[,,] buttons)
        {
            var bestMove = game.FindBestMove();
            if (bestMove != null)
            {
                game.MakeMove(bestMove.Item1, bestMove.Item2, bestMove.Item3, CellState);
                Button button = buttons[bestMove.Item1, bestMove.Item2, bestMove.Item3];
                button.Text = "X";
                button.Enabled = false;
                button.BackColor = System.Drawing.Color.FromArgb(255, 182, 193);
            }
        }
    }
}
