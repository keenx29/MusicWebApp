using WMPLib;
using Microsoft.AspNetCore.Mvc;
using MusicWebApp.Data;
using MusicWebApp.Entities;
using MusicWebApp.Extension;
using MusicWebApp.ViewModels.Songs;
using MusicWebApp.ViewModels.Home;
using Microsoft.EntityFrameworkCore;
using MusicWebApp.ViewModels.Playlists;

namespace MusicWebApp.Controllers
{
	public class SongController : Controller
	{
		
		[HttpGet]
		public IActionResult Create(int? Id)
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
				viewModel.Playlists = new List<Playlist>();
				viewModel.CurrentSong = new Song();
				viewModel.TotalSongs = 0;
				viewModel.Username = loggedUser.Username;
				foreach (Playlist playlist in dbContext.Playlists.Where(u => u.OwnerId == loggedUser.Id).ToList())
				{
					viewModel.Playlists.Add(playlist);
				}

				return View(viewModel);
			}
			else
			{
				return RedirectToAction("Login", "User");
			}
		}
		[HttpPost]
		public IActionResult Create(SongVM model)
		{
			if (model.AudioURL.Substring(0, 24) != "https://www.dropbox.com/")
			{
				ModelState.AddModelError("AudioURL", "*Invalid URL!");
				return View(model);
			}
			if (model.Name.Trim() == null)
			{
				ModelState.AddModelError("Name", "*This field is Required!");
				return View(model);
			}
			if (model.Artist.Trim() == null)
			{
				ModelState.AddModelError("Artist", "*This field is Required!");
				return View(model);
			}

			WebPlayerDbContext dbContext = new WebPlayerDbContext();
			WindowsMediaPlayer player = new WindowsMediaPlayer();
			Song newSong = new Song();
			newSong.Name = model.Name;
			newSong.Artist = model.Artist;
			newSong.AudioURL = model.AudioURL;

			if (newSong.AudioURL.GetLast(4) == "dl=0")
			{
				newSong.AudioURL = newSong.AudioURL.Substring(0, newSong.AudioURL.Length - 1) + "1";
			}

			player.URL = newSong.AudioURL;
			player.settings.volume = 0;
			player.controls.play();
			newSong.Duration = player.currentMedia.durationString;
			player.controls.stop();

			dbContext.Add(newSong);
			dbContext.SaveChanges();

			TempData["UserId"] = model.userId;
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult Edit(int? songId)
		{
			SongVM viewModel = new SongVM();
			int? userId = null;
			if (TempData.ContainsKey("UserId") && TempData["UserId"] is int tempId && tempId > 0)
			{
				userId = tempId;
				viewModel.userId = (int)TempData["UserId"];
			}
			else
			{
				RedirectToAction("Login", "User");
			}

			if (userId != null)
			{
				WebPlayerDbContext dbContext = new WebPlayerDbContext();

				User loggedUser = dbContext.Users.Where(u => u.Id == userId).FirstOrDefault();

				if (loggedUser == null)
				{
					return RedirectToAction("Login", "User");
				}

				Song songModel = dbContext.Songs.Where(u => u.Id == songId).FirstOrDefault();

				viewModel.Artist = songModel.Artist;
				viewModel.AudioURL = songModel.AudioURL;
				viewModel.Name = songModel.Name;
				viewModel.Username = loggedUser.Username;

				return View(viewModel);
			}
			else
			{
				return RedirectToAction("Login", "User");
			}
		}

		[HttpPost]
		public IActionResult Edit(SongVM model)
		{
			if (model.songId == 0)
			{
				ModelState.AddModelError("Id", "*System id error!");
				return View(model);
			}
			if (model.AudioURL.Substring(0, 24) != "https://www.dropbox.com/")
			{
				ModelState.AddModelError("AudioURL", "*Invalid URL!");
				return View(model);
			}

			if (model.Name.Trim() == null)
			{
				ModelState.AddModelError("Name", "*This field is Required!");
				return View(model);
			}

			if (model.Artist.Trim() == null)
			{
				ModelState.AddModelError("Artist", "*This field is Required!");
				return View(model);
			}

			WebPlayerDbContext dbContext = new WebPlayerDbContext();

			WindowsMediaPlayer player = new WindowsMediaPlayer();
			player.settings.autoStart = false;
			Song newSong = new Song();

			newSong.Id = model.songId;
			newSong.Name = model.Name;
			newSong.Artist = model.Artist;
			newSong.AudioURL = model.AudioURL;

			if (newSong.AudioURL.GetLast(4) == "dl=0")
			{
				newSong.AudioURL = newSong.AudioURL.Substring(0, newSong.AudioURL.Length - 1) + "1";
			}

			player.URL = newSong.AudioURL;
			player.settings.mute = true;
			player.controls.play();
			newSong.Duration = player.currentMedia.durationString;
			player.controls.stop();

			dbContext.Update(newSong);
			dbContext.SaveChanges();

			TempData["UserId"] = model.userId;
			return RedirectToAction("Index", "Home");
		}
		[HttpPost]
		public IActionResult Delete(SongVM model)
		{
			if (model.songId == 0)
			{
				ModelState.AddModelError("Id", "*System id error!");
				return RedirectToAction("Index","Home",model);
			}
			if (model.AudioURL != null)
			{
				if (model.AudioURL.Length < 25)
				{
					ModelState.AddModelError("AudioURL", "*Invalid URL!");
					return RedirectToAction("Index", "Home", model);
				}
				if (model.AudioURL.Substring(0, 24) != "https://www.dropbox.com/")
				{
					ModelState.AddModelError("AudioURL", "*Invalid URL!");
					return RedirectToAction("Index", "Home", model);
				}

			}

			WebPlayerDbContext dbContext = new WebPlayerDbContext();

			Song song = dbContext.Songs.Where(u => u.Id == model.songId).FirstOrDefault();

			if (song != null)
			{
				dbContext.Remove(song);
				dbContext.SaveChanges();
			}

			TempData["UserId"] = model.userId;
			return RedirectToAction("Index", "Home");
		}
		[HttpGet]
		public IActionResult Manage(int songId)
		{
			SongVM model = new SongVM();
			model.Playlists = new List<Playlist>();
			int? userId = null;
			if (songId == 0)
			{
				ModelState.AddModelError("Id", "*System id error!");
				return View();
			}

			if (TempData.ContainsKey("UserId") && TempData["UserId"] is int tempId && tempId > 0)
			{
				userId = tempId;
				model.userId = (int)TempData["UserId"];
			}
			else
			{
				RedirectToAction("Login", "User");
			}

			if (userId != null)
			{
				WebPlayerDbContext dbContext = new WebPlayerDbContext();

				User loggedUser = dbContext.Users.Where(u => u.Id == userId).FirstOrDefault();

				if (loggedUser == null)
				{
					return RedirectToAction("Login", "User");
				}

				Song song = dbContext.Songs.Where(u => u.Id == songId).FirstOrDefault();
				List<Playlist> playlists = dbContext.Playlists.Where(u => u.OwnerId == loggedUser.Id).ToList();
				model.Song = song;
				foreach (Playlist playlist in playlists)
				{
					SongToPlaylist songLink = new SongToPlaylist();
					songLink.SongId = song.Id;
					songLink.PlaylistId = playlist.Id;
					SongToPlaylist tempLink = dbContext.SongsToPlaylists.Where(u => u.SongId == songLink.SongId && u.PlaylistId == songLink.PlaylistId).FirstOrDefault();
					if (tempLink != null && tempLink.SongId == songLink.SongId && tempLink.PlaylistId == songLink.PlaylistId)
					{
						continue;
					}
					model.Playlists.Add(playlist);

				}
				model.Username = loggedUser.Username;
			}
			return View(model);
		}
		[HttpPost]
		public IActionResult Manage(SongVM model)
		{
			if (model.songId == 0 || model.userId == 0 || model.playlistId == 0 || model.songId == 0)
			{
				return RedirectToAction("Index", model.userId);
			}
			WebPlayerDbContext dbContext = new WebPlayerDbContext();
			Playlist playlistModel = dbContext.Playlists.Where(u => u.Id == model.playlistId).FirstOrDefault();
			Song songModel = dbContext.Songs.Where(u => u.Id == model.songId).FirstOrDefault();
			SongToPlaylist linkModel = new SongToPlaylist();
			PlaylistVM playlistVM = new PlaylistVM();

			User loggedUser = dbContext.Users.Where(u => u.Id == model.userId).FirstOrDefault();

			if (loggedUser == null)
			{
				return RedirectToAction("Login", "User");
			}

			if (loggedUser.Id != 0)
			{
				if (playlistModel != null && songModel != null)
				{
					if (playlistModel.OwnerId == model.userId)
					{
						linkModel.Playlist = playlistModel;
						linkModel.PlaylistId = playlistModel.Id;
						linkModel.Song = songModel;
						linkModel.SongId = songModel.Id;
						dbContext.Add(linkModel);
						dbContext.SaveChanges();
						playlistVM.userId = loggedUser.Id;
						playlistVM.playlistId = playlistModel.Id;
						playlistVM.ImageURL = playlistModel.ImageURL;
						playlistVM.Name = playlistModel.Name;
						playlistVM.Username = loggedUser.Username;
					}
				}
			}
			return RedirectToAction("Manage", "Playlist", playlistVM);
		}
	}
}