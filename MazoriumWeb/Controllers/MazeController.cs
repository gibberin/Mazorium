﻿using System;
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
        public ActionResult Display()
        {
            Maze maze = new Maze(10, 10);
            maze.GenerateMaze();
            return View(maze);
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