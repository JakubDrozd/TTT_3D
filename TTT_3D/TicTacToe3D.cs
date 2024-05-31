using System;
using System.Collections.Generic;

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
        private const int MaxDepth = 5; // Możesz eksperymentować z tą wartością


        public CellState[,,] Board => gameBoard;

        public TicTacToe3D(int gridSize)
        {
            this.gridSize = gridSize;
            inARowToWin = gridSize >= MaxInARowToWin ? MaxInARowToWin : gridSize;
            gameBoard = new CellState[gridSize, gridSize, gridSize];

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



        private int EvaluateBlockingMove(int x, int y, int z, CellState player, int targetLength)
        {
            int count = 0;
            var directions = new List<(int, int, int)>
    {
        // Straight lines
        (1, 0, 0), (0, 1, 0), (0, 0, 1),
        // Diagonals on XY plane
        (1, 1, 0), (-1, 1, 0),
        // Diagonals on XZ plane
        (1, 0, 1), (-1, 0, 1),
        // Diagonals on YZ plane
        (0, 1, 1), (0, -1, 1),
        // 3D Diagonals
        (1, 1, 1), (-1, 1, 1), (1, -1, 1), (-1, -1, 1),
        (1, 1, -1), (-1, 1, -1), (1, -1, -1), (-1, -1, -1)
    };

            foreach (var dir in directions)
            {
                int lineCount = 1;
                for (int i = 1; i < targetLength; i++)
                {
                    int newX = x + i * dir.Item1;
                    int newY = y + i * dir.Item2;
                    int newZ = z + i * dir.Item3;

                    if (newX < 0 || newX >= gridSize || newY < 0 || newY >= gridSize || newZ < 0 || newZ >= gridSize)
                        break;

                    if (gameBoard[newX, newY, newZ] == player)
                        lineCount++;
                    else if (gameBoard[newX, newY, newZ] == CellState.Empty)
                        continue;
                    else
                        break;
                }

                if (lineCount == targetLength)
                    count++;
            }

            return count;
        }




        private int CountPotentialLines(int x, int y, int z, CellState player)
        {
            int count = 0;
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
                int lineCount = 1;
                for (int i = 1; i < inARowToWin; i++)
                {
                    int newX = x + i * dir.Item1;
                    int newY = y + i * dir.Item2;
                    int newZ = z + i * dir.Item3;

                    if (newX < 0 || newX >= gridSize || newY < 0 || newY >= gridSize || newZ < 0 || newZ >= gridSize)
                        break;

                    if (gameBoard[newX, newY, newZ] == player)
                        lineCount++;
                    else if (gameBoard[newX, newY, newZ] == CellState.Empty)
                        continue;
                    else
                        break;
                }

                if (lineCount > 1)
                    count++;
            }

            return count;
        }


        private CellState? GetWinner()
        {
            if (GetWinningCoordinates(CellState.AI) != null)
                return CellState.AI;
            if (GetWinningCoordinates(CellState.Opponent) != null)
                return CellState.Opponent;
            return null;
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

            var moves = GetPossibleMoves(isMaximizing);

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


        private List<Tuple<int, int, int>> GetPossibleMoves(bool isMaximizing)
        {
            var moves = new List<Tuple<int, int, int>>();

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    for (int z = 0; z < gridSize; z++)
                    {
                        if (gameBoard[x, y, z] == CellState.Empty)
                        {
                            moves.Add(new Tuple<int, int, int>(x, y, z));
                        }
                    }
                }
            }

            // Sortowanie ruchów według heurystyki
            if (isMaximizing)
            {
                moves.Sort((a, b) => EvaluateMove(b.Item1, b.Item2, b.Item3).CompareTo(EvaluateMove(a.Item1, a.Item2, a.Item3)));
            }
            else
            {
                moves.Sort((a, b) => EvaluateMove(a.Item1, a.Item2, a.Item3).CompareTo(EvaluateMove(b.Item1, b.Item2, b.Item3)));
            }

            return moves;
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

            return score;
        }




        public Tuple<int, int, int> FindBestMove(int maxDepth)
        {
            int bestVal = int.MinValue;
            Tuple<int, int, int> bestMove = null;

            var moves = GetPossibleMoves(true);

            foreach (var move in moves)
            {
                gameBoard[move.Item1, move.Item2, move.Item3] = CellState.AI;
                int moveVal = Minimax(0, maxDepth, false, int.MinValue, int.MaxValue);
                gameBoard[move.Item1, move.Item2, move.Item3] = CellState.Empty;

                Console.WriteLine($"Move evaluated: ({move.Item1}, {move.Item2}, {move.Item3}) with value {moveVal}"); // Debug

                if (moveVal > bestVal)
                {
                    bestMove = move;
                    bestVal = moveVal;

                    // Zatrzymaj, jeśli znajdziemy wygrywający ruch
                    if (bestVal == 1000)
                    {
                        break;
                    }
                }
            }

            if (bestMove != null)
            {
                Console.WriteLine($"Best move: ({bestMove.Item1}, {bestMove.Item2}, {bestMove.Item3}) with value {bestVal}"); // Debug
            }

            return bestMove;
        }



        public void MakeMove(int x, int y, int z, CellState player)
        {
            gameBoard[x, y, z] = player;
        }
    }
}