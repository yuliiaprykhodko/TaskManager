using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [Authorize]
    [Route("usertasks")]
    public class UserTaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserTaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: 
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var userTasks = await _context.UserTasks
                .Include(ut => ut.Task)
                .Include(ut => ut.User)
                .ToListAsync();

            if (userTasks == null || !userTasks.Any())
            {
                return View(new List<UserTask>()); 
            }

            return View(userTasks);
        }

        // GET: 
        [HttpGet("create")]
        public IActionResult Create()
        {
            
            ViewBag.Users = _context.Users.ToList();
            ViewBag.Tasks = _context.Tasks.ToList();
            return View();
        }

        // POST: 
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserTask userTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); 
            }

            
            ViewBag.Users = _context.Users.ToList();
            ViewBag.Tasks = _context.Tasks.ToList();
            return View(userTask);
        }

        // GET: 
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var userTask = await _context.UserTasks.FindAsync(id);
            if (userTask == null)
            {
                return NotFound();
            }

            
            ViewBag.Users = _context.Users.ToList();
            ViewBag.Tasks = _context.Tasks.ToList();
            return View(userTask);
        }

        // POST: 
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserTask userTask)
        {
            if (id != userTask.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserTaskExists(userTask.Id))
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

            ViewBag.Users = _context.Users.ToList();
            ViewBag.Tasks = _context.Tasks.ToList();
            return View(userTask);
        }

        // GET: 
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userTask = await _context.UserTasks.FindAsync(id);
            if (userTask == null)
            {
                return NotFound();
            }
            return View(userTask);
        }

        // POST: 
        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userTask = await _context.UserTasks.FindAsync(id);
            if (userTask != null)
            {
                _context.UserTasks.Remove(userTask);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        
        private bool UserTaskExists(int id)
        {
            return _context.UserTasks.Any(e => e.Id == id);
        }
    }
}
