using MazoriumWeb.Models;
using System;
using Xunit;

namespace MazoriumTests
{
    public class MazeTests
    {
        [Fact]
        public void DefaultWidth()
        {
            Maze testMaze = new Maze();
            Assert.True(testMaze.Width > 0);
        }

        [Fact]
        public void DefaultHeight()
        {
            Maze testMaze = new Maze();
            Assert.True(testMaze.Height > 0);
        }

        [Fact]
        public void ValidMatrix()
        {
            Maze testMaze = new Maze();
            Assert.NotNull(testMaze.Matrix);
        }

        [Theory]
        [InlineData(10, 10, 10)]
        [InlineData(1, 10, 1)]
        [InlineData(100, 10, 100)]
        [InlineData(99, 10, 99)]
        [InlineData(10, 1, 10)]
        [InlineData(10, 100, 10)]
        public void ConstructProperWidthMaze(int width, int height, int expectedWidth)
        {
            Maze testMaze = new Maze(width, height);
            Assert.Equal(expectedWidth, testMaze.Width);
        }

        [Theory]
        [InlineData(10, 10, 10)]
        [InlineData(10, 1, 1)]
        [InlineData(10, 100, 100)]
        [InlineData(10, 99, 99)]
        [InlineData(1, 10, 10)]
        [InlineData(100, 10, 10)]
        public void ConstructProperHeightMaze(int width, int height, int expectedHeight)
        {
            Maze testMaze = new Maze(width, height);
            Assert.Equal(expectedHeight, testMaze.Height);
        }

        [Theory]
        [InlineData(0, 10, 1)]
        [InlineData(101, 10, 100)]
        [InlineData(-1, 10, 1)]
        [InlineData(int.MaxValue, 10, 100)]
        [InlineData(int.MinValue, 10, 1)]
        public void HandleInvalidWidth(int width, int height, int expectedWidth)
        {
            Maze testMaze = new Maze(width, height);
            Assert.Equal(expectedWidth, testMaze.Width);
        }

        [Theory]
        [InlineData(10, 0, 1)]
        [InlineData(10, 101, 100)]
        [InlineData(10, -1, 1)]
        [InlineData(10, int.MaxValue, 100)]
        [InlineData(10, int.MinValue, 1)]
        public void HandleInvalidHeight(int width, int height, int expectedHeight)
        {
            Maze testMaze = new Maze(width, height);
            Assert.Equal(expectedHeight, testMaze.Height);
        }

        [Fact]
        public void DefaultMazeIsGenerated()
        {
            Maze testMaze = new Maze();
            testMaze.GenerateMaze();

            Assert.True(0 < testMaze.Matrix.Count && 0 < testMaze.Matrix[0].Count);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(100, 100)]
        [InlineData(99, 99)]
        [InlineData(1, 100)]
        [InlineData(100, 1)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        [InlineData(-1, 1)]
        [InlineData(1, -1)]
        [InlineData(int.MaxValue, 1)]
        [InlineData(int.MinValue, 1)]
        [InlineData(1, int.MaxValue)]
        [InlineData(1, int.MinValue)]
        public void MazeOfGivenSizeIsGenerated(int width, int height)
        {
            Maze testMaze = new Maze(width, height);
            testMaze.GenerateMaze();
            Assert.True(height == testMaze.Matrix.Count && width == testMaze.Matrix[height - 1].Count);
        }
    }
}
