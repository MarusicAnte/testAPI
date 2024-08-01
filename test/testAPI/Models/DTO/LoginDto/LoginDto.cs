using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace testAPI.Models.DTO.LoginDto
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
