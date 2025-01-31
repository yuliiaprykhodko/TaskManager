using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTasksAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserTasksAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserTasksAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTask>>> GetUserTasks()
        {
            
            var userTasks = await _context.UserTasks.Include(ut => ut.Task).Include(ut => ut.User).ToListAsync();
            return Ok(userTasks); 
        }

        // GET: api/UserTasksAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTask>> GetUserTask(int id)
        {
            var userTask = await _context.UserTasks
                                        .Include(ut => ut.Task)  
                                        .Include(ut => ut.User) 
                                        .FirstOrDefaultAsync(ut => ut.Id == id);

            if (userTask == null)
            {
                return NotFound(); 
            }

            return Ok(userTask); 
        }

        // PUT: api/UserTasksAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserTask(int id, UserTask userTask)
        {
            if (id != userTask.Id)
            {
                return BadRequest(); 
            }

            _context.Entry(userTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTaskExists(id))
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

        
       
        [HttpPost]
        public async Task<ActionResult<UserTask>> PostUserTask(UserTask userTask)
        {
            
            if (userTask == null)
            {
                return BadRequest("Invalid data.");
            }

            _context.UserTasks.Add(userTask);
            await _context.SaveChangesAsync();

            
            return CreatedAtAction(nameof(GetUserTask), new { id = userTask.Id }, userTask);
        }

        // DELETE: 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserTask(int id)
        {
            var userTask = await _context.UserTasks.FindAsync(id);
            if (userTask == null)
            {
                return NotFound(); 
            }

            _context.UserTasks.Remove(userTask);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        
        private bool UserTaskExists(int id)
        {
            return _context.UserTasks.Any(e => e.Id == id);
        }


    }
}
