using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todo.Site.Models;

namespace Todo.Site.Domain
{
    public class TodoCleaner
    {
        private readonly TodoContext _context;
        private static Timer _timer;
        private static int TimeCleanMilliseconds  = 1000;

        public TodoCleaner(TodoContext context)
        {
            _context = context;

            if (_timer == null)
            {
                _timer = new Timer(
                    callback: Cleaner,
                    state: null,
                    dueTime: TimeCleanMilliseconds,
                    period: TimeCleanMilliseconds
                );
            }
        }

        private void Cleaner(object state)
        {
            var items = _context.TodoItems.Where(x =>
                x.Expire.HasValue && x.Created.HasValue &&
                x.Created.Value.AddMilliseconds(x.Expire.Value.TotalMilliseconds) <= DateTime.Now).ToArray();

            _context.TodoItems.RemoveRange(items);
            _context.SaveChanges();
        }
    }
}
