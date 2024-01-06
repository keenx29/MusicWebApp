using System.ComponentModel.DataAnnotations;

namespace MusicWebApp.ViewModels.Users
{
    public class LoginVM
    {
        [Required(ErrorMessage = "*This field is Required!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "*This field is Required!")]
        public string Password { get; set; }
        public int userId { get; set; }
    }
}
