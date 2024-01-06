using Microsoft.AspNetCore.Mvc;
using MusicWebApp.Data;
using MusicWebApp.Entities;
using MusicWebApp.ViewModels;
using MusicWebApp.ViewModels.Home;
using MusicWebApp.ViewModels.Songs;
using System.Diagnostics;

namespace MusicWebApp.Controllers
{
	public class HomeController : Controller
	{
		[HttpGet]
		public IActionResult Index(int? Id)
		{
			if (Id == null || Id == 0)
			{
				if (TempData.ContainsKey("UserId") && TempData["UserId"] is int tempId && tempId > 0)
				{
					Id = tempId;
				}
				else
				{
					RedirectToAction("Login", "User");
				}
			}
			if (Id != null)
			{
				WebPlayerDbContext dbContext = new WebPlayerDbContext();

				User loggedUser = dbContext.Users.Where(u => u.Id == Id).FirstOrDefault();

				if (loggedUser == null)
				{
					return RedirectToAction("Login", "User");
				}

				SongVM viewModel = new SongVM();

				viewModel.userId = loggedUser.Id;
				viewModel.Username = loggedUser.Username;
				viewModel.Songs = dbContext.Songs.ToList();
				viewModel.songsToPlaylists = new List<SongToPlaylist>();

				List<SongToPlaylist> songsToPlaylists = dbContext.SongsToPlaylists.Where(u => u.Id == loggedUser.Id).ToList();


				viewModel.Playlists = new List<Playlist>();
				foreach (Playlist playlist in dbContext.Playlists.Where(u => u.OwnerId == loggedUser.Id).ToList())
				{
					viewModel.Playlists.Add(playlist);
				}


				List<PlaylistToUser> shares = dbContext.PlaylistsToUsers.Where(u => u.Id == loggedUser.Id).ToList();

				foreach (PlaylistToUser share in shares)
				{
					viewModel.Playlists.Add(dbContext.Playlists.Where(u => u.Id == share.PlaylistId).FirstOrDefault());
				}
				return View(viewModel);
			}
			else
			{
				return RedirectToAction("Login", "User");
			}
		}
	}
}