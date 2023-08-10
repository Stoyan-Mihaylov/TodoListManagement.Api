using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListManagement.Business.Dtos.ImportDtos;
using TodoListManagement.Business.Exceptions;
using TodoListManagement.Data.Enums;
using TodoListManagement.Data.Models;
using TodoListManagement.Data.Repositories.TodoItems;

namespace TodoListManagement.Business.Service.TodoItem
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemsRepository _todoItemsRepository;

        public TodoItemService(ITodoItemsRepository todoItemsRepository)
        {
            _todoItemsRepository = todoItemsRepository;
        }

        public async Task<IEnumerable<string>> GetAllAsync()
            => await _todoItemsRepository.GetAllAsync();

        public async Task<Item> CreateAsync(TodoItemDto todoItemDto)
        {
            if (!Enum.IsDefined(typeof(Status), todoItemDto.Status))
            {
                throw new ArgumentException($"Invalid status");
            }

            if (todoItemDto.PriorityId <= 0 || todoItemDto.PriorityId > 3)
            {
                throw new ArgumentException($"Ivalid priority");
            }

            var todoItemToInsert = new Item(
            todoItemDto.Title,
            todoItemDto.Description,
            (Status)todoItemDto.Status,
            todoItemDto.PriorityId);

            await _todoItemsRepository.InsertAsync(todoItemToInsert);

            return todoItemToInsert;
        }

        public async Task<Item> UpdateAsync(int id, TodoItemDto todoItemDto)
        {
            var todoItemForUpdate = await _todoItemsRepository.GetByIdAsync(id);
            if (todoItemForUpdate is null)
            {
                throw new NotFoundException($"Item with id: {id} not found");
            }

            if (!Enum.IsDefined(typeof(Status), todoItemDto.Status))
            {
                throw new ArgumentException($"Invalid status");
            }

            if (todoItemDto.PriorityId <= 0 || todoItemDto.PriorityId > 3)
            {
                throw new ArgumentException($"Ivalid priority");
            }

            todoItemForUpdate.Title = todoItemDto.Title;
            todoItemForUpdate.Description = todoItemDto.Description;
            todoItemForUpdate.Status = (Status)todoItemDto.Status;
            todoItemForUpdate.PriorityId = todoItemDto.PriorityId;

            var updatedItem = await _todoItemsRepository.UpdateAsync(todoItemForUpdate);

            return updatedItem;
        }

        public async Task DeleteAsync(int id)
        {
            var itemToDelete = await _todoItemsRepository.GetByIdAsync(id);
            if (itemToDelete is null)
            {
                throw new NotFoundException($"Item with id: {id} not found");
            }

            await _todoItemsRepository.DeleteAsync(itemToDelete);
        }
    }
}
