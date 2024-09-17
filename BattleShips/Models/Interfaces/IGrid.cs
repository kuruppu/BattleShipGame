using BattleShips.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShips.Models.Interfaces
{
    public interface IGrid
    {
        void Initialize();
        bool CanPlaceShip(IShip ship, int row, int col, bool isHorizontal);
        void PlaceShip(IShip ship, int row, int col, bool isHorizontal);
        void Display();
        char GetCell(int row, int col);
        void SetCell(int row, int col, char value);
    }
}
