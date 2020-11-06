using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazoriumWeb.Models
{
    public class MazeSpec
    {
        protected int _height = 10;
        protected int _width = 10;

        public int Height { get { return _height; } set { _height = value; } }
        public int Width { get { return _width; } set { _width = value; } }

        public Cell Start { get; set; }
        public Cell End { get; set; }

        public int Seed { get; set; }

        protected List<List<Cell>> _matrix;
        public List<List<Cell>> Matrix { get { return _matrix; } set { _matrix = value; } }

        public MazeSpec(Maze newMaze)
        {
            Height = newMaze.Height;
            Width = newMaze.Width;
            Start = newMaze.Start;
            End = newMaze.End;
            Seed = newMaze.Seed;
            Matrix = newMaze.Matrix;
        }
    }
}
