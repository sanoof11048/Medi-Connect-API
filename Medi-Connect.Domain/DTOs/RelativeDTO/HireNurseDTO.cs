using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.RelativeDTO
{
    public class HireNurseDTO
    {
        public Guid PatientId { get; set; }
        public Guid NurseId { get; set; }
    }
}
