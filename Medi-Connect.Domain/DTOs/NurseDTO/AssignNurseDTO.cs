using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.NurseDTO
{
    public class AssignNurseDTO
    {
        public Guid RequestId { get; set; }
        public Guid NurseId { get; set; }
    }

}
