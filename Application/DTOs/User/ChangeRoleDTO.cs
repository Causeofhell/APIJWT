
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class ChangeRoleDTO
    {
        [Required, EmailAddress]
        public string? Email { get; set; } = string.Empty;

        [Required]
        public string? RoleName { get; set; } = string.Empty;
    }
}
