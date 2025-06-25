using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.Auth.OTP
{
    public class OTPRequestDTO
    {
        [EmailAddress]
        public string? EmailAddress { get; set; }
    }
}
