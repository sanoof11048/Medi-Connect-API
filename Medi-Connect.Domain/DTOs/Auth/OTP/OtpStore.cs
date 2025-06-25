using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.Auth.OTP
{
    public class OtpStore
    {
        public string Otp { get; set; }
        public DateTime Expiry { get; set; }
    }
}
