using ChatPlatform.Shared;
using System.ComponentModel.DataAnnotations;

namespace ChatPlatform.Server.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [MaxLength(UserLimits.MaxLoginLength)]
        public string Login { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
