using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class CreateUserRequestModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}