using System.ComponentModel.DataAnnotations;

namespace BackendServiceStarter.Models.Requests.User
{
    public class CreateUserRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string ConfirmPassword { get; set; }
        
        public UserRole Role { get; set; }
    }
}