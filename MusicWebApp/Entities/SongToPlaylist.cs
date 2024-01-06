using System.ComponentModel.DataAnnotations.Schema;

namespace MusicWebApp.Entities
{
	public class SongToPlaylist
	{
		public int Id { get; set; }
		public int SongId { get; set; }
		public int PlaylistId { get; set; }

		[ForeignKey(nameof(SongId))]
		public Song Song { get; set; }

		[ForeignKey(nameof(PlaylistId))]
		public Playlist Playlist { get; set; }
	}
}
