using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MusicWebApp.Entities
{
    public class Song
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Artist { get; set; }
        [Required]
        public string AudioURL { get; set; }
		[Required]
		public string Duration { get; set; }

    }
}
