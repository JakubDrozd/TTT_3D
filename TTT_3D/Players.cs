using System.Windows.Forms;

namespace TicTacToe3DApp
{
    namespace TicTacToe3DApp
    {
        abstract class Player
        {
            public string Sign { get; protected set; }
            public CellState CellState { get; protected set; }

            protected Player(CellState cellState, string sign)
            {
                CellState = cellState;
                Sign = sign;
            }

            public abstract void MakeMove(TicTacToe3D game, Button[,,] buttons, int x, int y, int z);
            public abstract void MakeMove(TicTacToe3D game, Button[,,] buttons);
        }



        class HumanPlayer : Player
        {
            public HumanPlayer(CellState cellState, string sign) : base(cellState, sign) { }

            public override void MakeMove(TicTacToe3D game, Button[,,] buttons, int x, int y, int z)
            {
                if (game.Board[x, y, z] != CellState.Empty) return;

                game.MakeMove(x, y, z, CellState);
                buttons[x, y, z].Text = Sign;
                buttons[x, y, z].Enabled = false;
                buttons[x, y, z].BackColor = CellState == CellState.Opponent ? System.Drawing.Color.FromArgb(144, 238, 144) : System.Drawing.Color.FromArgb(255, 182, 193);
            }

            public override void MakeMove(TicTacToe3D game, Button[,,] buttons)
            {
                throw new System.NotImplementedException();
            }
        }



        class AIPlayer : Player
        {
            public AIPlayer(CellState cellState, string sign) : base(cellState, sign) { }

            public override void MakeMove(TicTacToe3D game, Button[,,] buttons)
            {
                var bestMove = game.FindBestMove();
                if (bestMove != null)
                {
                    game.MakeMove(bestMove.Item1, bestMove.Item2, bestMove.Item3, CellState);
                    Button button = buttons[bestMove.Item1, bestMove.Item2, bestMove.Item3];
                    button.Text = Sign;
                    button.Enabled = false;
                    button.BackColor = System.Drawing.Color.FromArgb(255, 182, 193);
                }
            }

            public override void MakeMove(TicTacToe3D game, Button[,,] buttons, int x, int y, int z)
            {
                throw new System.NotImplementedException();
            }
        }


    }

}
