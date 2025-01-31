using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace TaskManager.Controllers
{
    [Authorize]
    [Route("apptask")]
    public class AppTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: 
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tasks = await _context.Tasks
                .Include(t => t.Project) 
                .ToListAsync();

            
            var taskModels = tasks.Select(t => new AppTaskModel
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Priority = t.Priority,
                ProjectName = t.Project.Name, 
                DueDate = t.DueDate,
                IsCompleted = t.IsCompleted
            }).ToList();

            return View(taskModels); 
        }

        // GET: 
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.Project) 
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task); 
        }

        // GET: 
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(); 
        }

        // POST: 
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Priority,ProjectId")] AppTask task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); 
            }
            return View(task);
        }

        // GET: 
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task); 
        }

        // POST: 
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Priority,ProjectId")] AppTask task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Tasks.Any(e => e.Id == id))
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
            return View(task);
        }

        // GET: 
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task); 
        }

        // POST: 
        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); 
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}

