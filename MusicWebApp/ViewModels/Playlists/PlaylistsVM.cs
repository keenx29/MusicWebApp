using MusicWebApp.Entities;
using System.ComponentModel.DataAnnotations;

namespace MusicWebApp.ViewModels.Playlists
{
	public class PlaylistsVM
	{
		public int userId { get; set; }
		public List<Playlist> playlists { get; set; }
		[Required]
		public string ImageURL { get; set; }
		[Required]
		public string Name { get; set; }
		public string? Username { get; set; }
	}
}
