namespace BattleShips.Models.Interfaces
{
   public interface IShip
    {
        string Name { get; }
        int Size { get; }
        bool IsSunk { get; }
        List<(int, int)> OccupiedCells { get; }
        char Symbol { get; }

        void Hit(int row, int col);
    }
}
