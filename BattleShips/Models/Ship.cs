using BattleShips.Models.Interfaces;

namespace BattleShips.Models
{
    public abstract class Ship : IShip
    {
        public string Name { get; }
        public int Size { get; }
        public bool IsSunk => Hits == Size;
        public List<(int, int)> OccupiedCells { get; }
        private int Hits;
        public char Symbol { get; }
        public Ship(string name, int size)
        {
            Name = name;
            Size = size;
            Hits = 0;
            OccupiedCells = new List<(int, int)>();
        }

        public void Hit(int row, int col)
        {
            if (OccupiedCells.Contains((row, col)))
            {
                Hits++;
            }
        }
    }
}
