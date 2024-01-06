using Microsoft.AspNetCore.Mvc;
using MusicWebApp.Data;
using MusicWebApp.Entities;
using MusicWebApp.ViewModels.Playlists;

namespace MusicWebApp.Controllers
{
	public class PlaylistController : Controller
	{
		[HttpGet]
		public IActionResult Index(int? Id)
		{
			PlaylistVM model = new PlaylistVM();

			if (TempData.ContainsKey("UserId") && TempData["UserId"] is int tempId && tempId > 0)
			{
				Id = tempId;
				model.userId = tempId;
			}
			else if (Id == 0)
			{
				RedirectToAction("Login", "User");
			}
			WebPlayerDbContext dbContext = new WebPlayerDbContext();
			
			model.Playlists = dbContext.Playlists.Where(m => m.OwnerId == Id).ToList();
			User loggedUser = dbContext.Users.Where(u => u.Id == Id).FirstOrDefault();

			if (loggedUser == null)
			{
				return RedirectToAction("Login", "User");
			}

			model.Username = loggedUser.Username;

			return View(model);
		}
		[HttpGet]
		public IActionResult Create(int? Id)
		{
			WebPlayerDbContext dbContext = new WebPlayerDbContext();
			User loggedUser = new User();
			CreatePlaylistVM model = new CreatePlaylistVM();
			if (TempData.ContainsKey("UserId") && TempData["UserId"] is int tempId && tempId > 0)
			{
				model.userId = tempId;
				loggedUser = dbContext.Users.Where(u => u.Id == tempId).FirstOrDefault();
				model.Username = loggedUser.Username;
			}
			else
			{
				RedirectToAction("Login", "User");
			}
			return View(model);
		}
		[HttpPost]
		public IActionResult Create(CreatePlaylistVM model)
		{
			if (TempData.ContainsKey("UserId") && TempData["UserId"] is int tempId && tempId > 0)
			{
				model.userId = tempId;
			}
			else
			{
				RedirectToAction("Login", "User");
			}
			if (model.Name.Trim() == null)
			{
				ModelState.AddModelError("Name", "*This field is Required!");
				return View(model);
			}
			if (model.ImageURL.Trim() == null)
			{
				ModelState.AddModelError("ImageURL", "*Invalid URL!");
				return View(model);
			}

			WebPlayerDbContext dbContext = new WebPlayerDbContext();
			Playlist newPlaylist = new Playlist();
			newPlaylist.Name = model.Name;
			newPlaylist.ImageURL = model.ImageURL;
			newPlaylist.OwnerId = model.userId;

			dbContext.Add(newPlaylist);
			dbContext.SaveChanges();

			TempData["UserId"] = model.userId;
			return RedirectToAction("Index", "Playlist");
		}
		[HttpGet]
		public IActionResult Manage(int playlistId)
		{
			PlaylistVM model = new PlaylistVM();
			int? userId = null;
			if (TempData.ContainsKey("UserId") && TempData["UserId"] is int tempId && tempId > 0)
			{
				userId = tempId;
				model.userId = (int)userId;
			}
			else if (TempData.ContainsKey("PlaylistId") && TempData["PlaylistId"] is int tempPId && tempPId > 0)
			{
				playlistId = tempPId;

			}
			else if (model.userId == 0)
			{
				return RedirectToAction("Login", "User");
			}

			if (playlistId != 0)
			{
				WebPlayerDbContext dbContext = new WebPlayerDbContext();
				User loggedUser = dbContext.Users.Where(u => u.Id == model.userId).FirstOrDefault();
				if (loggedUser == null)
				{
					return RedirectToAction("Login", "User");
				}
				Playlist playlistModel = dbContext.Playlists.Where(u => u.Id == playlistId).FirstOrDefault();
				List<SongToPlaylist> linkList = new List<SongToPlaylist>();
				linkList = dbContext.SongsToPlaylists.Where(u => u.PlaylistId == playlistId).ToList();
				model.Songs = new List<Song>();

				foreach (SongToPlaylist link in linkList)
				{
					Song tempSong = new Song();
					tempSong = dbContext.Songs.Where(u => u.Id == link.SongId).FirstOrDefault();
					if (tempSong != null)
					{
						model.Songs.Add(tempSong);
					}
				}
				model.Name = playlistModel.Name;
				model.ImageURL = playlistModel.ImageURL;
				model.Username = loggedUser.Username;
				model.CurrentSong = model.Songs.FirstOrDefault();
				//TO-DO TOTAL DURATION FOR PLAYLIST CALCULATION WITH NEW STRING EXTENSION METHOD
				model.Songs.OrderBy(u => u.Id);
				model.TotalSongs = model.Songs.Count();
				model.Playlists = dbContext.Playlists.Where(u => u.OwnerId == loggedUser.Id).ToList();
				return View(model);
			}
			return RedirectToAction("Index", "Playlist", userId);
		}
	}
}
