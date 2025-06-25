using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.NurseDTO
{
    public class MarkPaymentDTO
    {
        public Guid AssignmentId { get; set; }
        public bool IsToNurse { get; set; }
    }
}
