using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MilestoneMotorsWeb.Data.Interfaces;
using MilestoneMotorsWeb.Models;
using MilestoneMotorsWeb.Utilities;
using MilestoneMotorsWeb.ViewModels;

namespace MilestoneMotorsWeb.Controllers
{
    public class UserController(
        IUserInterface userRepository,
        IPhotoService photoService,
        UserManager<User> userManager,
        SignInManager<User> signInManager
    ) : Controller
    {
        private readonly IUserInterface _userRepository = userRepository;
        private readonly IPhotoService _photoService = photoService;
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;

        [Authorize]
        public async Task<IActionResult> Detail(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }
            var userPage = await _userRepository.GetByIdAsync(id);
            if (userPage == null)
            {
                return NotFound();
            }
            return View(userPage);
        }

        [Authorize]
        public async Task<IActionResult> EditPage(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }
            var userPage = await _userRepository.GetByIdAsync(id);
            if (userPage == null)
            {
                return NotFound();
            }
            var userViewModel = new EditUserViewModel
            {
                City = userPage.City,
                State = userPage.State,
                Country = userPage.Country,
            };
            return View(userViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditPage(EditUserViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View(editVM);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            if (editVM.ProfilePictureImageUrl != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(editVM.ProfilePictureImageUrl);

                if (photoResult.Error != null)
                {
                    ModelState.AddModelError("Image", "Failed to upload image");
                    return View(editVM);
                }

                if (!string.IsNullOrEmpty(user.ProfilePictureImageUrl))
                {
                    _ = _photoService.DeletePhotoAsync(user.ProfilePictureImageUrl);
                }

                user.ProfilePictureImageUrl = photoResult.Url.ToString();
                user.City = editVM?.City?.FirstCharToUpper().Trim() ?? string.Empty;
                user.State = editVM?.State?.FirstCharToUpper().Trim() ?? string.Empty;
                user.Country = editVM?.Country?.FirstCharToUpper().Trim() ?? string.Empty;

                await _userManager.UpdateAsync(user);

                return RedirectToAction("Detail", "User", new { user.Id });
            }

            user.City = editVM?.City?.FirstCharToUpper().Trim() ?? string.Empty;
            user.State = editVM?.State?.FirstCharToUpper().Trim() ?? string.Empty;
            user.Country = editVM?.Country?.FirstCharToUpper().Trim() ?? string.Empty;

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Detail", "User", new { user.Id });
        }

        [Authorize]
        public async Task<IActionResult> MyListings()
        {
            var userCars = await _userRepository.GetUserCarsAsync();
            return View(userCars);
        }

        [HttpPost]
        [Authorize]
        [Route("User/DeleteUser")]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    await _userRepository.Delete(currentUser);
                    await _signInManager.SignOutAsync();
                    return Json(new { success = true, message = "User deleted successfully" });
                }

                return Json(new { success = false, message = "User not found" });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
