using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoListManagement.Api.Common.Constants;
using TodoListManagement.Api.Extensions;
using TodoListManagement.Business.Dtos.ImportDtos;
using TodoListManagement.Business.Service.TodoItem;


namespace TodoListManagement.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;

        public TodoItemController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var itemsList = await _todoItemService.GetAllAsync();
            return Ok(itemsList);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] TodoItemDto todoItemDto)
        {
            var item = await _todoItemService.CreateAsync(todoItemDto);
            return CreatedAtAction(nameof(Create), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] TodoItemDto todoItemDto)
        {
            var item = await _todoItemService.UpdateAsync(id, todoItemDto);
            return CreatedAtAction(nameof(Update), new { id = item.Id }, item);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await _todoItemService.DeleteAsync(id);
            return Ok();
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("admin")]
        public IActionResult Admin()
        {
            var email = User.GetEmail();
            var role = User.GetRole();

            return Ok(new { Message = $"Hello, {email}! You are an {role}" });
        }
    }
}
