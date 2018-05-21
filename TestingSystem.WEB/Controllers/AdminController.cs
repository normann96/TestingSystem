using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using TestingSystem.BLL.EntitiesDto;
using TestingSystem.BLL.Interfaces;
using TestingSystem.Constants;
using TestingSystem.WEB.Models.Users;

namespace TestingSystem.WEB.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class AdminController : Controller
    {
        private IUserService UserService { get; }
        private IRoleService RoleService { get; }

        public AdminController(IUserService userService, IRoleService roleService)
        {
            UserService = userService ?? throw new ArgumentNullException(nameof(userService));
            RoleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
        }
        // GET: Admin
        public async Task<ActionResult> Index()
        {
            var usersDto = await UserService.GetAllAsync();
            var rolesDto = RoleService.Roles;
            var users = usersDto.Select(userDto => new UserViewModel
            {
                Id = userDto.Id,
                UserName = userDto.UserName,
                Email = userDto.Email,
                Roles = rolesDto.Join(userDto.RolesId, role => role.Id, id => id, (role, id) => role.Name)
            }).ToList();

            return View(users);
        }

        [HttpGet]
        public ActionResult CreateUser()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> CreateUser(CreateUserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                UserDto user = new UserDto { UserName = userViewModel.Name, Email = userViewModel.Email };

                IdentityResult result = await UserService.CreateAsync(user, userViewModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                AddErrorsFromResult(result);
            }
            return View(userViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(string id)
        {
            if (User.Identity.GetUserId() == id)
            {
                ViewBag.Error = "You can't delete yourself!";
                return View("Error");
            }

            IdentityResult result = await UserService.DeleteAsync(id);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View("Error");
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserService?.Dispose();
                RoleService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}