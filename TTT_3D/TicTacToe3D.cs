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
                (1, 0, 0), (0, 1, 0), (0, 0, 1),
                (1, 1, 0), (-1, 1, 0),
                (1, 0, 1), (-1, 0, 1),
                (0, 1, 1), (0, -1, 1),
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

        public int EvaluateMove(int x, int y, int z)
        {
            int score = 0;

            gameBoard[x, y, z] = CellState.AI;
            if (IsWinner(CellState.AI))
            {
                score += 1000;
            }
            gameBoard[x, y, z] = CellState.Empty;

            gameBoard[x, y, z] = CellState.Opponent;
            if (IsWinner(CellState.Opponent))
            {
                score += 500;
            }
            gameBoard[x, y, z] = CellState.Empty;

            score += EvaluateBlockingMove(x, y, z, CellState.Opponent, 3) * 200;

            int center = gridSize / 2;
            if (x == center && y == center && z == center)
            {
                score += 50;
            }
            else if (x == center || y == center || z == center)
            {
                score += 20;
            }

            if (x == 0 || x == gridSize - 1 ||
                y == 0 || y == gridSize - 1 ||
                z == 0 || z == gridSize - 1)
            {
                score += 10;
            }

            score += CountPotentialLines(x, y, z, CellState.AI) * 10;

            return score;
        }

        private int EvaluateBlockingMove(int x, int y, int z, CellState player, int targetLength)
        {
            int count = 0;
            var directions = new List<(int, int, int)>
    {
        (1, 0, 0), (0, 1, 0), (0, 0, 1),
        (1, 1, 0), (-1, 1, 0),
        (1, 0, 1), (-1, 0, 1),
        (0, 1, 1), (0, -1, 1),
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
                (1, 0, 0), (0, 1, 0), (0, 0, 1),
                (1, 1, 0), (-1, 1, 0),
                (1, 0, 1), (-1, 0, 1),
                (0, 1, 1), (0, -1, 1),
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

        public Tuple<int, int, int> FindBestMove()
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
                            int moveVal = EvaluateMove(x, y, z);

                            if (moveVal > bestVal)
                            {
                                bestMove = new Tuple<int, int, int>(x, y, z);
                                bestVal = moveVal;
                            }
                        }
                    }
                }
            }

            return bestMove;
        }


        public void MakeMove(int x, int y, int z, CellState player)
        {
            gameBoard[x, y, z] = player;
        }
    }
}