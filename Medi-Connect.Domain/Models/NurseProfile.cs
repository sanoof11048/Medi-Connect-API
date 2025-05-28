using Medi_Connect.Domain.DTOs.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.Models
{
    public class NurseProfile
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string Qualification { get; set; }
        public int ExperienceYears { get; set; }
        public List<string>? CertificatesUrls { get; set; }
        public string Bio { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
    }

}
