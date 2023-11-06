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
    public class ClassController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IClassRepository _classRepository;
        private readonly IStudentRepository _studentRepository;

        public ClassController(DatabaseContext context, IClassRepository classRepository,
            IStudentRepository studentRepository)
        {
            _context = context;
            _classRepository = classRepository;
            _studentRepository = studentRepository;
        }

        // GET: Class
        public async Task<IActionResult> Index()
        {
            return _context.Classes != null
                ? View(await _classRepository.GetAllClassAsync())
                : Problem("Entity set 'DatabaseContext.Classes'  is null.");
        }

        // GET: Class/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var @class = await _classRepository.GetClassAsync(id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);
        }

        // GET: Class/Create
        public IActionResult Create()
        {
            if (_context.Students != null)
                ViewBag.Students = _context.Students.Select(c => new SelectListItem
                {
                    Value = c.Guid.ToString(),
                    Text = c.FullName
                });
            return View();
        }

        // POST: Class/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Guid,Name,ClassCode,Students")] Class model)
        {
            await _classRepository.AddClassAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: Class/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }

            if (_context.Students != null)
            {
                var stds = await _context.Students.Where(x => x.ClassGuid == @class.Guid).ToListAsync();
                @class.Students = stds;
                ViewBag.Students = _context.Students.Select(student => new SelectListItem
                {
                    Selected = student.ClassGuid == id,
                    Value = student.Guid.ToString(),
                    Text = student.FullName
                });
            }
            return View(@class);
        }

        // POST: Class/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Guid,Name,ClassCode,Students")] Class @class)
        {
            await _classRepository.UpdateClassAsync(id, @class);
            return RedirectToAction(nameof(Index));
        }

        // GET: Class/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);
        }

        // POST: Class/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Classes == null)
            {
                return Problem("Entity set 'DatabaseContext.Classes'  is null.");
            }

            var @class = await _context.Classes.FindAsync(id);
            if (@class != null)
            {
                _context.Classes.Remove(@class);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}