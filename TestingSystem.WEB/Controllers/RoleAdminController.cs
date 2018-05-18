using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TestingSystem.BLL.Interfaces;
using TestingSystem.Constants;
using TestingSystem.WEB.Models.Roles;

namespace TestingSystem.WEB.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class RoleAdminController : Controller
    {
        private IUserService UserService { get; }
        private IRoleService RoleService { get; }

        public RoleAdminController(IUserService userService, IRoleService roleService)
        {
            UserService = userService ?? throw new ArgumentNullException(nameof(userService));
            RoleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
        }

        // GET: RoleAdmin
        public async Task<ActionResult> Index()
        {
            var roles = await RoleService.GetAllAsync();
            var roleViews = roles.Select(role => new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            }).ToList();
            return View(roleViews);
        }


        [HttpPost]
        public async Task<ActionResult> AddRole([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await RoleService.CreateAsync(name);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                AddErrorsFromResult(result);
            }
            return PartialView("_AddRole", name);
        }


        [HttpGet]
        public async Task<ActionResult> EditRole(string id)
        {
            var roleDto = await RoleService.FindByIdAsync(id);
            var users = await UserService.GetAllAsync();

            var members = users.Where(x => x.RolesId.Any(y => y == roleDto.Id)).ToList();
            var nonMembers = users.Except(members).ToList();

            return View(new RoleEditModel
            {
                Role = roleDto,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<ActionResult> EditRole(RoleEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result;
                foreach (var userId in model.IdsToAdd ?? new string[] { })
                {
                    result = await UserService.AddToRoleAsync(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error");
                    }
                }

                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    result = await UserService.RemoveFromRoleAsync(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error");
                    }
                }
                return RedirectToAction("Index");
            }
            return View("Error");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteRole(string id)
        {
            IdentityResult result = await RoleService.DeleteAsync(id);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View("Error");
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
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