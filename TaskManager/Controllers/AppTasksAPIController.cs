using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Framework;

namespace TaskManager.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AppTasksAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppTasksAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppTaskModel>>> GetTasks()
        {
            List<AppTask> appTasks = await _context.Tasks.Include(t => t.Project).ToListAsync();

            var taskModels = appTasks.Select(t => new AppTaskModel
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Priority = t.Priority,
                ProjectName = t.Project?.Name ?? "No Project",
                DueDate = t.DueDate,
                IsCompleted = t.IsCompleted
            }).ToList();

            return taskModels;
        }

        // GET: 
        [HttpGet("{id}")]
        public async Task<ActionResult<AppTaskModel>> GetTask(int id)
        {
            var task = await _context.Tasks.Include(t => t.Project).FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            var taskModel = new AppTaskModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Priority = task.Priority,
                ProjectName = task.Project?.Name ?? "No Project",
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted
            };

            return taskModel;
        }

        // POST: 
        [HttpPost]
        public async Task<ActionResult<AppTaskModel>> PostTask(AppTask task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var taskModel = new AppTaskModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Priority = task.Priority,
                ProjectName = task.Project?.Name,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted
            };

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, taskModel);
        }

        // PUT: 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, AppTask task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
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

            return NoContent();
        }

        // DELETE:
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}