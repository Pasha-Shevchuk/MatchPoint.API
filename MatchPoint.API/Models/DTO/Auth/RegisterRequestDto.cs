using System.Security.Principal;

namespace MatchPoint.API.Models.DTO.Auth
{
    public class RegisterRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
