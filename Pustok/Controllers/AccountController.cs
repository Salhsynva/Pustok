using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pustok.Models;
using Pustok.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager)
        {
            this._userManager = userManager;
        }
        public IActionResult Index()
        {
            AllMembersViewModel allMembersVM = new AllMembersViewModel
            {
                loginVM = new MemberLoginViewModel(),
                registerVM = new MemberRegisterViewModel()
            };
            return View(allMembersVM);
        }
        [HttpPost]
        public async Task<IActionResult> Register(MemberRegisterViewModel memberVM)
        {
            if (!ModelState.IsValid)
                return View("index", new AllMembersViewModel { registerVM = memberVM});

            AppUser appUser = new AppUser
            {
                Fullname = memberVM.Fullname,
                UserName = memberVM.Username,
                Email = memberVM.Email
            };

            var result =  await _userManager.CreateAsync(appUser, memberVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View("index");
            }


            return RedirectToAction("index","home");
        }
        [HttpPost]
        public async Task<IActionResult> Login(MemberLoginViewModel memberVM)
        {
            if (!ModelState.IsValid)
            {
                return View("index",new AllMembersViewModel { loginVM = memberVM});
            }

            return View();
        }
    }
}
