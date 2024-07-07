using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using TicTacToe3DApp.TicTacToe3DApp;

namespace TicTacToe3DApp
{
    public partial class Form1 : Form
    {
        private TicTacToe3D game;
        private Button[,,] buttons;
        private Player player1;
        private Player player2;
        private bool playerTurn;
        private int gridSize;
        private System.Windows.Forms.Timer timer;
        private int timeLeft;
        private bool aiVsAiMode;

        public Form1(int gridSize, bool playerVsAI, bool aiVsAiMode)
        {
            this.gridSize = gridSize;
            this.aiVsAiMode = aiVsAiMode;
            if (aiVsAiMode)
            {
                player1 = new AIPlayer(CellState.Opponent, "O");
                player2 = new AIPlayer(CellState.AI, "X");
            }
            else
            {
                player1 = new HumanPlayer(CellState.Opponent, "O");
                if (playerVsAI)
                {
                    player2 = new AIPlayer(CellState.AI, "X");
                }
                else
                {
                    player2 = new HumanPlayer(CellState.AI, "X");
                }
            }
            playerTurn = true;

            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            this.BackColor = System.Drawing.Color.FromArgb(240, 248, 255);
            this.Text = "Kółko i Krzyżyk [3D]";
            SetFormIcon();
            InitializeGame();
            InitializeTimer();

            if (aiVsAiMode)
            {
                StartAiVsAiGame();
            }
        }

        private async void StartAiVsAiGame()
        {
            while (true)
            {
                Player currentPlayer = playerTurn ? player1 : player2;
                currentPlayer.MakeMove(game, buttons);

                var winningCoords = game.GetWinningCoordinates(currentPlayer.CellState);
                if (winningCoords != null)
                {
                    HighlightWinningLine(winningCoords);
                    ShowEndGameDialog($"{currentPlayer.Sign} wins!");
                    break;
                }

                if (!game.IsMoveLeft())
                {
                    ShowEndGameDialog("It's a draw!");
                    break;
                }

                playerTurn = !playerTurn;

                await Task.Delay(10);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void SetFormIcon()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("TicTacToe3DApp.icon.png"))
            {
                if (stream != null)
                {
                    this.Icon = new Icon(stream);
                }
            }
        }

        private void InitializeTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timeLeft = 30;
            lblTimer.Text = $"Time: {timeLeft}";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft--;
                lblTimer.Text = $"Time: {timeLeft}";
            }
            else
            {
                timer.Stop();
                MessageBox.Show("Time's up! You lose!");
                ResetGame();
            }
        }

        private void StartTimer()
        {
            timeLeft = 30;
            lblTimer.Text = $"Time: {timeLeft}";
            timer.Start();
        }

        private void StopTimer()
        {
            timer.Stop();
        }

        private void InitializeGame()
        {
            game = new TicTacToe3D(gridSize);
            buttons = new Button[gridSize, gridSize, gridSize];
            CreateBoard();
        }

        private void HighlightWinningLine(List<Tuple<int, int, int>> winningCoords)
        {
            foreach (var (x, y, z) in winningCoords)
            {
                buttons[x, y, z].BackColor = Color.Yellow;
            }
        }

        private void CreateBoard()
        {
            int buttonSize = 50;

            int numRows;
            int numCols;

            if (gridSize % 3 == 0)
            {
                numRows = gridSize / 3;
                numCols = 3;
            }
            else if (gridSize % 2 == 0)
            {
                numRows = 2;
                numCols = gridSize / 2;
            }
            else if (gridSize == 7)
            {
                numRows = 2;
                numCols = 4;
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
                            BackColor = Color.FromArgb(240, 240, 240),
                            Font = new Font("Arial", 16, FontStyle.Bold),
                            FlatStyle = FlatStyle.Flat,
                            FlatAppearance = { BorderColor = Color.Black, BorderSize = 1 }
                        };

                        buttons[x, y, z].FlatAppearance.MouseOverBackColor = Color.FromArgb(210, 210, 210);
                        buttons[x, y, z].FlatAppearance.MouseDownBackColor = Color.FromArgb(180, 180, 180);

                        buttons[x, y, z].Click += Button_Click;
                        panel.Controls.Add(buttons[x, y, z], x, y);
                    }
                }

                tableLayoutPanel1.Controls.Add(panel, z % numCols, z / numCols);
            }

            this.ClientSize = new Size(buttonSize * gridSize * numCols, buttonSize * gridSize * numRows + 50);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            StopTimer();

            Button button = sender as Button;
            var (x, y, z) = (Tuple<int, int, int>)button.Tag;

            Player currentPlayer = playerTurn ? player1 : player2;
            currentPlayer.MakeMove(game, buttons, x, y, z);

            var winningCoords = game.GetWinningCoordinates(currentPlayer.CellState);
            if (winningCoords != null)
            {
                HighlightWinningLine(winningCoords);
                ShowEndGameDialog($"{currentPlayer.Sign} wins!");
                return;
            }

            if (!game.IsMoveLeft())
            {
                ShowEndGameDialog("It's a draw!");
                return;
            }

            playerTurn = !playerTurn;

            if (!playerTurn && player2 is AIPlayer)
            {
                player2.MakeMove(game, buttons);
                winningCoords = game.GetWinningCoordinates(player2.CellState);
                if (winningCoords != null)
                {
                    HighlightWinningLine(winningCoords);
                    ShowEndGameDialog("AI wins!");
                    return;
                }

                if (!game.IsMoveLeft())
                {
                    ShowEndGameDialog("It's a draw!");
                    return;
                }

                playerTurn = !playerTurn;
            }

            StartTimer();
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
            StopTimer();

            game = new TicTacToe3D(gridSize);
            for (int z = 0; z < gridSize; z++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    for (int x = 0; x < gridSize; x++)
                    {
                        buttons[x, y, z].Text = "";
                        buttons[x, y, z].Enabled = true;
                        buttons[x, y, z].BackColor = Color.FromArgb(240, 240, 240);
                        buttons[x, y, z].BackgroundImage = null;
                    }
                }
            }
            playerTurn = true;
            StartTimer();
        }
    }
}