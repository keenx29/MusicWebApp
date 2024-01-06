using MusicWebApp.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MusicWebApp.ViewModels.Playlists
{
	public class CreatePlaylistVM
	{
		public int userId { get; set; }
		[Required]
		public string ImageURL { get; set; }
		[Required]
		public string Name { get; set; }
		public string? Username { get; set; }
	}
}
