using BattleShips.Models.Interfaces;
using BattleShips.Models;

namespace BattleShips
{
    class Program
    {
        static void Main(string[] args)
        {
            IGrid grid = new Grid();
            Game game = new Game(grid);
            game.Start();
        }
    }
}

