using System.ComponentModel.DataAnnotations;

namespace BackendServiceStarter.Models.Requests.User
{
    public class UpdateUserRequest
    {
        [EmailAddress]
        public string? Email { get; set; }
        
        public string? Password { get; set; }
        
        public string? ConfirmPassword { get; set; }
        
        public UserRole? Role { get; set; }
    }
}