using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace BackendServiceStarter.Models.Requests.Auth
{
    public class AuthRequest
    {
        [FromBody]
        [Required]
        public string Email { get; set; }
        
        [FromBody]
        [Required]
        public string Password { get; set; }
    }
}