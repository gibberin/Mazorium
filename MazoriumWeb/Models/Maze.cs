using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazoriumWeb.Models
{
    public class Maze : IEnumerable<Cell>
    {
        protected int _height = 10;
        protected int _width = 10;

        public int Height { get { return _height; } set { _height = value; GenerateMaze(); } }
        public int Width { get { return _width; } set { _width = value;  GenerateMaze(); } }

        public Cell Start { get; set; }
        public Cell End { get; set; }

        public int Seed { get; set; }

        protected List<List<Cell>> _matrix;
        public List<List<Cell>> Matrix { get { return _matrix; } }

        public Maze(int x = 10, int y = 10, int seed = 0)
        {
            x = Math.Max(Math.Min(x, 100), 1);
            y = Math.Max(Math.Min(y, 100), 1);

            _width = x;
            _height = y;

            _matrix = new List<List<Cell>>();

            Start = new Cell(1, 1);
            End = new Cell(Width, Height);

            Seed = seed;
        }

        /// <summary>
        /// Generate maze with existing parameters
        /// </summary>
        /// <returns>The total number of cells in the maze</returns>
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
        public int GenerateMaze(Cell start, Cell end, int height = 10, int width = 10, int seed = 0)
        {
            _height = height;
            _width = width;

            _matrix.Clear();

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
                _matrix.Add(new List<Cell>());

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

                    _matrix[row - 1].Add(newCell);
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

            if(1 == Width * Height)
            {
                return 1;
            }

            // Create paths
            int pathCount = 0;
            Seed = (0 == Seed) ? (int)DateTime.Now.Ticks : Seed;
            Random rand = new Random(Seed);
            List<Cell> currPath = new List<Cell>();
            List<Cell> validNextCells = new List<Cell>();

            Cell cell = End;

            //  Select random unconnected cell
            while (0 < unconnectedCells.Count)
            {
                currPath.Clear();
                pathCount++;
                currPath.Add(cell);

                // Extend path in a random direction until a previously connected cell is encountered
                bool extend = true;
                while (extend)
                {
                    validNextCells = GetValidNextCells(ref currPath, cell);

                    // Pick a random neighbor
                    int numNextCells = validNextCells.Count;
                    if (0 == numNextCells)
                    {
                        //Console.WriteLine("Dead end encountered at (" + cell.X + ", " + cell.Y + ")");
                        unconnectedCells.Remove(cell);

                        List<Cell> cellPool = new List<Cell>(currPath);

                        // Resolve deadend paths by picking a random cell in the current path and extending the path in a different direction
                        while (0 == validNextCells.Count())
                        {
                            cell = cellPool[rand.Next(cellPool.Count())];
                            cellPool.Remove(cell);
                            //Console.WriteLine("Testing random cell on path (" + cell.X + ", " + cell.Y + ")");
                            validNextCells = GetValidNextCells(ref currPath, cell);
                            if(0 == cellPool.Count())
                            {
                                Debug.Assert(0 == unconnectedCells.Count()); // We should only get here if there are no unconnected cells left to use to break out of the dead end
                                return pathCount;
                            }
                        }

                        //Console.WriteLine("Selected (" + cell.X + ", " + cell.Y + ")");
                        numNextCells = validNextCells.Count();
                    }

                    Cell nextCell = validNextCells[rand.Next(numNextCells)];
                    //Console.Write("Adding (" + nextCell.X + ", " + nextCell.Y + ")");
                    extend = nextCell.IsUnconnected();
                    //Console.WriteLine(extend ? " - extending" : " ");
                    cell.ConnectTo(nextCell);
                    unconnectedCells.Remove(cell);
                    cell = nextCell;
                    currPath.Add(cell);
                }

                // Pick the next starting cell at random
                if (0 < unconnectedCells.Count)
                {
                    cell = unconnectedCells[rand.Next(unconnectedCells.Count)];
                    //Console.WriteLine("New start at (" + cell.X + ", " + cell.Y + ")");
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
            if(null == _matrix || 0 == _matrix.Count())
            {
                Debug.WriteLine("Attempt to retrieve cells from non-existant matrix");
                return null;
            }

            // Test requested cell exists
            if((1 > row || row > _matrix.Count()) || (1 > col || col > _matrix[0].Count()))
            {
                Debug.WriteLine("Requested cell (" + col + ", " + row + ") is outside matrix boundaries " + _matrix[0].Count() + "x" + _matrix.Count());
                return null;
            }

            return _matrix[row - 1][col - 1];
        }

        /// <summary>
        /// Provide cell enumerator for the maze grid
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Cell> GetEnumerator()
        {
            foreach(List<Cell> row in _matrix)
            {
                foreach(Cell cell in row)
                {
                    yield return cell;
                }
            }
        }

        // The IEnumerable.GetEnumerator method is also required
        // because IEnumerable<T> derives from IEnumerable.
        System.Collections.IEnumerator
          System.Collections.IEnumerable.GetEnumerator()
        {
            // Invoke IEnumerator<string> GetEnumerator() above.
            return GetEnumerator();
        }

        /// <summary>
        /// Recursive depth first search (DFS) for the path between two given cells
        /// </summary>
        /// <param name="first">The start cell</param>
        /// <param name="last">The end cell</param>
        /// <returns>A list of cells defining the path to the destination or null if there is no path</returns>
        public List<Cell> GetOptimalPathDFS(List<Cell>visited, Cell first, Cell last)
        {
            List<Cell> path = new List<Cell>();
            Queue<Cell> nextCells = new Queue<Cell>();

            if(first == last)
            {
                path.Add(first);
                return path;
            }

            if(1 == last.Connections.Count && last != End && last != Start)
            {
                return null;
            }

            visited.Add(last);

            // Seek a path between each connected cell and the destination (start cell)
            foreach(Cell next in last.Connections)
            {
                // Don't check the cell we are starting from
                if(visited.Contains(next))
                {
                    continue;
                }

                path = GetOptimalPathDFS(visited, first, next);

                if(null != path)
                {
                    path.Insert(0, last);
                    return path;
                }
            }

            // No path was found
            return null;
        }

        /// <summary>
        /// Breadth first search (BFS) for the path between two given cells
        /// </summary>
        /// <param name="first">The start cell</param>
        /// <param name="last">The end cell</param>
        /// <returns>A list of cells defining the path to the destination or null if there is no path</returns>
        public List<Cell> GetOptimalPathBFS(Cell first, Cell last)
        {
            List<Cell> path = new List<Cell>();
            HashSet<Cell> visited = new HashSet<Cell>();

            // Queue the start cell
            Queue<Cell> nextCells = new Queue<Cell>();
            first.pathParent = null;
            nextCells.Enqueue(first);

            // Check cells in the queue
            Cell currCell;
            while(0 < nextCells.Count)
            {
                // Get the next cell in the queue, mark it as visited
                currCell = nextCells.Dequeue();
                visited.Add(currCell);

                // If this is the destination cell build the final path
                if(currCell == last)
                {
                    // Walk backward from this cell and build the path
                    path.Insert(0, currCell);
                    while (null != currCell.pathParent)
                    {
                        path.Add(currCell.pathParent);
                        currCell = currCell.pathParent;
                    }

                    return path;
                }
                else
                {
                    // Queue this cell's unvisited neighbors
                    foreach (Cell cnx in currCell.Connections)
                    {
                        if (!visited.Contains(cnx))
                        {
                            cnx.pathParent = currCell;
                            nextCells.Enqueue(cnx);
                        }
                    }
                }
            }

            // No path was found
            return null;
        }

        /// <summary>
        /// Returns a JSON representation of the maze info
        /// </summary>
        /// <returns>A JSON string</returns>
        public string ToJson()
        {
            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            json.AppendLine($"  \"width\" : \"{Width}\",");
            json.AppendLine($"  \"height\" : \"{Height}\",");
            json.AppendLine("");
            json.AppendLine("  \"start\" : [");
            json.AppendLine("      {");
            json.AppendLine($"        \"X\" : \"{Start.X}\",");
            json.AppendLine($"        \"Y\" : \"{Start.Y}\",");
            json.AppendLine("      {");
            json.AppendLine("    ],");
            json.AppendLine("  \"end\" : [");
            json.AppendLine("      {");
            json.AppendLine($"        \"X\" : \"{End.X}\",");
            json.AppendLine($"        \"Y\" : \"{End.Y}\",");
            json.AppendLine("      {");
            json.AppendLine("    ],");
            json.AppendLine("");
            json.AppendLine($"  \"seed\" : \"{Seed}\",");
            json.AppendLine("");

            json.AppendLine("  \"cell\" : [");
            // Convert each cell's info to json
            foreach (Cell cell in this)
            {
                json.AppendLine("");
                json.Append(cell.ToJson());
            }

            json.AppendLine("  ]");
            json.Append("}");

            return json.ToString();
        }
    }
}
