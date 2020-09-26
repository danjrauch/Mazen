using System;
using Xunit;

namespace Mazen.Tests
{
    public class CellUnit
    {
        [Fact]
        public void BidiLink()
        {
            Grid grid = new Grid(1, 2);
            grid[0,0].Link(Direction.East);
            Assert.True(grid[0,0].IsLinked(Direction.East));
            Assert.True(grid[0,1].IsLinked(Direction.West));
        }
    }
}
