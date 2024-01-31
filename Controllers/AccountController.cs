using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWeb.Data;
using MilestoneMotorsWeb.Models;
using MilestoneMotorsWeb.ViewModels;

namespace MilestoneMotorsWeb.Controllers
{
    public class AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
        : Controller
    {
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly UserManager<User> _userManager = userManager;

        [HttpGet]
        public IActionResult Register()
        {
            var registerViewModel = new RegisterUserViewModel();
            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel registerUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(registerUserViewModel.Email);
                if (user != null)
                {
                    TempData["Error"] = "User already exists";
                    return View(registerUserViewModel);
                }
                var newUser = new User
                {
                    Email = registerUserViewModel.Email.Trim(),
                    UserName = registerUserViewModel.Username.Trim()
                };
                var response = await _userManager.CreateAsync(
                    newUser,
                    registerUserViewModel.Password
                );
                if (response.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                    TempData["Success"] = "Account successfully created!";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in response.Errors)
                    {
                        TempData["Error"] += error.Description;
                    }

                    return View(registerUserViewModel);
                }
            }
            return View(registerUserViewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            var loginUserViewModel = new LoginUserViewModel();
            return View(loginUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel loginUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginUserViewModel.Email);
                if (user != null)
                {
                    var passwordCheck = await _userManager.CheckPasswordAsync(
                        user,
                        loginUserViewModel.Password
                    );
                    if (passwordCheck)
                    {
                        var result = await _signInManager.PasswordSignInAsync(
                            user,
                            loginUserViewModel.Password,
                            false,
                            false
                        );
                        if (result.Succeeded)
                        {
                            TempData["Success"] = "Login successful!";
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    TempData["Error"] = "Invalid user credentials.";
                    return View(loginUserViewModel);
                }
                TempData["Error"] = "No user registered with that email.";
                return View(loginUserViewModel);
            }
            return View(loginUserViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
