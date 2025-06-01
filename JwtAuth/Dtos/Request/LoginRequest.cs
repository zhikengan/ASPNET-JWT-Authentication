﻿using System.ComponentModel.DataAnnotations;

namespace JwtAuth.Dtos.Request
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
