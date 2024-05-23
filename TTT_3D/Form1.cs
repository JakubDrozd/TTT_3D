using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace TicTacToe3DApp
{
    public partial class Form1 : Form
    {
        private int gridSize;
        private bool playerVsAI;
        private TicTacToe3D game;
        private Button[,,] buttons;
        private bool playerTurn;


        public Form1(int gridSize, bool playerVsAI)
        {
            this.gridSize = gridSize;
            this.playerVsAI = playerVsAI;
            playerTurn = true; // Zawsze zaczyna gracz

            InitializeComponent();
            this.BackColor = Color.FromArgb(240, 248, 255); // Pastelowy kolor tła (AliceBlue)
            this.Text = "Kółko i Krzyżyk [3D]"; // Tytuł paska tytułu
            SetFormIcon();
            InitializeGame();
        }

        private void SetFormIcon()
        {
            // Wczytaj ikonę z zasobów osadzonego pliku
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("TicTacToe3DApp.icon.png"))
            {
                if (stream != null)
                {
                    this.Icon = new Icon(stream);
                }
            }
        }


        private void InitializeGame()
        {
            game = new TicTacToe3D(gridSize);
            buttons = new Button[gridSize, gridSize, gridSize];
            CreateBoard();
        }

        private void CreateBoard()
        {
            int buttonSize = 50;

            int numRows;
            int numCols;

            if (gridSize % 3 == 0)
            {
                numRows = 3;
                numCols = gridSize / 3;
            }
            else if (gridSize % 2 == 0)
            {
                numRows = 2;
                numCols = gridSize / 2;
            }
            else
            {
                numRows = 1;
                numCols = gridSize;
            }

            tableLayoutPanel1.RowCount = numRows;
            tableLayoutPanel1.ColumnCount = numCols;

            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.ColumnStyles.Clear();

            for (int i = 0; i < numRows; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / numRows));
            }
            for (int i = 0; i < numCols; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / numCols));
            }

            for (int z = 0; z < gridSize; z++)
            {
                var panel = new TableLayoutPanel
                {
                    RowCount = gridSize,
                    ColumnCount = gridSize,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(5)
                };

                for (int i = 0; i < gridSize; i++)
                {
                    panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / gridSize));
                    panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / gridSize));
                }

                for (int y = 0; y < gridSize; y++)
                {
                    for (int x = 0; x < gridSize; x++)
                    {
                        buttons[x, y, z] = new Button
                        {
                            Dock = DockStyle.Fill,
                            Margin = new Padding(1),
                            Tag = new Tuple<int, int, int>(x, y, z),
                            BackColor = Color.White,
                            Font = new Font("Arial", 16, FontStyle.Bold),
                            FlatStyle = FlatStyle.Flat,
                            FlatAppearance = { BorderColor = Color.Black, BorderSize = 1 }
                        };
                        buttons[x, y, z].Click += Button_Click;
                        panel.Controls.Add(buttons[x, y, z], x, y);
                    }
                }

                tableLayoutPanel1.Controls.Add(panel, z % numCols, z / numCols);
            }

            this.ClientSize = new Size(buttonSize * gridSize * numCols, buttonSize * gridSize * numRows);
        }




        private void HighlightWinningLine(List<Tuple<int, int, int>> winningCoords)
        {
            foreach (var coord in winningCoords)
            {
                int x = coord.Item1;
                int y = coord.Item2;
                int z = coord.Item3;

                buttons[x, y, z].BackColor = Color.Yellow;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            var (x, y, z) = (Tuple<int, int, int>)button.Tag;

            if (game.Board[x, y, z] != CellState.Empty) return;

            if (playerTurn)
            {
                game.MakeMove(x, y, z, CellState.Opponent);
                button.Text = "O";
                button.Enabled = false;
                button.BackColor = Color.FromArgb(144, 238, 144); // Pastelowy zielony

                var winningCoords = game.GetWinningCoordinates(CellState.Opponent);
                if (winningCoords != null)
                {
                    HighlightWinningLine(winningCoords);
                    ShowEndGameDialog("Player O wins!");
                    return;
                }

                if (!game.IsMoveLeft())
                {
                    ShowEndGameDialog("It's a draw!");
                    return;
                }

                if (playerVsAI)
                {
                    var bestMove = game.FindBestMove();
                    if (bestMove != null)
                    {
                        game.MakeMove(bestMove.Item1, bestMove.Item2, bestMove.Item3, CellState.AI);
                        buttons[bestMove.Item1, bestMove.Item2, bestMove.Item3].Text = "X";
                        buttons[bestMove.Item1, bestMove.Item2, bestMove.Item3].Enabled = false;
                        buttons[bestMove.Item1, bestMove.Item2, bestMove.Item3].BackColor = Color.FromArgb(255, 182, 193); // Pastelowy czerwony

                        winningCoords = game.GetWinningCoordinates(CellState.AI);
                        if (winningCoords != null)
                        {
                            HighlightWinningLine(winningCoords);
                            ShowEndGameDialog("AI wins!");
                            return;
                        }
                    }

                    if (!game.IsMoveLeft())
                    {
                        ShowEndGameDialog("It's a draw!");
                        return;
                    }
                }
                else
                {
                    playerTurn = !playerTurn;
                }
            }
            else
            {
                game.MakeMove(x, y, z, CellState.AI);
                button.Text = "X";
                button.Enabled = false;
                button.BackColor = Color.FromArgb(255, 182, 193); // Pastelowy czerwony

                var winningCoords = game.GetWinningCoordinates(CellState.AI);
                if (winningCoords != null)
                {
                    HighlightWinningLine(winningCoords);
                    ShowEndGameDialog("Player X wins!");
                    return;
                }

                if (!game.IsMoveLeft())
                {
                    ShowEndGameDialog("It's a draw!");
                    return;
                }

                playerTurn = !playerTurn;
            }
        }

        private void ShowEndGameDialog(string message)
        {
            var result = MessageBox.Show($"{message}\nDo you want to play again?", "Game Over", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                ResetGame();
            }
            else
            {
                Application.Exit();
            }
        }


        private void ResetGame()
        {
            game = new TicTacToe3D(gridSize);
            for (int z = 0; z < gridSize; z++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    for (int x = 0; x < gridSize; x++)
                    {
                        buttons[x, y, z].Text = "";
                        buttons[x, y, z].Enabled = true;
                        buttons[x, y, z].BackColor = Color.White;
                    }
                }
            }
            playerTurn = true; // Zawsze zaczyna gracz
        }

    }
}
