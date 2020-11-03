using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MazoriumWeb.Models
{
    public class MazeView
    {
        public Maze AMaze { get; set; }
        public List<Cell> DFSPath { get; set; }
        public List<Cell> BFSPath { get; set; }

        public MazeView(Maze maze)
        {
            AMaze = maze;
        }
    }
}
