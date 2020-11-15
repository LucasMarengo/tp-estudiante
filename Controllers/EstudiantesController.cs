using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tpEstudiante;
using tpEstudiante.Models;
using tpEstudiante.ViewModel;

namespace tpEstudiante.Controllers
{
    [Authorize]
    public class EstudiantesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment env;

        public EstudiantesController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        private void ShowRequest()
        {
            Console.WriteLine("Query");
            Console.WriteLine(this.Request.QueryString);
            foreach (var item in Request.Query)
            {
                Console.WriteLine($"   {item.Key}: {item.Value}");
            }
        }

        // GET: Estudiantes

        public async Task<IActionResult> Index(string busquedaEst, int? CarreraId, int pagina = 1)
        {
            ShowRequest();
            int RegistrosPorPagina = 4; 

            var applicationDbContext = _context.Estudiantes.Include(e => e.Carrera).Select(e => e);
            if (!string.IsNullOrEmpty(busquedaEst))
            {
                applicationDbContext = applicationDbContext.Where(e => e.Nombre.Contains(busquedaEst));
            }
            if (CarreraId.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(e => e.CarreraId == CarreraId.Value);
            }
            //Generar pagina
            var registrosMostrar = applicationDbContext
                        .Skip((pagina - 1) * RegistrosPorPagina)
                        .Take(RegistrosPorPagina);

            EstudiantesViewModel modelo = new EstudiantesViewModel()
            {
                Estudiantes = await registrosMostrar.ToListAsync(),
                ListCarreras = new SelectList(_context.Carreras, "Id", "Descripcion", CarreraId),
                busquedaEst = busquedaEst,
                CarreraId = CarreraId
            };
            modelo.Paginador.PaginaActual = pagina;
            modelo.Paginador.RegistrosPorPagina = RegistrosPorPagina;
            modelo.Paginador.TotalRegistros = await applicationDbContext.CountAsync();
            if (!string.IsNullOrEmpty(busquedaEst))
            {
                modelo.Paginador.ValoresQueryString.Add("busquedaEst", busquedaEst);
            }
            if (CarreraId.HasValue)
            {
                modelo.Paginador.ValoresQueryString.Add("CarreraId", CarreraId.Value.ToString());
            }


            return View(modelo);
;

        }

        // GET: Estudiantes/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiantes
                .Include(e => e.Carrera)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        // GET: Estudiantes/Create
        [Authorize(Roles = Roles.AdminRole)]
        public IActionResult Create()
        {
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Descripcion");
            return View();
        }

        // POST: Estudiantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = Roles.AdminRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Edad,Año,Calle,Localidad,CarreraId,Foto,Legajo")] Estudiante estudiante/*, UserManager<IdentityUser> userManager, string Email*/)
        {
            if (ModelState.IsValid)
            {
                var archivo = HttpContext.Request.Form.Files;
                if (archivo != null && archivo.Count > 0)
                {
                    var archivoFoto = archivo[0];
                    var pathDestino = Path.Combine(env.WebRootPath,"image\\estudiantes");
                    if (archivoFoto.Length > 0)
                    {
                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoFoto.FileName);

                        using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
                        {
                            archivoFoto.CopyTo(filestream);
                            estudiante.Foto = archivoDestino;
                        };

                    }
                }

                _context.Add(estudiante);
                await _context.SaveChangesAsync();

                //var userEstudiante = userManager.Users.Where(x => x.Email == Roles.SuperAdminRole).FirstOrDefault();

                //userEstudiante = new IdentityUser
                //{
                //    UserName = estudiante.Nombre,
                //    Email = Email,
                //    EmailConfirmed = true,
                //    PhoneNumberConfirmed = true
                //};
                //await userManager.CreateAsync(userEstudiante, estudiante.Legajo);
                //await userManager.AddToRoleAsync(userEstudiante, Roles.EstudianteRole);



                return RedirectToAction(nameof(Index));
            }
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Descripcion", estudiante.CarreraId);
            return View(estudiante);
        }

        // GET: Estudiantes/Edit/5
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           
            var estudiante = await _context.Estudiantes
                .Include(x => x.EstudianteMateria)
                .ThenInclude(m => m.Materia)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (estudiante == null)
            {
                return NotFound();
            }
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Descripcion", estudiante.CarreraId);
            return View(estudiante);
        }

        // POST: Estudiantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = Roles.AdminRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Edad,Año,Calle,Localidad,CarreraId,Foto")] Estudiante estudiante)
        {
            if (id != estudiante.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var archivo = HttpContext.Request.Form.Files;
                if (archivo != null && archivo.Count > 0)
                {
                    var archivoFoto = archivo[0];
                    var pathDestino = Path.Combine(env.WebRootPath, "image\\estudiantes");
                    if (archivoFoto.Length > 0)
                    {
                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoFoto.FileName);

                        using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
                        {
                            archivoFoto.CopyTo(filestream);

                            string viejoArchivo = Path.Combine(pathDestino, estudiante.Foto ?? "");
                            if (System.IO.File.Exists(viejoArchivo))
                                System.IO.File.Delete(viejoArchivo);
                            estudiante.Foto = archivoDestino;
                        };

                    }
                }



                try
                {
                    _context.Update(estudiante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudianteExists(estudiante.Id))
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
            ViewData["CarreraId"] = new SelectList(_context.Carreras, "Id", "Descripcion", estudiante.CarreraId);
            return View(estudiante);
        }

        // GET: Estudiantes/Delete/5
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiantes
                .Include(e => e.Carrera)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        // POST: Estudiantes/Delete/5
        [Authorize(Roles = Roles.AdminRole)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstudianteExists(int id)
        {
            return _context.Estudiantes.Any(e => e.Id == id);
        }
    }
}
