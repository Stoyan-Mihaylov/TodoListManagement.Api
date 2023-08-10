using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListManagement.Business.Dtos.ImportDtos;
using TodoListManagement.Data.Models;

namespace TodoListManagement.Business.Service.TodoItem
{
    public interface ITodoItemService
    {
        Task<IEnumerable<string>> GetAllAsync();
        Task<Item> CreateAsync(TodoItemDto todoItemDto);
        Task<Item> UpdateAsync(int id, TodoItemDto todoItemDto);
        Task DeleteAsync(int id);
    }
}