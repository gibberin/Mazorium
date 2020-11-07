using MazoriumWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MazoriumTests
{
    public class CellTests
    {
        [Fact]
        public void DefaultCell()
        {
            Cell testCell = new Cell();
            Assert.NotNull(testCell);
            Assert.NotNull(testCell.Neighbors);
            Assert.NotNull(testCell.Connections);
        }

        [Fact]
        public void NeighborIsAdded()
        {
            Cell testCell = new Cell(1, 1);
            Cell neighborCell = new Cell(2, 1);
            testCell.AddNeighbor(neighborCell);
            Assert.True(testCell.IsNeighborTo(neighborCell));
            Assert.True(neighborCell.IsNeighborTo(testCell));
        }

        [Fact]
        public void IsUnconnected()
        {
            Cell testCell = new Cell();
            Assert.True(testCell.IsUnconnected());
        }

        [Fact]
        public void NumConnectionsIsCorrect()
        {
            Cell testCell = new Cell(2, 2);
            Cell cell2 = new Cell(2, 1);
            Cell cell3 = new Cell(2, 3);
            Cell cell4 = new Cell(1, 2);
            Cell cell5 = new Cell(3, 2);

            testCell.ConnectTo(cell2);
            testCell.ConnectTo(cell3);
            testCell.ConnectTo(cell4);
            testCell.ConnectTo(cell5);

            Assert.True(4 == testCell.NumConnections());
        }

        [Fact]
        public void ConnectionIsAdded()
        {
            Cell testCell = new Cell(1, 1);
            Cell cnxCell = new Cell(2, 1);
            testCell.ConnectTo(cnxCell);
            Assert.False(testCell.IsUnconnected());
            Assert.False(cnxCell.IsUnconnected());
            Assert.True(testCell.IsConnectedTo(cnxCell));
            Assert.True(cnxCell.IsConnectedTo(testCell));
        }

        [Theory]
        [InlineData(1, 1, 1, 1)]
        [InlineData(100, 100, 100, 100)]
        [InlineData(101, 100, 100, 100)]
        [InlineData(100, 101, 100, 100)]
        [InlineData(99, 99, 99, 99)]
        [InlineData(1, 100, 1, 100)]
        [InlineData(100, 1, 100, 1)]
        [InlineData(0, 1, 1, 1)]
        [InlineData(1, 0, 1, 1)]
        [InlineData(-1, 1, 1, 1)]
        [InlineData(1, -1, 1, 1)]
        [InlineData(int.MaxValue, 1, 100, 1)]
        [InlineData(int.MinValue, 1, 1, 1)]
        [InlineData(1, int.MaxValue, 1, 100)]
        [InlineData(1, int.MinValue, 1, 1)]
        public void CellLocationSetCorrectly(int x, int y, int expectedX, int expectedY)
        {
            Cell testCell = new Cell(x, y);
            Assert.True(expectedX == testCell.X && expectedY == testCell.Y);
        }
    }
}
