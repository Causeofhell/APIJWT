﻿

using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class LoginDTO
    {
        [Required, EmailAddress]
        public string? Email { get; set; } = string.Empty;

        [Required]
        public string? Password { get; set; } = string.Empty;
    }
}
