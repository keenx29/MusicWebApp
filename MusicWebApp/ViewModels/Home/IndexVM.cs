using MusicWebApp.Entities;

namespace MusicWebApp.ViewModels.Home
{
	public class IndexVM
	{
		public List<Playlist> Playlists { get; set; }
		public List<Song> Songs { get; set; }
		public List<SongToPlaylist> songsToPlaylists { get; set; }
		public string Username { get; set; }
		public int userId { get; set; }
	}
}
