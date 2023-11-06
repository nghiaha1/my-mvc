using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyMvc.Data;
using MyMvc.Repository;

namespace MyMvc.Controllers
{
    public class StudentController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IStudentRepository _studentRepository;
        private readonly IClassRepository _classRepository;

        public StudentController(DatabaseContext context, IStudentRepository studentRepository,
            IClassRepository classRepository)
        {
            _context = context;
            _studentRepository = studentRepository;
            _classRepository = classRepository;
        }

        // GET: Student
        public async Task<IActionResult> Index()
        {
            return _context.Students != null
                ? View(await _studentRepository.GetAllStudentAsync())
                : Problem("Entity set 'DatabaseContext.Students'  is null.");
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _studentRepository.GetStudentAsync(id);
            var @class = await _classRepository.GetClassAsync(student.ClassGuid);
            if (student == null)
            {
                return NotFound();
            }
            student.Class = @class;
            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            if (_context.Classes != null)
                ViewBag.Classes = _context.Classes.Select(c => new SelectListItem
                {
                    Value = c.Guid.ToString(),
                    Text = c.Name
                });
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Guid,FullName,StudentCode,Class")] Student student)
        {
            await _studentRepository.AddStudentAsync(student);
            return RedirectToAction(nameof(Index));
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            if (_context.Classes != null)
                ViewBag.Classes = _context.Classes.Select(c => new SelectListItem
                {
                    Selected = c.Guid == student.ClassGuid,
                    Value = c.Guid.ToString(),
                    Text = c.Name
                });
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Guid,FullName,StudentCode,Class")] Student student)
        {
            await _studentRepository.UpdateStudentAsync(id, student);
            return RedirectToAction(nameof(Index));
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'DatabaseContext.Students'  is null.");
            }

            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(Guid id)
        {
            return (_context.Students?.Any(e => e.Guid == id)).GetValueOrDefault();
        }
    }
}