using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.UserDTOs
{
    public class NurseProfileCreateDTO
    {
        public string Qualification { get; set; }
        public int ExperienceYears { get; set; }
        public List<IFormFile>? CertificateFiles { get; set; }
        public List<string>? CertificatesUrls { get; set; }
        public string Bio { get; set; }
        public DateOnly Availbility { get; set; }

    }
}
