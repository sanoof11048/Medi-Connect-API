using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string? PhotoUrl { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

    }

}
