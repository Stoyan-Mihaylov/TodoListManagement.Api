using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListManagement.Data.Data;
using TodoListManagement.Data.Models;

namespace TodoListManagement.Data.Repositories.TodoItems
{
    public class TodoItemsRepository : ITodoItemsRepository
    {
        private readonly TodoItemDbContext _dbContext;

        public TodoItemsRepository(TodoItemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<string>> GetAllAsync()
        {
            return await _dbContext.Items
                .OrderByDescending(i => i.Priority)
                .Select(i => i.Priority.Title)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Item> GetByIdAsync(int id)
        {
            var itemToReturn = await _dbContext.Items
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            return itemToReturn;
        }

        public async Task InsertAsync(Item item)
        {
            await _dbContext.Items.AddAsync(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Item itemToDelete)
        {
            _dbContext.Items.Remove(itemToDelete);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Item> UpdateAsync(Item item)
        {
            _dbContext.Items.Update(item);

            await _dbContext.SaveChangesAsync();

            return item;
        }
    }
}
