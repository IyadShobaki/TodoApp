using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoLibrary.DataAccess;
using TodoLibrary.Models;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodosController : ControllerBase
{
   private readonly ITodoData _data;
   public TodosController(ITodoData data)
   {
      _data = data;
   }

   private int GetUserId()
   {
      // Built in functionality (Part of ASP.NET Core) to get the claim "Sub == NameIdentifier".
      // We tried to put it in ctor because each user will create a new instance of this class
      // when using an endpoint (Transient). But at the time of creating a new intance Claims/Token
      // Will not be create it yet and the value will be null
      var userIdText = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
      return int.Parse(userIdText);
   }
   // GET: api/Todos
   [HttpGet]
   // Using ActionResult to be able to return the right status code
   // Its more flexible than just returning only IEnumerable<TodoDbModel>
   // which will return only ok status code no matter what.
   // We use ActionResult and not the interface IActionResult
   // becuase its allow us passing strongly typed object
   public async Task<ActionResult<List<TodoDbModel>>> Get()
   {

      var output = await _data.GetAllAssigned(GetUserId());

      return Ok(output);
   }

   // GET api/Todos/5
   [HttpGet("{todoId}")]
   public async Task<ActionResult<TodoDbModel>> Get(int todoId)
   {
      var output = await _data.GetOneAssigned(GetUserId(), todoId);

      return Ok(output);
   }

   // POST api/Todos
   [HttpPost]
   public async Task<ActionResult<TodoDbModel>> Post([FromBody] string task)
   {
      var output = await _data.Create(GetUserId(), task);
      return Ok(output);

   }

   // PUT api/Todos/5
   [HttpPut("{todoId}")]
   public async Task<IActionResult> Put(int todoId, [FromBody] string task)
   {
      await _data.UpdateTask(GetUserId(), todoId, task);

      return Ok();
   }

   // PUT api/Todos/5/Complete
   [HttpPut("{todoId}/Complete")]
   public async Task<IActionResult> Complete(int todoId)
   {
      await _data.CompleteTodo(GetUserId(), todoId);

      return Ok();
   }

   // DELETE api/Todos/5
   [HttpDelete("{todoId}")]
   public async Task<IActionResult> Delete(int todoId)
   {
      await _data.Delete(GetUserId(), todoId);

      return Ok();
   }
}
