using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;

namespace StudentPortal.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;

        // DI gives us the AppDbContext here
        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        // ========== LIST ==========
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.ToListAsync();
            return View(students);
        }

        // ========== CREATE ==========

        // TRIP 1: show the empty form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // TRIP 2: receive the filled form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"{student.Name} was added successfully!";
            return RedirectToAction(nameof(Index));
        }

        // ========== EDIT ==========

        // TRIP 1: show the form filled with the student's data
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // TRIP 2: save the changes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(student);
            }

            _context.Students.Update(student);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"{student.Name} was updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // ========== DELETE ==========

        // TRIP 1: show "Are you sure?"
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // TRIP 2: really delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"{student.Name} was deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}