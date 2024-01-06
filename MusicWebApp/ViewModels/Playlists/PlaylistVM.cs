using MusicWebApp.Entities;
using System.ComponentModel.DataAnnotations;

namespace MusicWebApp.ViewModels.Playlists
{
	public class PlaylistVM
	{
		public int userId { get; set; }
		public int playlistId { get; set; }
		[Required]
		public string ImageURL { get; set; }
		[Required]
		public string Name { get; set; }
		public List<Song> Songs { get; set; } = new List<Song>();
		public Song CurrentSong {  get; set; }
		public int TotalSongs { get; set; } 
		public string? Username { get; set; }
		public int TotalDuration { get; set; }
		public List<Playlist> Playlists { get; set; }
	}
}
