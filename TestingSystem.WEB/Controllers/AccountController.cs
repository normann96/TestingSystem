using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using TestingSystem.BLL.Interfaces;
using TestingSystem.WEB.Models.Account;

namespace TestingSystem.WEB.Controllers
{
    public class AccountController : Controller
    {
        private IUserService UserService { get; }
        private IAuthenticationManager AuthManager => HttpContext.GetOwinContext().Authentication;

        public AccountController(IUserService userService)
        {
            UserService = userService;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return View("Error");

            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel details, string returnUrl)
        {
            var userDto = await UserService.FindAsync(details.Name, details.Password);
            if (userDto == null)
            {
                ModelState.AddModelError("", "Incorrect username or password");
            }
            else
            {
                ClaimsIdentity ident = await UserService.CreateIdentityAsync(userDto,
                    DefaultAuthenticationTypes.ApplicationCookie);

                AuthManager.SignOut();
                AuthManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = false
                }, ident);
                return RedirectToLocal(returnUrl);
            }

            return View(details);
        }

        [Authorize]
        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}