using Microsoft.AspNetCore.Mvc;
using TodoLibrary.Models;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodosController : ControllerBase
{
   // GET: api/Todos
   [HttpGet]
   // Using ActionResult to be able to return the right status code
   // Its more flexible than just returning only IEnumerable<TodoDbModel>
   // which will return only ok status code no matter what.
   // We use ActionResult and not the interface IActionResult
   // becuase its allow us passing strongly typed object
   public ActionResult<IEnumerable<TodoDbModel>> Get()
   {
      throw new NotImplementedException();
   }

   // GET api/Todos/5
   [HttpGet("{id}")]
   public ActionResult<TodoDbModel> Get(int id)
   {
      throw new NotImplementedException();
   }

   // POST api/Todos
   [HttpPost]
   public IActionResult Post([FromBody] string value)
   {
      throw new NotImplementedException();
   }

   // PUT api/Todos/5
   [HttpPut("{id}")]
   public IActionResult Put(int id, [FromBody] string value)
   {
      throw new NotImplementedException();
   }

   // PUT api/Todos/5/Complete
   [HttpPut("{id}/Complete")]
   public IActionResult Complete(int id)
   {
      throw new NotImplementedException();
   }

   // DELETE api/Todos/5
   [HttpDelete("{id}")]
   public IActionResult Delete(int id)
   {
      throw new NotImplementedException();
   }
}
