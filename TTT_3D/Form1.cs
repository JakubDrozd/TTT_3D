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
        private TicTacToe3D game;
        private Button[,,] buttons;
        private Player player1;
        private Player player2;
        private bool playerTurn;
        private int gridSize;
        private Timer timer;
        private int timeLeft;

        public Form1(int gridSize, bool playerVsAI)
        {
            this.gridSize = gridSize;
            player1 = new HumanPlayer(CellState.Opponent);
            if (playerVsAI)
            {
                player2 = new AIPlayer(CellState.AI);
            }
            else
            {
                player2 = new HumanPlayer(CellState.AI);
            }
            playerTurn = true; // Zawsze zaczyna gracz

            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            this.BackColor = System.Drawing.Color.FromArgb(240, 248, 255); // Pastelowy kolor tła (AliceBlue)
            this.Text = "Kółko i Krzyżyk [3D]"; // Tytuł paska tytułu
            SetFormIcon();
            InitializeGame();
            InitializeTimer();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
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

        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = 1000; // 1 sekunda
            timer.Tick += Timer_Tick;
            timeLeft = 30; // 30 sekund na ruch
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
                ResetGame(); // Można również dodać inne działanie, np. zakończenie gry
            }
        }

        private void StartTimer()
        {
            timeLeft = 30; // Reset the timer
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
                            BackColor = Color.FromArgb(240, 240, 240), // Łagodniejszy kolor
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
            StopTimer(); // Zatrzymaj timer przed przetworzeniem ruchu

            Button button = sender as Button;
            var (x, y, z) = (Tuple<int, int, int>)button.Tag;

            if (game.Board[x, y, z] != CellState.Empty) return;

            Player currentPlayer = playerTurn ? player1 : player2;
            game.MakeMove(x, y, z, currentPlayer.CellState);

            button.Text = currentPlayer.CellState == CellState.Opponent ? "O" : "X";
            button.Enabled = false;
            button.BackColor = currentPlayer.CellState == CellState.Opponent ? System.Drawing.Color.FromArgb(144, 238, 144) : System.Drawing.Color.FromArgb(255, 182, 193);

            var winningCoords = game.GetWinningCoordinates(currentPlayer.CellState);
            if (winningCoords != null)
            {
                HighlightWinningLine(winningCoords);
                ShowEndGameDialog(currentPlayer.CellState == CellState.Opponent ? "Player O wins!" : "Player X wins!");
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

            StartTimer(); // Uruchom timer dla kolejnego ruchu
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
            StopTimer(); // Zatrzymaj timer przy resetowaniu gry

            game = new TicTacToe3D(gridSize);
            for (int z = 0; z < gridSize; z++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    for (int x = 0; x < gridSize; x++)
                    {
                        buttons[x, y, z].Text = "";
                        buttons[x, y, z].Enabled = true;
                        buttons[x, y, z].BackColor = Color.FromArgb(240, 240, 240); // Łagodniejszy kolor
                        buttons[x, y, z].BackgroundImage = null; // Usunięcie grafiki tła
                    }
                }
            }
            playerTurn = true; // Zawsze zaczyna gracz
            StartTimer(); // Uruchom timer dla nowej gry
        }
    }
}