using Moq;
using BattleShips.Models;
using BattleShips.Models.Interfaces;

namespace BattleShips.Tests
{
    public class GameTests
    {
        [Fact]
        public void PlayerEndsGame_InputEnd_GameOverIsTrue()
        {
            // Arrange
            var mockGrid = new Mock<IGrid>();
            var game = new Game(mockGrid.Object);

            // Act
            // Simulate player typing "END" by invoking the GetPlayerInput method with "END"
            var privateMethod = typeof(Game).GetMethod("GetPlayerInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Console.SetIn(new System.IO.StringReader("END\n"));
            privateMethod.Invoke(game, null);

            // Assert
            // Use reflection to check if 'isGameOver' is true
            var isGameOverField = typeof(Game).GetField("isGameOver", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            bool isGameOver = (bool)isGameOverField.GetValue(game);
            Assert.True(isGameOver);
        }

        [Fact]
        public void AllShipsSunk_GameOverIsTrue()
        {
            // Arrange
            var mockGrid = new Mock<IGrid>();
            var mockShip = new Mock<IShip>();

            // Mock a scenario where all ships are sunk
            mockShip.Setup(s => s.IsSunk).Returns(true); // Pretend ship is sunk

            var game = new Game(mockGrid.Object);
            var ships = new List<IShip> { mockShip.Object };
            var privateShipsField = typeof(Game).GetField("_ships", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            privateShipsField.SetValue(game, ships);

            // Act
            var privateMethod = typeof(Game).GetMethod("CheckIfAllShipsSunk", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            privateMethod.Invoke(game, null);

            // Assert
            var isGameOverField = typeof(Game).GetField("isGameOver", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            bool isGameOver = (bool)isGameOverField.GetValue(game);
            Assert.True(isGameOver);
        }

        [Fact]
        public void InvalidCoordinateInput_DoesNotAffectGameState()
        {
            // Arrange
            var mockGrid = new Mock<IGrid>();
            var game = new Game(mockGrid.Object);

            // Act
            // Simulate invalid input, such as "Z99"
            var privateMethod = typeof(Game).GetMethod("GetPlayerInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Console.SetIn(new System.IO.StringReader("Z99\n"));
            privateMethod.Invoke(game, null);

            // Assert
            var isGameOverField = typeof(Game).GetField("isGameOver", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            bool isGameOver = (bool)isGameOverField.GetValue(game);
            Assert.False(isGameOver); // Invalid input should not end the game
        }

        [Fact]
        public void PlayerHitsShip_CorrectCellUpdatedToX()
        {
            // Arrange
            var mockGrid = new Mock<IGrid>();
            var mockShip = new Mock<IShip>();

            // Setup a ship on the grid at a specific location
            mockGrid.Setup(g => g.GetCell(0, 0)).Returns('S'); // 'S' for ship

            // Setup ship to record a hit and be updated
            mockShip.Setup(s => s.Hit(0, 0)).Callback(() => mockShip.Setup(s => s.IsSunk).Returns(false));

            var game = new Game(mockGrid.Object);
            var ships = new List<IShip> { mockShip.Object };
            var privateShipsField = typeof(Game).GetField("_ships", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            privateShipsField.SetValue(game, ships);

            // Act
            var privateMethod = typeof(Game).GetMethod("GetPlayerInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Console.SetIn(new System.IO.StringReader("A1\n"));
            privateMethod.Invoke(game, null);

            // Assert
            mockGrid.Verify(g => g.SetCell(0, 0, 'X'), Times.Once); // Ensure cell (0,0) was updated to 'X' after a hit
        }

        [Fact]
        public void PlayerMissesShip_CorrectCellUpdatedToO()
        {
            // Arrange
            var mockGrid = new Mock<IGrid>();

            // Setup grid to return empty water on A1
            mockGrid.Setup(g => g.GetCell(0, 0)).Returns(' '); // Empty water

            var game = new Game(mockGrid.Object);

            // Act
            var privateMethod = typeof(Game).GetMethod("GetPlayerInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Console.SetIn(new System.IO.StringReader("A1\n"));
            privateMethod.Invoke(game, null);

            // Assert
            mockGrid.Verify(g => g.SetCell(0, 0, 'O'), Times.Once); // Ensure cell (0,0) was updated to 'O' after a miss
        }
    }
}

