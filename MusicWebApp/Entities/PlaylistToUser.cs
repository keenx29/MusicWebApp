using System.ComponentModel.DataAnnotations.Schema;

namespace MusicWebApp.Entities
{
    public class PlaylistToUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PlaylistId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [ForeignKey(nameof(PlaylistId))]
        public Playlist Playlist { get; set; }
    }
}
