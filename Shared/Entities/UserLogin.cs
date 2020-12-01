using System.ComponentModel.DataAnnotations;

namespace BlazorBattles.Shared.Entities
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Please enter the Email.")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}