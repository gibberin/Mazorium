using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazoriumWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MazoriumWeb
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapiController : ControllerBase
    {
        // GET: api/<MapiController>
        [HttpGet]
        public string Get()
        {
            Maze newMaze = new Maze();
            newMaze.GenerateMaze();

            // TODO: Build iterator for cells in maze grid

            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            json.AppendLine($"  'Width' : '{newMaze.Width}',");
            json.AppendLine($"  'Height' : '{newMaze.Height}',");
            json.AppendLine($"  'Start' : '{newMaze.Start.X}, {newMaze.Start.Y}',");
            json.AppendLine($"  'End' : '{newMaze.End.X}, {newMaze.End.Y}',");
            json.AppendLine($"  'Seed' : '{newMaze.Seed}',");

            // TODO: Convert each cell's info to json

            json.Append("}");

            return json.ToString();
        }

        // GET api/<MapiController>/5
        [HttpGet("{id}")]
        public string Get(int seed)
        {
            return "value";
        }

        // POST api/<MapiController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MapiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MapiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
