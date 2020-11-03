using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using MazoriumWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MazoriumWeb.Controllers
{
    public class MazeController : Controller
    {
        // GET: MazeController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MazeController
        public ActionResult AMaze(int width = 10, int height = 10)
        {
            Maze maze = new Maze(width, height);
            MazeView mazeView = new MazeView(maze);

            // Create the actual maze
            maze.GenerateMaze();

            // Determine solution paths using DFS and BFS algorithms
            List<Cell> visited = new List<Cell>();
            mazeView.DFSPath = maze.GetOptimalPathDFS(visited, maze.Start, maze.End);
            if(null == mazeView.DFSPath)
            {
                Console.WriteLine("No valid DFS path identified.");
            }
            else
            {
                Console.WriteLine("Valid DFS path identified with " + mazeView.DFSPath.Count + " steps");
            }

            mazeView.BFSPath = maze.GetOptimalPathBFS(maze.Start, maze.End);
            if (null == mazeView.BFSPath)
            {
                Console.WriteLine("No valid BFS path identified.");
            }
            else
            {
                Console.WriteLine("Valid BFS path identified with " + mazeView.BFSPath.Count + " steps");
            }

            return View(mazeView);
        }

        // GET: MazeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MazeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MazeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MazeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MazeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MazeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MazeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
