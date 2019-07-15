using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todo.Site.Models;

namespace Todo.Site.Domain
{
    public interface ITodoCrudManger
    {
        Task DeleteAsync(long id);
        Task UpdateAsync(TodoItem todoItem);
        Task CreateAsync(TodoItem todoItem);
        Task<TodoItem> GetAsync(long id);
        Task<TodoItem[]> GetAllAsync();
    }

    public class TodoCrudManger : ITodoCrudManger
    {
        private readonly TodoContext _context;

        public TodoCrudManger(TodoContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TodoItem todoItem)
        {
            _context.Update(todoItem);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(TodoItem todoItem)
        {
            todoItem.Created = DateTime.Now;

            _context.Add(todoItem);
            await _context.SaveChangesAsync();
        }

        public async Task<TodoItem> GetAsync(long id)
        {
            return await _context.TodoItems.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<TodoItem[]> GetAllAsync()
        {
            return await _context.TodoItems.ToArrayAsync();
        }
    }
}
