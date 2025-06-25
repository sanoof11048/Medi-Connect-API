using Medi_Connect.Domain.Models.Other;
using Medi_Connect.Domain.Models.PatientDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.NurseDTO
{
    public class NurseRequestDTO
    {
        public Guid PatientId { get; set; }
        public DateTime StartDate { get; set; }
        public int DurationDays { get; set; }
        public string Requirements { get; set; }
        public CareServiceType CareType { get; set; }

    }
}
