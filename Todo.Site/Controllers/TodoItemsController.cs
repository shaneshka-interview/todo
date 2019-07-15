using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Todo.Site.Domain;
using Todo.Site.Models;

namespace Todo.Site.Controllers
{
    public class TodoItemsController : Controller
    {
        private readonly ITodoCrudManger _crudManger;

        public TodoItemsController(ITodoCrudManger crudManger)
        {
            _crudManger = crudManger;
        }

        // GET: TodoItems
        public async Task<IActionResult> Index()
        {
            return View(await _crudManger.GetAllAsync());
        }

        // GET: TodoItems/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoItem = await _crudManger.GetAsync(id.Value);
            if (todoItem == null)
            {
                return NotFound();
            }

            if (!todoItem.Expire.HasValue)
            {
                await _crudManger.DeleteAsync(todoItem.Id);
            }

            return View(todoItem);
        }

        // GET: TodoItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TodoItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Created,Expire")] TodoItem todoItem)
        {
            if (ModelState.IsValid)
            {
                await _crudManger.CreateAsync(todoItem);
                return RedirectToAction(nameof(Index));
            }
            return View(todoItem);
        }

        // GET: TodoItems/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoItem = await _crudManger.GetAsync(id.Value);
            if (todoItem == null)
            {
                return NotFound();
            }
            return View(todoItem);
        }

        // POST: TodoItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Description,Created,Expire")] TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _crudManger.UpdateAsync(todoItem);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await TodoItemExistsAsync(todoItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todoItem);
        }

        // GET: TodoItems/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoItem = await _crudManger.GetAsync(id.Value);
            if (todoItem == null)
            {
                return NotFound();
            }

            return View(todoItem);
        }

        // POST: TodoItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _crudManger.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        
        private async Task<bool> TodoItemExistsAsync(long id)
        {
            return await _crudManger.GetAsync(id) != null;
        }
    }
}
