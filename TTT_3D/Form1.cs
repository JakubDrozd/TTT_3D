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
        private bool playerTurn;
        private bool playerVsAI;
        private int gridSize;
        private Server server;
        private Client client;
        private bool isServer;
        private bool isClient;

        public Form1(int gridSize, bool isServer, bool isClient, bool playerVsAI = false, string ipAddress = null, int port = 0)
        {
            this.gridSize = gridSize;
            this.playerVsAI = playerVsAI; // Upewnij się, że ten wiersz jest obecny
            this.isServer = isServer;
            this.isClient = isClient;
            playerTurn = true; // Zawsze zaczyna gracz

            InitializeComponent();
            this.BackColor = Color.FromArgb(240, 248, 255); // Pastelowy kolor tła (AliceBlue)
            this.Text = "Kółko i Krzyżyk [3D]"; // Tytuł paska tytułu
            SetFormIcon();
            InitializeGame();

            if (isServer)
            {
                InitializeNetwork(true, false, port: port);
            }
            else if (isClient)
            {
                InitializeNetwork(false, true, ipAddress: ipAddress, port: port);
            }
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

        private void InitializeNetwork(bool isServer, bool isClient, string ipAddress = null, int port = 0)
        {
            this.isServer = isServer;
            this.isClient = isClient;

            if (isServer)
            {
                server = new Server();
                server.OnMessageReceived += Server_OnMessageReceived;
                server.Start(port);
            }
            else if (isClient)
            {
                client = new Client();
                client.OnMessageReceived += Client_OnMessageReceived;
                client.Connect(ipAddress, port);
            }
        }

        private void Server_OnMessageReceived(string message)
        {
            // Obsługa wiadomości od klienta
            Invoke(new Action(() =>
            {
                ProcessMoveMessage(message);
            }));
        }

        private void Client_OnMessageReceived(string message)
        {
            // Obsługa wiadomości od serwera
            Invoke(new Action(() =>
            {
                ProcessMoveMessage(message);
            }));
        }

        private void ProcessMoveMessage(string message)
        {
            var parts = message.Split(',');
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            int z = int.Parse(parts[2]);
            CellState player = (CellState)Enum.Parse(typeof(CellState), parts[3]);

            buttons[x, y, z].Text = player == CellState.Opponent ? "O" : "X";
            buttons[x, y, z].Enabled = false;
            buttons[x, y, z].BackColor = player == CellState.Opponent ? Color.FromArgb(144, 238, 144) : Color.FromArgb(255, 182, 193);
            game.MakeMove(x, y, z, player);
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

            this.ClientSize = new Size(buttonSize * gridSize * numCols, buttonSize * gridSize * numRows);
        }

        private void InitializeGame()
        {
            game = new TicTacToe3D(gridSize);
            buttons = new Button[gridSize, gridSize, gridSize];
            CreateBoard();
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            var (x, y, z) = (Tuple<int, int, int>)button.Tag;

            if (game.Board[x, y, z] != CellState.Empty) return;

            var currentPlayer = playerTurn ? CellState.Opponent : CellState.AI;
            game.MakeMove(x, y, z, currentPlayer);
            button.Text = currentPlayer == CellState.Opponent ? "O" : "X";
            button.Enabled = false;
            button.BackColor = currentPlayer == CellState.Opponent ? Color.FromArgb(144, 238, 144) : Color.FromArgb(255, 182, 193);
            string moveMessage = $"{x},{y},{z},{currentPlayer}";

            if (isServer)
            {
                server.SendMessage(moveMessage);
            }
            else if (isClient)
            {
                client.SendMessage(moveMessage);
            }

            var winningCoords = game.GetWinningCoordinates(currentPlayer);
            if (winningCoords != null)
            {
                HighlightWinningLine(winningCoords);
                ShowEndGameDialog(currentPlayer == CellState.Opponent ? "Player O wins!" : "Player X wins!");
                return;
            }

            if (!game.IsMoveLeft())
            {
                ShowEndGameDialog("It's a draw!");
                return;
            }

            if (playerVsAI && currentPlayer == CellState.Opponent)
            {
                var bestMove = game.FindBestMove();
                if (bestMove != null)
                {
                    game.MakeMove(bestMove.Item1, bestMove.Item2, bestMove.Item3, CellState.AI);
                    buttons[bestMove.Item1, bestMove.Item2, bestMove.Item3].Text = "X";
                    buttons[bestMove.Item1, bestMove.Item2, bestMove.Item3].Enabled = false;
                    buttons[bestMove.Item1, bestMove.Item2, bestMove.Item3].BackColor = Color.FromArgb(255, 182, 193);

                    winningCoords = game.GetWinningCoordinates(CellState.AI);
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
                }
            }
            else
            {
                playerTurn = !playerTurn;
            }
        }


        private void HighlightWinningLine(List<Tuple<int, int, int>> winningCoords)
        {
            foreach (var (x, y, z) in winningCoords)
            {
                buttons[x, y, z].BackColor = Color.Yellow;
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
                if (isServer)
                {
                    server.Stop();
                }
                else if (isClient)
                {
                    client.Disconnect();
                }
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
                        buttons[x, y, z].BackColor = Color.FromArgb(240, 240, 240); // Łagodniejszy kolor
                        buttons[x, y, z].BackgroundImage = null; // Usunięcie grafiki tła
                    }
                }
            }
            playerTurn = true; // Zawsze zaczyna gracz
        }

        private Image LoadImageFromFile(string fileName)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (File.Exists(path))
            {
                return Image.FromFile(path);
            }
            return null;
        }
    }
}
