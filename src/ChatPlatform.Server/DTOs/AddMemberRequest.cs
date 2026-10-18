using System.ComponentModel.DataAnnotations;

namespace ChatPlatform.Server.DTOs
{
    public class AddMemberRequest
    {
        [Required]
        public string Login { get; set; } = string.Empty;
    }
}
