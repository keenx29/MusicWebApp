using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MusicWebApp.Entities
{
    public class Playlist
    {
        [Key]
        public int Id { get; set; }
        public int OwnerId { get; set; }
        [Required]
        public string ImageURL { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]

        [ForeignKey(nameof(OwnerId))]
        public User Owner { get; set; }
    }
}
