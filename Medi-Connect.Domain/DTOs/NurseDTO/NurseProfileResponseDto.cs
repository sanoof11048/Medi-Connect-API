using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.NurseDTO
{
    public class NurseProfileResponseDto
    {
        public Guid HomeNurseId { get; set; }
        public string Qualification { get; set; }
        public int ExperienceYears { get; set; }
        public string Bio { get; set; }
        public DateOnly DOB { get; set; }
        public DateOnly AvailableOn { get; set; }
        public bool IsAvailable { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
    }
}
