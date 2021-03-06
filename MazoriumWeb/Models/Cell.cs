﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazoriumWeb.Models
{
    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Visited = false;
        public Cell pathParent;

        public List<Cell> Neighbors { get; }
        public List<Cell> Connections { get; }

        public Cell(int x = 0, int y = 0)
        {
            x = Math.Max(Math.Min(x, 100), 1);
            y = Math.Max(Math.Min(y, 100), 1);

            X = x;
            Y = y;

            Neighbors = new List<Cell>();
            Connections = new List<Cell>();
        }

        /// <summary>
        /// Add the given cell as a neighbor
        /// </summary>
        /// <param name="cell">The cell to add as a neighbor</param>
        /// <returns>The number of neighbors after addition</returns>
        public int AddNeighbor(Cell cell)
        {
            Neighbors.Add(cell);

            if(!cell.Neighbors.Contains(this))
            {
                cell.Neighbors.Add(this);
            }

            return Neighbors.Count;
        }

        /// <summary>
        /// Create a connection to given cell
        /// </summary>
        /// <param name="cell">The cell to connect to</param>
        /// <returns>The number of connected cells</returns>
        public int ConnectTo(Cell cell)
        {
            Connections.Add(cell);

            if(!cell.Connections.Contains(this))
            {
                cell.ConnectTo(this);
            }

            return Connections.Count;
        }

        /// <summary>
        /// Check if there is a connection to the given cell
        /// </summary>
        /// <param name="cell">The cell to check</param>
        /// <returns>True if connected, otherwise false</returns>
        public bool IsConnectedTo(Cell cell)
        {
            return Connections.Contains(cell);
        }

        /// <summary>
        /// Determines if this cell has any connections
        /// </summary>
        /// <returns>True if the cell has no connections</returns>
        public bool IsUnconnected()
        {
            return 0 == NumConnections();
        }

        /// <summary>
        /// Returns the number of connections this cell has
        /// </summary>
        /// <returns>The number of connections this cell has</returns>
        public int NumConnections()
        {
            return Connections.Count;
        }

        /// <summary>
        /// Check if the given cell is a neighbor
        /// </summary>
        /// <param name="cell">The cell to check</param>
        /// <returns>True if a neighbor, otherwise false</returns>
        public bool IsNeighborTo(Cell cell)
        {
            return Neighbors.Contains(cell);
        }

        /// <summary>
        /// Get a list of neighbors to which this cell is not connected
        /// </summary>
        /// <returns>A list of cells</returns>
        public List<Cell> UnconnectedNeighbors()
        {
            List<Cell> unconnectedNeighbors = new List<Cell>();

            foreach(Cell neighbor in Neighbors)
            {
                if(!Connections.Contains(neighbor))
                {
                    unconnectedNeighbors.Add(neighbor);
                }
            }

            return unconnectedNeighbors;
        }

        /// <summary>
        /// Return a JSON representation of this cell
        /// </summary>
        /// <returns>A JSON string</returns>
        public string ToJson()
        {
            StringBuilder json = new StringBuilder();

            json.AppendLine("    {");
            json.AppendLine($"      \"X\" : \"{X}\",");
            json.AppendLine($"      \"Y\" : \"{Y}\",");
            json.AppendLine("");

            // Enumerate neighbors
            json.AppendLine("      \"neighbors\" : [");
            foreach (Cell neighbor in Neighbors)
            {
                json.AppendLine("        {");
                json.AppendLine($"          \"X\" : \"{neighbor.X}\",");
                json.AppendLine($"          \"Y\" : \"{neighbor.Y}\",");
                json.AppendLine("        },");
            }
            json.AppendLine("      ],");
            json.AppendLine("");

            json.AppendLine("      \"connections\" : [");
            // Enumerate Connections
            foreach (Cell cnx in Connections)
            {
                json.AppendLine("        {");
                json.AppendLine($"          \"X\" : \"{cnx.X}\",");
                json.AppendLine($"          \"Y\" : \"{cnx.Y}\",");
                json.AppendLine("        },");

            }
            json.AppendLine("      ],");

            json.AppendLine("    },");

            return json.ToString();
        }
    }
}
