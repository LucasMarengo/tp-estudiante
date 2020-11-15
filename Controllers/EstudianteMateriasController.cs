using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tpEstudiante;
using tpEstudiante.Models;

namespace tpEstudiante.Controllers
{
    [Authorize(Roles = Roles.AdminRole)]
    public class EstudianteMateriasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstudianteMateriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EstudianteMaterias
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EstudianteMaterias.Include(e => e.Estudiante).Include(e => e.Materia);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: EstudianteMaterias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudianteMateria = await _context.EstudianteMaterias
                .Include(e => e.Estudiante)
                .Include(e => e.Materia)
                .FirstOrDefaultAsync(m => m.EstudianteId == id);
            if (estudianteMateria == null)
            {
                return NotFound();
            }

            return View(estudianteMateria);
        }

        // GET: EstudianteMaterias/Create
        public IActionResult Create()
        {
            ViewData["EstudianteId"] = new SelectList(_context.Estudiantes, "Id", "Nombre");
            ViewData["MateriaId"] = new SelectList(_context.Materias, "Id", "Nombre");
            return View();
        }

        // POST: EstudianteMaterias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EstudianteId,MateriaId,FechaInscripcion")] EstudianteMateria estudianteMateria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estudianteMateria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstudianteId"] = new SelectList(_context.Estudiantes, "Id", "Nombre", estudianteMateria.EstudianteId);
            ViewData["MateriaId"] = new SelectList(_context.Materias, "Id", "Nombre", estudianteMateria.MateriaId);
            return View(estudianteMateria);
        }

        // GET: EstudianteMaterias/Edit/5
        public async Task<IActionResult> Edit(int? idEstudiante, int? idMateria)
            {
            if (idEstudiante == null || idMateria == null)
            {
                return NotFound();
            }

            var estudianteMateria = await _context.EstudianteMaterias.FindAsync(idEstudiante, idMateria);
            if (estudianteMateria == null)
            {
                return NotFound();
            }
            ViewData["EstudianteId"] = new SelectList(_context.Estudiantes, "Id", "Nombre", estudianteMateria.EstudianteId);
            ViewData["MateriaId"] = new SelectList(_context.Materias, "Id", "Nombre", estudianteMateria.MateriaId);
            return View(estudianteMateria);
        }

        // POST: EstudianteMaterias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EstudianteId,MateriaId,FechaInscripcion")] EstudianteMateria estudianteMateria)
        {
            //if (id != estudianteMateria.EstudianteId)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudianteMateria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudianteMateriaExists(estudianteMateria.EstudianteId))
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
            ViewData["EstudianteId"] = new SelectList(_context.Estudiantes, "Id", "Nombre", estudianteMateria.EstudianteId);
            ViewData["MateriaId"] = new SelectList(_context.Materias, "Id", "Nombre", estudianteMateria.MateriaId);
            return View(estudianteMateria);
        }

        // GET: EstudianteMaterias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudianteMateria = await _context.EstudianteMaterias
                .Include(e => e.Estudiante)
                .Include(e => e.Materia)
                .FirstOrDefaultAsync(m => m.EstudianteId == id);
            if (estudianteMateria == null)
            {
                return NotFound();
            }

            return View(estudianteMateria);
        }

        // POST: EstudianteMaterias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estudianteMateria = await _context.EstudianteMaterias.FindAsync(id);
            _context.EstudianteMaterias.Remove(estudianteMateria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstudianteMateriaExists(int id)
        {
            return _context.EstudianteMaterias.Any(e => e.EstudianteId == id);
        }
    }
}
