using MusicWebApp.Entities;
using System.ComponentModel.DataAnnotations;

namespace MusicWebApp.ViewModels.Songs
{
	public class SongVM
	{
		[Required(ErrorMessage = "*This field is Required!")]
		public string Name { get; set; }
		[Required(ErrorMessage = "*This field is Required!")]
		public string Artist { get; set; }
		[Required(ErrorMessage = "*This field is Required!")]
		public string AudioURL { get; set; }
		public int songId { get; set; }
		public int playlistId { get; set; }
		public int userId { get; set; }
		public List<Playlist> Playlists { get; set; }
		public List<Song> Songs { get; set; }
		public Song Song { get; set; }
		public Song CurrentSong { get; set; } = new Song();
		public int TotalSongs { get; set; } = 0;
		public List<SongToPlaylist> songsToPlaylists { get; set; }
		public string? Username { get; set; }
		public SongToPlaylist? songLink { get; set; }
	}
}
