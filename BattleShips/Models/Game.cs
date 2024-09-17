using BattleShips.Models.Interfaces;
using BattleShips.Models;

namespace BattleShips.Models
{
    public class Game
    {
        private readonly IGrid _grid;
        private readonly List<IShip> _ships;
        private bool isGameOver;
        private bool isPlayerEndedGame;  // Flag to track if player manually ended the game

        public Game(IGrid grid)
        {
            _grid = grid;
            _ships = new List<IShip>();
            isGameOver = false;
            isPlayerEndedGame = false; // Initially, the game was not ended manually
        }

        public void Start()
        {
            _grid.Initialize();
            PlaceShipsRandomly();

            while (!isGameOver)
            {
                _grid.Display();
                GetPlayerInput();
                if (!isGameOver)  // Check again to avoid further checks if game ended
                {
                    CheckIfAllShipsSunk(); // Check if all ships have been sunk
                }
            }

            // Game over logic
            if (!isPlayerEndedGame)
            {
                Console.WriteLine("Game ended by the player.");
            }
            else
            {
                Console.WriteLine("Congratulations! You have sunk all the ships!");
            }
        }

        private void PlaceShipsRandomly()
        {
            Random rand = new Random();

            var battleship = new Battleship();
            PlaceShipRandomly(battleship, rand);
            _ships.Add(battleship);

            var destroyer1 = new Destroyer();
            PlaceShipRandomly(destroyer1, rand);
            _ships.Add(destroyer1);

            var destroyer2 = new Destroyer();
            PlaceShipRandomly(destroyer2, rand);
            _ships.Add(destroyer2);
        }

        private void PlaceShipRandomly(IShip ship, Random rand)
        {
            bool isPlaced = false;

            while (!isPlaced)
            {
                int row = rand.Next(0, 10);
                int col = rand.Next(0, 10);
                bool isHorizontal = rand.Next(0, 2) == 0;

                if (_grid.CanPlaceShip(ship, row, col, isHorizontal))
                {
                    _grid.PlaceShip(ship, row, col, isHorizontal);
                    isPlaced = true;
                }
            }
        }

        private void GetPlayerInput()
        {
            Console.Write("Enter coordinates (e.g., A5) or type 'END' to quit: ");
            string? input = Console.ReadLine()?.ToUpper();

            // If the player enters "END", quit the game
            if (input == "END")
            {
                isGameOver = true; // End the game loop
                return;
            }

            if (input.Length < 2 || input.Length > 3 || input[0] < 'A' || input[0] > 'J')
            {
                Console.WriteLine("Invalid input! Try again.");
                return;
            }

            // Parse row and column from user input
            int row = input[0] - 'A';
            if (!int.TryParse(input.Substring(1), out int col) || col < 1 || col > 10)
            {
                Console.WriteLine("Invalid input! Try again.");
                return;
            }

            col -= 1; // Adjust for 0-based index

            // Check if the user hit or missed a ship
            if (_grid.GetCell(row, col) == 'S')
            {
                Console.WriteLine("Hit!");
                _grid.SetCell(row, col, 'X');
                foreach (var ship in _ships)
                {
                    ship.Hit(row, col);
                    if (ship.IsSunk)
                    {
                        Console.WriteLine($"You sunk a {ship.Name}!");
                    }
                }
            }
            else if (_grid.GetCell(row, col) == 'X' || _grid.GetCell(row, col) == 'O')
            {
                Console.WriteLine("You've already targeted this spot! Try again.");
            }
            else
            {
                Console.WriteLine("Miss!");
                _grid.SetCell(row, col, 'O');
            }
        }

        private void CheckIfAllShipsSunk()
        {
            isGameOver = _ships.TrueForAll(ship => ship.IsSunk);
        }
    }
}
