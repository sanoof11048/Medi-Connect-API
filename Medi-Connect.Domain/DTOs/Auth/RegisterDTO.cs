using Medi_Connect.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.Auth
{
    public class RegisterDTO
    {
        [Required]
        public string? FullName { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required, Phone]
        public string? PhoneNumber { get; set; }

        [Required, MinLength(6)]
        public string? Password { get; set; }


        public IFormFile? PhotoFile { get; set; }
    }

}
