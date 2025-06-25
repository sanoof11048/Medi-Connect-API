using Medi_Connect.Domain.DTOs.Auth.OTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Infrastructure.Context
{
    public static class InMemoryOtpStore
    {
        public static Dictionary<string, OtpStore> OtpDict = new();
    }
}
