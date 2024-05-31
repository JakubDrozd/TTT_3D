using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TicTacToe3DApp
{
    enum CellState
    {
        Empty,
        AI,
        Opponent
    }

    class TicTacToe3D
    {
        private const int MaxInARowToWin = 5;
        private int inARowToWin;
        private int gridSize;
        private CellState[,,] gameBoard;
        private Stopwatch stopwatch;
        private const int MaxTime = 1000; // Max time in milliseconds for AI to make a decision

        public CellState[,,] Board => gameBoard;

        public TicTacToe3D(int gridSize)
        {
            this.gridSize = gridSize;
            inARowToWin = gridSize >= MaxInARowToWin ? MaxInARowToWin : gridSize;
            gameBoard = new CellState[gridSize, gridSize, gridSize];
            stopwatch = new Stopwatch();

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    for (int z = 0; z < gridSize; z++)
                    {
                        gameBoard[x, y, z] = CellState.Empty;
                    }
                }
            }
        }

        public bool IsMoveLeft()
        {
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    for (int z = 0; z < gridSize; z++)
                    {
                        if (gameBoard[x, y, z] == CellState.Empty)
                            return true;
                    }
                }
            }
            return false;
        }

        private List<Tuple<int, int, int>> CheckForBlockingMove(CellState player, int requiredCount)
        {
            var directions = new List<(int, int, int)>
    {
        // Proste linie
        (1, 0, 0), (0, 1, 0), (0, 0, 1),
        // Przekątne na płaszczyźnie XY
        (1, 1, 0), (-1, 1, 0),
        // Przekątne na płaszczyźnie XZ
        (1, 0, 1), (-1, 0, 1),
        // Przekątne na płaszczyźnie YZ
        (0, 1, 1), (0, -1, 1),
        // Przekątne 3D
        (1, 1, 1), (-1, 1, 1), (1, -1, 1), (-1, -1, 1),
        (1, 1, -1), (-1, 1, -1), (1, -1, -1), (-1, -1, -1)
    };

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    for (int z = 0; z < gridSize; z++)
                    {
                        if (gameBoard[x, y, z] == player)
                        {
                            foreach (var dir in directions)
                            {
                                int count = 0;
                                Tuple<int, int, int> emptyCell = null;
                                for (int i = 0; i < inARowToWin; i++)
                                {
                                    int newX = x + i * dir.Item1;
                                    int newY = y + i * dir.Item2;
                                    int newZ = z + i * dir.Item3;

                                    if (newX < 0 || newX >= gridSize || newY < 0 || newY >= gridSize || newZ < 0 || newZ >= gridSize)
                                        break;

                                    if (gameBoard[newX, newY, newZ] == player)
                                        count++;
                                    else if (gameBoard[newX, newY, newZ] == CellState.Empty)
                                        emptyCell = new Tuple<int, int, int>(newX, newY, newZ);
                                    else
                                        break;
                                }

                                if (count == requiredCount && emptyCell != null)
                                    return new List<Tuple<int, int, int>> { emptyCell };
                            }
                        }
                    }
                }
            }
            return null;
        }


        private List<Tuple<int, int, int>> IsLineComplete(int x, int y, int z, (int dx, int dy, int dz) direction, CellState player)
        {
            List<Tuple<int, int, int>> winningCoords = new List<Tuple<int, int, int>>
            {
                new Tuple<int, int, int>(x, y, z)
            };

            for (int i = 1; i < inARowToWin; i++)
            {
                int newX = x + i * direction.dx;
                int newY = y + i * direction.dy;
                int newZ = z + i * direction.dz;

                if (newX < 0 || newX >= gridSize || newY < 0 || newY >= gridSize || newZ < 0 || newZ >= gridSize)
                    return null;

                if (gameBoard[newX, newY, newZ] != player)
                    return null;

                winningCoords.Add(new Tuple<int, int, int>(newX, newY, newZ));
            }

            return winningCoords;
        }

        private List<Tuple<int, int, int>> CheckForWinner(int x, int y, int z)
        {
            var directions = new List<(int, int, int)>
            {
                // Proste linie
                (1, 0, 0), (0, 1, 0), (0, 0, 1),
                // Przekątne na płaszczyźnie XY
                (1, 1, 0), (-1, 1, 0),
                // Przekątne na płaszczyźnie XZ
                (1, 0, 1), (-1, 0, 1),
                // Przekątne na płaszczyźnie YZ
                (0, 1, 1), (0, -1, 1),
                // Przekątne 3D
                (1, 1, 1), (-1, 1, 1), (1, -1, 1), (-1, -1, 1),
                (1, 1, -1), (-1, 1, -1), (1, -1, -1), (-1, -1, -1)
            };

            foreach (var dir in directions)
            {
                var winningCoords = IsLineComplete(x, y, z, dir, gameBoard[x, y, z]);
                if (winningCoords != null)
                    return winningCoords;
            }

            return null;
        }

        public List<Tuple<int, int, int>> GetWinningCoordinates(CellState player)
        {
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    for (int z = 0; z < gridSize; z++)
                    {
                        if (gameBoard[x, y, z] == player)
                        {
                            var winningCoords = CheckForWinner(x, y, z);
                            if (winningCoords != null)
                                return winningCoords;
                        }
                    }
                }
            }
            return null;
        }

        public bool IsWinner(CellState player)
        {
            return GetWinningCoordinates(player) != null;
        }

        private List<Tuple<int, int, int>> GetCriticalMoves(CellState player, int requiredCount)
        {
            var directions = new List<(int, int, int)>
    {
        (1, 0, 0), (0, 1, 0), (0, 0, 1),
        (1, 1, 0), (-1, 1, 0),
        (1, 0, 1), (-1, 0, 1),
        (0, 1, 1), (0, -1, 1),
        (1, 1, 1), (-1, 1, 1), (1, -1, 1), (-1, -1, 1),
        (1, 1, -1), (-1, 1, -1), (1, -1, -1), (-1, -1, -1)
    };

            var criticalMoves = new List<Tuple<int, int, int>>();

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    for (int z = 0; z < gridSize; z++)
                    {
                        if (gameBoard[x, y, z] == player)
                        {
                            foreach (var dir in directions)
                            {
                                int count = 0;
                                Tuple<int, int, int> emptyCell = null;
                                for (int i = 0; i < inARowToWin; i++)
                                {
                                    int newX = x + i * dir.Item1;
                                    int newY = y + i * dir.Item2;
                                    int newZ = z + i * dir.Item3;

                                    if (newX < 0 || newX >= gridSize || newY < 0 || newY >= gridSize || newZ < 0 || newZ >= gridSize)
                                        break;

                                    if (gameBoard[newX, newY, newZ] == player)
                                        count++;
                                    else if (gameBoard[newX, newY, newZ] == CellState.Empty)
                                        emptyCell = new Tuple<int, int, int>(newX, newY, newZ);
                                    else
                                        break;
                                }

                                if (count == requiredCount && emptyCell != null)
                                    criticalMoves.Add(emptyCell);
                            }
                        }
                    }
                }
            }

            return criticalMoves;
        }


        public int EvaluateMove(int x, int y, int z)
        {
            int score = 0;

            // Preferowanie środkowych pozycji
            int center = gridSize / 2;
            if (x == center && y == center && z == center)
            {
                score += 3; // Preferuj centrum
            }
            else if (x == center || y == center || z == center)
            {
                score += 2; // Preferuj linie środkowe
            }
            else
            {
                score += 1; // Preferuj pozostałe
            }

            // Dodatkowa heurystyka dla blokowania przeciwnika
            gameBoard[x, y, z] = CellState.Opponent;
            if (IsWinner(CellState.Opponent))
            {
                score += 100;
            }
            gameBoard[x, y, z] = CellState.Empty;

            // Sprawdzenie blokady dla przeciwnika z trzema elementami w kombinacji
            var criticalMoves = GetCriticalMoves(CellState.Opponent, 3);
            if (criticalMoves.Any(m => m.Item1 == x && m.Item2 == y && m.Item3 == z))
            {
                score += 50;
            }

            // Sprawdzenie blokady dla przeciwnika z czterema elementami w kombinacji
            criticalMoves = GetCriticalMoves(CellState.Opponent, 4);
            if (criticalMoves.Any(m => m.Item1 == x && m.Item2 == y && m.Item3 == z))
            {
                score += 200;
            }

            return score;
        }




        private List<Tuple<int, int, int>> GetOrderedMoves()
        {
            var moves = new List<Tuple<int, int, int>>();
            var scoredMoves = new List<Tuple<int, int, int, int>>();

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    for (int z = 0; z < gridSize; z++)
                    {
                        if (gameBoard[x, y, z] == CellState.Empty)
                        {
                            int score = EvaluateMove(x, y, z);
                            scoredMoves.Add(new Tuple<int, int, int, int>(x, y, z, score));
                        }
                    }
                }
            }

            scoredMoves = scoredMoves.OrderByDescending(move => move.Item4).ToList();

            foreach (var move in scoredMoves)
            {
                moves.Add(new Tuple<int, int, int>(move.Item1, move.Item2, move.Item3));
            }

            return moves;
        }


        public int Minimax(int depth, int maxDepth, bool isMaximizing, int alpha, int beta)
        {
            var winner = GetWinner();
            if (winner == CellState.AI)
                return 1000 - depth;
            if (winner == CellState.Opponent)
                return depth - 1000;
            if (!IsMoveLeft() || depth >= maxDepth)
                return 0;

            var moves = GetOrderedMoves();

            if (isMaximizing)
            {
                int maxEval = int.MinValue;
                foreach (var move in moves)
                {
                    gameBoard[move.Item1, move.Item2, move.Item3] = CellState.AI;
                    int eval = Minimax(depth + 1, maxDepth, false, alpha, beta);
                    gameBoard[move.Item1, move.Item2, move.Item3] = CellState.Empty;
                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                    {
                        break; // Cut-off
                    }
                }
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                foreach (var move in moves)
                {
                    gameBoard[move.Item1, move.Item2, move.Item3] = CellState.Opponent;
                    int eval = Minimax(depth + 1, maxDepth, true, alpha, beta);
                    gameBoard[move.Item1, move.Item2, move.Item3] = CellState.Empty;
                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                    {
                        break; // Cut-off
                    }
                }
                return minEval;
            }
        }




        public Tuple<int, int, int> FindBestMove(int maxDepth)
        {
            int bestVal = int.MinValue;
            Tuple<int, int, int> bestMove = null;

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    for (int z = 0; z < gridSize; z++)
                    {
                        if (gameBoard[x, y, z] == CellState.Empty)
                        {
                            gameBoard[x, y, z] = CellState.AI;
                            int moveVal = Minimax(0, maxDepth, false, int.MinValue, int.MaxValue);
                            gameBoard[x, y, z] = CellState.Empty;

                            moveVal += EvaluateMove(x, y, z); // Dodanie heurystyki

                            if (moveVal > bestVal)
                            {
                                bestMove = new Tuple<int, int, int>(x, y, z);
                                bestVal = moveVal;

                                // Zatrzymaj, jeśli znajdziemy wygrywający ruch
                                if (bestVal == 1000)
                                {
                                    return bestMove;
                                }
                            }
                        }
                    }
                }
            }

            return bestMove;
        }


        private Tuple<int, int, int> IterativeDeepening(int maxDepth)
        {
            int bestVal = int.MinValue;
            Tuple<int, int, int> bestMove = null;

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    for (int z = 0; z < gridSize; z++)
                    {
                        if (gameBoard[x, y, z] == CellState.Empty)
                        {
                            gameBoard[x, y, z] = CellState.AI;
                            int moveVal = Minimax(0, maxDepth, false, int.MinValue, int.MaxValue);
                            gameBoard[x, y, z] = CellState.Empty;

                            moveVal += EvaluateMove(x, y, z); // Dodanie heurystyki

                            if (moveVal > bestVal)
                            {
                                bestMove = new Tuple<int, int, int>(x, y, z);
                                bestVal = moveVal;

                                // Zatrzymaj, jeśli znajdziemy wygrywający ruch
                                if (bestVal == 1000)
                                {
                                    return bestMove;
                                }
                            }
                        }
                    }
                }
            }

            return bestMove;
        }

        private CellState GetWinner()
        {
            if (IsWinner(CellState.AI))
                return CellState.AI;
            if (IsWinner(CellState.Opponent))
                return CellState.Opponent;
            return CellState.Empty;
        }

        public void MakeMove(int x, int y, int z, CellState player)
        {
            gameBoard[x, y, z] = player;
        }
    }
}
