using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListManagement.Data.Models;

namespace TodoListManagement.Data.Repositories.TodoItems
{
    public interface ITodoItemsRepository
    {
        Task<IEnumerable<string>> GetAllAsync();

        Task<Item> GetByIdAsync(int id);

        Task InsertAsync(Item item);

        Task DeleteAsync(Item itemToDelete);

        Task<Item> UpdateAsync(Item item);
    }
}
