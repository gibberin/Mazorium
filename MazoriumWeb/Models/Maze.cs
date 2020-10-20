using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MazoriumWeb.Models
{
    public class Maze
    {
        int _height = 100;
        int _width = 100;

        public int Height { get { return _height; } set { _height = value; GenerateMaze(); } }
        public int Width { get { return _width; } set { _width = value;  GenerateMaze(); } }

        public Cell Start { get; set; }
        public Cell End { get; set; }

        List<List<Cell>> matrix;

        public Maze(int x, int y)
        {
            _width = x;
            _height = y;

            Start = new Cell(1, 1);
            End = new Cell(Width, Height);
        }

        public int GenerateMaze()
        {
            return GenerateMaze(Start, End, Height, Width);
        }

        public int GenerateMaze(Cell start, Cell end, int height = 100, int width = 100)
        {
            _height = height;
            _width = width;
            Start = start;
            End = end;

            matrix = new List<List<Cell>>();

            List<Cell> unconnectedCells = new List<Cell>();

            // Populate maze matrix
            for(int row = 1; row <= _width; row++)
            {
                matrix.Add(new List<Cell>());

                for(int col = 1; col <= _height; col++)
                {
                    Cell newCell = new Cell(col, row);
                    if (1 < col)
                    {
                        newCell.AddNeighbor(GetAt(col - 1, row));
                    }

                    if (1 < row)
                    {
                        newCell.AddNeighbor(GetAt(col, row - 1));
                    }

                    matrix[row - 1].Add(newCell);
                    unconnectedCells.Add(newCell);
                }
            }

            Start = GetAt(Start.X, Start.Y);
            End = GetAt(End.X, End.Y);

            // Create paths
            Random rand = new Random((int)DateTime.Now.Ticks);

            //  Select random unconnected cell
            while (0 < unconnectedCells.Count)
            {
                Cell cell = unconnectedCells[rand.Next(unconnectedCells.Count)];
                Console.WriteLine("Starting new path at (" + cell.X + ", " + cell.Y + ")");

                // Extend path in a random direction until a connected cell is encountered
                bool extend = true;
                while(extend)
                {
                    // Pick a random neighbor
                    int unconnectedCount = cell.UnconnectedNeighbors().Count;
                    if(0 == unconnectedCount)
                    {
                        extend = false;
                        unconnectedCells.Remove(cell);
                        continue;
                    }
                    int randIdx = rand.Next(unconnectedCount);
                    Debug.Assert(randIdx < unconnectedCount);
                    Cell nextCell = cell.UnconnectedNeighbors()[randIdx];
                    extend = nextCell.IsUnconnected() && nextCell != Start && nextCell != End;
                    cell.ConnectTo(nextCell);
                    unconnectedCells.Remove(cell);
                    cell = nextCell;
                    Console.WriteLine("Connecting to (" + cell.X + ", " + cell.Y + ")");
                }

                Console.WriteLine("Halted at (" + cell.X + ", " + cell.Y + ") with " + cell.NumConnections() + " connections");
            }

            Console.WriteLine("All cells connected.");

            // TODO: Print maze

            return _height * _width;
        }

        /// <summary>
        /// Retrieve the cell at a given location
        /// </summary>
        /// <param name="col">The column of the desired cell (1 based)</param>
        /// <param name="row">The row of the desired cell (1 based)</param>
        /// <returns></returns>
        public Cell GetAt(int col, int row)
        {
            if(null == matrix)
            {
                return null;
            }

            Debug.Assert(0 < col && col <= Width);
            Debug.Assert(0 < row && row <= Height);

            return matrix[row - 1][col - 1];
        }
    }
}
