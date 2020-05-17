using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Logging;
using UserRegistraionInCore.Models;
using UserRegistraionInCore.ViewModels;

namespace UserRegistraionInCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(UserManager<ApplicationUser> userManager, ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._logger = logger;
            this._roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            var users = await _userManager.Users.Where(x => x.Id == Id).FirstOrDefaultAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            user.FirstName = model.FirstName;
            user.Email = model.Email;
            user.UserName = model.UserName;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel vm)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(vm.Name));

            return RedirectToAction("IndexRole");
        }

        [HttpGet]
        public IActionResult IndexRole()
        {
            var roles = _roleManager.Roles.ToList();
            List<CreateRoleViewModel> vm = new List<CreateRoleViewModel>();

            foreach (var item in roles)
            {
                vm.Add(new CreateRoleViewModel { Id = item.Id, Name = item.Name });
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string Id)
        {
            var role = await _roleManager.Roles.Where(x => x.Id == Id).FirstOrDefaultAsync();
            CreateRoleViewModel vm = new CreateRoleViewModel();
            vm.Id = role.Id;
            vm.Name = role.Name;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(CreateRoleViewModel vm)
        {
            var role = await _roleManager.Roles.Where(x => x.Id == vm.Id).FirstOrDefaultAsync();
            role.Name = vm.Name;

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("IndexRole");
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await _roleManager.Roles.Where(x => x.Id == Id).FirstOrDefaultAsync();
            CreateRoleViewModel vm = new CreateRoleViewModel();
            vm.Id = role.Id;
            vm.Name = role.Name;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(CreateRoleViewModel vm)
        {
            var role = await _roleManager.Roles.Where(x => x.Id == vm.Id).FirstOrDefaultAsync();
            role.Name = vm.Name;

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("IndexRole");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
