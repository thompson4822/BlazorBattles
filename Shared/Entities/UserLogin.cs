using System.ComponentModel.DataAnnotations;

namespace BlazorBattles.Shared.Entities
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Please enter the Username.")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}