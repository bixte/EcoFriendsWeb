using EcoFriendsWeb.DataModels;
using EcoFriendsWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.IO;

namespace EcoFriendsWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> UserManager;
        private readonly SignInManager<User> SignInManager;

        private readonly ILogger<AccountController> Logger;

        public IWebHostEnvironment Environment;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger, IWebHostEnvironment environment)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            Logger = logger;
            Environment = environment;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null) => View(new LoginViewModel { ReturnUrl = returnUrl });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await SignInManager.PasswordSignInAsync(model.NickName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (model.ReturnUrl == "/Account/Profile")
                        return RedirectToAction("Profile", new { name = model.NickName });

                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);
                    else
                        return RedirectToAction("Index", "Blog");
                }
                else
                    ModelState.AddModelError("", "Неверный логин и (или) пароль");
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Registration() => View();

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.NickName);
                if (user == null)
                {
                    user = new User(model.NickName)
                    {
                        Email = model.Email,
                        PhoneNumber = model.Phone
                    };
                    var resultToCreate = await UserManager.CreateAsync(user, model.Password);
                    if (resultToCreate.Succeeded)
                    {
                        if (model.NickName.EndsWith("_Admin"))
                            await UserManager.AddToRoleAsync(user, "Admin");

                        await UserManager.AddToRoleAsync(user, "User");
                        await SignInManager.SignInAsync(user, false);
                        return RedirectToAction("Index", "Blog");
                    }
                    else
                        foreach (var error in resultToCreate.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            ModelState.AddModelError("", "данный пользователь уже сущестует");
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile(string name, bool? loginChangeResultSucceeded = null, bool? passwordChangeResultSucceeded = null)
        {

            var user = await UserManager.FindByNameAsync(name);
            var model = new UserViewModels
            {
                User = user,
            };
           
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeDataUser(string description, [Required] string userName, [Required] string email, string city, string adress, [Required] string phoneNumber)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(userName);
                if (description != null)
                    user.Description = description;
                if (email != null)
                    user.Email = email;
                if (city != null)
                    user.City = city;
                if (adress != null)
                    user.Adress = adress;
                if (phoneNumber != null)
                    user.PhoneNumber = phoneNumber;

                var changeResult = await UserManager.UpdateAsync(user);
                if (changeResult.Succeeded)
                    return RedirectToAction("Profile", new { name = userName, loginChangeResultSucceeded = true });
                else
                {
                    foreach (var error in changeResult.Errors)
                    {
                        Logger.LogError(error.Description);
                    }
                }
            }
            return RedirectToAction("Profile", new { name = userName, loginChangeResultSucceeded = false });


        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassowrd(string userName, [Required] string nowPassword, [Required] string newPassword)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(nowPassword) && !string.IsNullOrWhiteSpace(newPassword))
                {
                    if (User.Identity.Name == userName)
                    {
                        var user = await UserManager.FindByNameAsync(userName);
                        var resultToChange = await UserManager.ChangePasswordAsync(user, nowPassword, newPassword);
                        if (resultToChange.Succeeded)
                        {
                            return RedirectToAction("Profile", new { name = user.UserName, passwordChangeResultSucceeded = true });
                        }
                    }
                }

            }
            return RedirectToAction("Profile", new { name = userName, passwordChangeResultSucceeded = true });

        }
        [Authorize]
        public async Task<IActionResult> SignOutAccount()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Blog");
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePhoto(string userName, IFormFile avatar)
        {
            string filePath = $"{Environment.WebRootPath}/img/usersAvatar/{avatar.FileName}";

            using (var stream = System.IO.File.Create(filePath))
            {
                await avatar.CopyToAsync(stream);
            }
            var user = await UserManager.FindByNameAsync(userName);
            if (user !=null)
            {
                await DeletePhotoProfile(userName);
                user.AvatarPath = $"{avatar.FileName}";
                await UserManager.UpdateAsync(user);
            }
            
            return RedirectToAction("Profile", new { name = userName });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeletePhotoProfile(string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user != null)
            {
                if (User.Identity.Name == user.UserName)
                {
                    if (user.AvatarPath !=null)
                    {
                        await Task.Run(() =>
                        {
                            System.IO.File.Delete($"{Environment.WebRootPath}/img/usersAvatar/{user.AvatarPath}");
                        });
                    }
                    
                    user.AvatarPath = null;
                    await UserManager.UpdateAsync(user);
                    return RedirectToAction("Profile", new { name = userName });
                }
            }
            ModelState.AddModelError("", "ошибка");
            return RedirectToAction("Profile", new { name = userName });
        }

    }
}
