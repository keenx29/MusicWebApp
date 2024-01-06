using Microsoft.AspNetCore.Mvc;
using MusicWebApp.Data;
using MusicWebApp.Entities;
using MusicWebApp.ViewModels.Users;

namespace MusicWebApp.Controllers
{
    public class UserController : Controller
	{
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login(LoginVM model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			WebPlayerDbContext dbContext = new WebPlayerDbContext();

			User loggedUser = dbContext.Users.Where
				(u => u.Username == model.Username && u.Password == model.Password).FirstOrDefault();

			if (loggedUser == null)
			{
				ModelState.AddModelError("credentials", "Invalid credentials!");
				return View(model);
			}
			TempData["UserId"] = loggedUser.Id;
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Register(LoginVM model)
		{
			if (model.Username.Length < 3)
			{
				ModelState.AddModelError("Username", "*Username can't be shorter than 3 characters!");
			}
			if (model.Username.Length > 20)
			{
				ModelState.AddModelError("Username", "*Username can't be longer than 20 characters!");
			}
			if (model.Password.Length < 4)
			{
				ModelState.AddModelError("Password", "*Password can't be shorter than 4 characters!");
			}
			if (model.Password.Length > 20)
			{
				ModelState.AddModelError("Password", "*Password can't be longer than 20 characters!");
			}
			WebPlayerDbContext dbContext = new WebPlayerDbContext();

			User entity = new User();
			entity.Username = model.Username;
			entity.Password = model.Password;

			dbContext.Users.Add(entity);
			dbContext.SaveChanges();

			User loggedUser = dbContext.Users.Where(u => u.Username == entity.Username && u.Password == entity.Password).FirstOrDefault();

			if (loggedUser == null)
			{
				return RedirectToAction("Login", "User");
			}
			TempData["UserId"] = loggedUser.Id;
			return RedirectToAction("Index", "Home");
		}
	}
}
