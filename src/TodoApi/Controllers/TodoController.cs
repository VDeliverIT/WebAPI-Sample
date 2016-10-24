using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class TodoController : Controller
    {
        public TodoController(ITodoRepository iTodoRepository)
        {
            TodoRepo = iTodoRepository;
        }
        public ITodoRepository TodoRepo { get; set; }

        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return TodoRepo.GetAll();
        }

        [HttpGet("{value}", Name = "GetTodo")]
        public IActionResult GetById(string value)
        {
            var item = TodoRepo.Find(value);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }


        [HttpGet ("{key}/{name}", Name = "GetTodoItemUsingTwoQueryParams")]
        public IActionResult GetByKeyAndName(string key,string name)
        {
            var item = TodoRepo.Find(key);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpGet("key={key}&name={name}", Name = "GetByKeyAndNameWithAmpersandSeparator")]
        public IActionResult GetByKeyAndNameWithAmpersandSeparator(string key, string name)
        {
            var item = TodoRepo.Find(key);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            TodoRepo.Add(item);
            return CreatedAtRoute("GetTodoItemUsingTwoQueryParams", new { key = item.Key,name=item.Name },item);
        }

        [HttpPut("{value}")]
        public IActionResult Update(string value, [FromBody] TodoItem item)
        {
            if (item == null || item.Key != value)
            {
                return BadRequest();
            }

            var todo = TodoRepo.Find(value);
            if (todo == null)
            {
                return NotFound();
            }

            TodoRepo.Update(item);
            return new NoContentResult();
        }

        [HttpPatch("{value}")]
        public IActionResult Update([FromBody] TodoItem item, string value)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var todo = TodoRepo.Find(value);
            if (todo == null)
            {
                return NotFound();
            }

            item.Key = todo.Key;

            TodoRepo.Update(item);
            return new NoContentResult();
        }

        [HttpDelete("{value}")]
        public IActionResult Delete(string value)
        {
            var todo = TodoRepo.Find(value);
            if (todo == null)
            {
                return NotFound();
            }

            TodoRepo.Remove(value);
            return new NoContentResult();
        }
    }
}