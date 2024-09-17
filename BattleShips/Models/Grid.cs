using BattleShips.Models.Interfaces;
using System;

namespace BattleShips.Models
{
    public class Grid : IGrid
    {
        private const int GridSize = 10;
        private readonly char[,] grid;

        public Grid()
        {
            grid = new char[GridSize, GridSize];
            Initialize();
        }

        public void Initialize()
        {
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    grid[row, col] = '~';
                }
            }
        }

        // Places the ship on the grid in a valid position
        public bool PlaceShip(IShip ship)
        {
            Random random = new Random();
            bool placed = false;

            while (!placed)
            {
                int startRow = random.Next(0, GridSize);
                int startCol = random.Next(0, GridSize);
                bool horizontal = random.Next(2) == 0;

                if (IsValidPosition(ship, startRow, startCol, horizontal))
                {
                    PlaceShipOnGrid(ship, startRow, startCol, horizontal);
                    placed = true;
                }
            }

            return placed;
        }

        // Validates if the ship can be placed at the given position
        private bool IsValidPosition(IShip ship, int startRow, int startCol, bool horizontal)
        {
            int length = ship.Size;

            // Check if the ship will fit within the grid bounds
            if (horizontal)
            {
                if (startCol + length > GridSize)
                    return false;
            }
            else
            {
                if (startRow + length > GridSize)
                    return false;
            }

            // Check if the ship overlaps with any existing ships
            for (int i = 0; i < length; i++)
            {
                if (horizontal)
                {
                    if (grid[startRow, startCol + i] != ' ')
                        return false; // Overlaps with another ship
                }
                else
                {
                    if (grid[startRow + i, startCol] != ' ')
                        return false; // Overlaps with another ship
                }
            }

            return true; // Valid position
        }

        // Place the ship on the grid after validation
        private void PlaceShipOnGrid(IShip ship, int startRow, int startCol, bool horizontal)
        {
            int length = ship.Size;

            for (int i = 0; i < length; i++)
            {
                if (horizontal)
                {
                    grid[startRow, startCol + i] = ship.Symbol;
                }
                else
                {
                    grid[startRow + i, startCol] = ship.Symbol;
                }
            }
        }

        // Expose grid for testing purposes
        public char[,] GetGrid()
        {
            return grid;
        }

        public void Display()
        {
            Console.WriteLine("  1 2 3 4 5 6 7 8 9 10");
            for (int row = 0; row < GridSize; row++)
            {
                Console.Write((char)('A' + row) + " ");
                for (int col = 0; col < GridSize; col++)
                {
                    char displayChar = grid[row, col] == 'S' ? '~' : grid[row, col];
                    Console.Write(displayChar + " ");
                }
                Console.WriteLine();
            }
        }

        public void PlaceShip(IShip ship, int row, int col, bool isHorizontal)
        {
            for (int i = 0; i < ship.Size; i++)
            {
                if (isHorizontal)
                {
                    grid[row, col + i] = 'S';
                    ship.OccupiedCells.Add((row, col + i));
                }
                else
                {
                    grid[row + i, col] = 'S';
                    ship.OccupiedCells.Add((row + i, col));
                }
            }
        }

        public bool CanPlaceShip(IShip ship, int row, int col, bool isHorizontal)
        {
            if (isHorizontal)
            {
                if (col + ship.Size > GridSize) return false;

                for (int i = 0; i < ship.Size; i++)
                {
                    if (grid[row, col + i] == 'S') return false;
                }
            }
            else
            {
                if (row + ship.Size > GridSize) return false;

                for (int i = 0; i < ship.Size; i++)
                {
                    if (grid[row + i, col] == 'S') return false;
                }
            }

            return true;
        }

        public char GetCell(int row, int col) => grid[row, col];
        public void SetCell(int row, int col, char value) => grid[row, col] = value;
    }
}
