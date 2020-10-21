using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MazoriumWeb.Models
{
    public class Maze
    {
        int _height = 10;
        int _width = 10;

        public int Height { get { return _height; } set { _height = value; GenerateMaze(); } }
        public int Width { get { return _width; } set { _width = value;  GenerateMaze(); } }

        public Cell Start { get; set; }
        public Cell End { get; set; }

        List<List<Cell>> matrix;

        public Maze(int x, int y)
        {
            _width = x;
            _height = y;

            matrix = new List<List<Cell>>();

            Start = new Cell(1, 1);
            End = new Cell(Width, Height);
        }

        /// <summary>
        /// Generate maze with existing parameters
        /// </summary>
        /// <returns></returns>
        public int GenerateMaze()
        {
            return GenerateMaze(Start, End, Height, Width);
        }

        /// <summary>
        /// Generate maze with specific parameters
        /// </summary>
        /// <param name="start">A cell object containing the location of the start of the maze in the matrix</param>
        /// <param name="end">A cell object containing the location of the end of the maze in the matrix</param>
        /// <param name="height">The number of rows in the maze</param>
        /// <param name="width">The number of columns in the maze</param>
        /// <returns>The total number of cells in the maze</returns>
        public int GenerateMaze(Cell start, Cell end, int height = 10, int width = 10)
        {
            _height = height;
            _width = width;

            matrix.Clear();

            List<Cell> unconnectedCells = new List<Cell>();

            // Populate maze matrix with unconnected cells
            PopulateMatrix(ref unconnectedCells, height, width);

            Start = GetAt(start.X, start.Y);
            End = GetAt(end.X, end.Y);

            // Create paths
            int numPaths = GeneratePaths(ref unconnectedCells);
            Debug.Assert(0 < numPaths);
            Debug.WriteLine("Generated " + numPaths + " paths with an average length of " + (height * width) / numPaths + " cells.");

            return _height * _width;
        }

        /// <summary>
        /// Fills the maze matrix with unconnected cells
        /// </summary>
        /// <param name="unconnectedCells">A list to hold all the new unconnected cells added to the maze</param>
        /// <param name="height">The number of rows in the maze</param>
        /// <param name="width">The number of columns in the maze</param>
        /// <returns>The total number of cells added</returns>
        int PopulateMatrix(ref List<Cell> unconnectedCells, int height, int width)
        {
            Debug.Assert(null != unconnectedCells);
            Debug.Assert(0 < height);
            Debug.Assert(0 < width);

            for (int row = 1; row <= Height; row++)
            {
                matrix.Add(new List<Cell>());

                for (int col = 1; col <= Width; col++)
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

            return width * height;
        }

        /// <summary>
        /// Generates paths within the maze matrix
        /// </summary>
        /// <param name="unconnectedCells">A list of all the unconnected cells in the matrix</param>
        /// <returns>The number of paths generated</returns>
        int GeneratePaths(ref List<Cell> unconnectedCells)
        {
            Debug.Assert(null != unconnectedCells);
            if (0 == unconnectedCells.Count())
            {
                Debug.WriteLine("No unconnected cells provided to generate new paths.");
                return 0;
            }

            // Create paths
            int pathCount = 0;
            Random rand = new Random((int)DateTime.Now.Ticks);
            List<Cell> currPath = new List<Cell>();
            List<Cell> validNextCells = new List<Cell>();

            //  Select random unconnected cell
            while (0 < unconnectedCells.Count)
            {
                currPath.Clear();
                pathCount++;

                Cell cell = unconnectedCells[rand.Next(unconnectedCells.Count)];

                // Extend path in a random direction until a connected cell is encountered
                bool extend = true;
                while (extend)
                {
                    validNextCells = GetValidNextCells(ref currPath, cell);

                    // Pick a random neighbor
                    int numNextCells = validNextCells.Count;
                    if (0 == numNextCells)
                    {
                        unconnectedCells.Remove(cell);

                        // Resolve cyclical paths by picking a random cell in the current path and extending the path in a different direction
                        while (0 == validNextCells.Count())
                        {
                            cell = currPath[rand.Next(currPath.Count())];
                            validNextCells = GetValidNextCells(ref currPath, cell);
                        }

                        numNextCells = validNextCells.Count();
                    }

                    Cell nextCell = validNextCells[rand.Next(numNextCells)];
                    extend = nextCell.IsUnconnected();
                    cell.ConnectTo(nextCell);
                    unconnectedCells.Remove(cell);
                    cell = nextCell;
                    currPath.Add(cell);
                }
            }

            return pathCount;
        }

        /// <summary>
        /// Gets a list of neighboring cells to which a connection can be made (i.e. are not already in the current path)
        /// </summary>
        /// <param name="currPath">The list of cells in the current path</param>
        /// <param name="cell">The cell to start from</param>
        /// <returns>A list of cells to which a connection would be valid</returns>
        List<Cell> GetValidNextCells(ref List<Cell> currPath, Cell cell)
        {
            List<Cell> validCells = new List<Cell>();

            foreach (Cell uConnNeighbor in cell.UnconnectedNeighbors())
            {
                if (!currPath.Contains(uConnNeighbor))
                {
                    validCells.Add(uConnNeighbor);
                }
            }

            return validCells;
        }

        /// <summary>
        /// Retrieve the cell at a given location
        /// </summary>
        /// <param name="col">The column of the desired cell (1 based)</param>
        /// <param name="row">The row of the desired cell (1 based)</param>
        /// <returns></returns>
        public Cell GetAt(int col, int row)
        {
            if(null == matrix || 0 == matrix.Count())
            {
                Debug.WriteLine("Attempt to retrieve cells from non-existant matrix");
                return null;
            }

            // Test requested cell exists
            if((1 > row || row > matrix.Count()) || (1 > col || col > matrix[0].Count()))
            {
                Debug.WriteLine("Requested cell (" + col + ", " + row + ") is outside matrix boundaries " + matrix[0].Count() + "x" + matrix.Count());
                return null;
            }

            return matrix[row - 1][col - 1];
        }
    }
}
