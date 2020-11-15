using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tpEstudiante.Models;
using tpEstudiante.ViewModel;

namespace tpEstudiante.Controllers
{
    [Authorize(Roles = Roles.SuperAdminRole)]
    public class UserRolesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var listaUsuarios = new List<UserRolesViewModel>();
            foreach (IdentityUser item in users)
            {
                var nuevoUsuario = new UserRolesViewModel
                {
                    UserId = item.Id,
                    UserName = item.UserName,
                    Email = item.Email,
                    Roles = new List<string>(await _userManager.GetRolesAsync(item))
                };
                listaUsuarios.Add(nuevoUsuario);
            }

            return View(listaUsuarios);
        }



        public async Task<IActionResult> AdminRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"El usuario con el ID = {userId} no se encuentra.";
                return NotFound();
            }

            ViewBag.userID = user.Id;
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            foreach (IdentityRole role in _roleManager.Roles)
            {
                var usuarioRole = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                };
                model.Add(usuarioRole);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AdminRoles(string userId, List<ManageUserRolesViewModel> model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, $"El usuario con el ID = {userId} no se encuentra.");
                return View();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, $"No se pudo quitar los roles existentes del usuario.");
                return View();
            }

            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(x => x.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, $"No se pudo agregar los roles nuevos al usuario.");
                return View();
            }
            return RedirectToAction("Index");
        }
    }
}
