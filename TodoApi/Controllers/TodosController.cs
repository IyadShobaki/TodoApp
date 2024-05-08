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
   private readonly ILogger<TodosController> _logger;

   public TodosController(ITodoData data, ILogger<TodosController> logger)
   {
      _data = data;
      _logger = logger;
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
      _logger.LogInformation("GET: api/Todos");

      try
      {
         var output = await _data.GetAllAssigned(GetUserId());
         return Ok(output);
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "The GET call to api/Todos failed.");
         return BadRequest();
      }


   }

   // GET api/Todos/5
   [HttpGet("{todoId}")]
   public async Task<ActionResult<TodoDbModel>> Get(int todoId)
   {
      _logger.LogInformation("GET: api/Todos/{TodoId}", todoId);
      try
      {
         var output = await _data.GetOneAssigned(GetUserId(), todoId);

         return Ok(output);
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "The GET call to {ApiPath} failed. The Id was {TodoId}",
                     "api/Todos/TodoId", todoId);
         // we can use string interoplation $ with args but not with the message 
         //_logger.LogError(ex, "The GET call to {ApiPath} failed. The Id was {TodoId}",
         //            $"api/Todos/{TodoId}", todoId);

         return BadRequest();
      }
   }

   // POST api/Todos
   [HttpPost]
   public async Task<ActionResult<TodoDbModel>> Post([FromBody] string task)
   {


      _logger.LogInformation("POST: api/Todos (Task: {Task})", task);

      try
      {
         var output = await _data.Create(GetUserId(), task);
         return Ok(output);
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "The POST call to api/Todos failed. Task value was {Task}", task);
         return BadRequest();
      }

   }

   // PUT api/Todos/5
   [HttpPut("{todoId}")]
   public async Task<IActionResult> Put(int todoId, [FromBody] string task)
   {
      _logger.LogInformation("PUT: api/Todos/{TodoId} (Task: {Task})", todoId, task);

      try
      {
         await _data.UpdateTask(GetUserId(), todoId, task);
         return Ok();
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "The PUT call to api/Todos/{TodoId} failed. Task value was {Task}",
            todoId, task);
         return BadRequest();
      }

   }

   // PUT api/Todos/5/Complete
   [HttpPut("{todoId}/Complete")]
   public async Task<IActionResult> Complete(int todoId)
   {

      _logger.LogInformation("PUT: api/Todos/{TodoId}/Complete", todoId);

      try
      {
         await _data.CompleteTodo(GetUserId(), todoId);
         return Ok();
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "The PUT call to api/Todos/{TodoId}/Complete failed.",
            todoId);
         return BadRequest();
      }

   }

   // DELETE api/Todos/5
   [HttpDelete("{todoId}")]
   public async Task<IActionResult> Delete(int todoId)
   {



      _logger.LogInformation("DELETE: api/Todos/{TodoId}", todoId);

      try
      {
         await _data.Delete(GetUserId(), todoId);
         return Ok();
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "The DELETE call to api/Todos/{TodoId} failed.", todoId);
         return BadRequest();
      }
   }
}
