using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.UserDTOs
{
    public class UpdateUserDTO
    {
        public Guid Id { get; set; }

        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }

    }
}
