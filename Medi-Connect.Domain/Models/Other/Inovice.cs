using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.Models.Other
{
    public class Invoice 
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AssignmentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidOn { get; set; }
        public bool IsPaidToNurse { get; set; }

        public NurseAssignment Assignment { get; set; }
    }

}
