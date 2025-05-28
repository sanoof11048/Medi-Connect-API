using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.Models
{
    public class PatientProfile
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string PhotoUrl { get; set; }

        public string SecretCode { get; set; } // Used for linking
        public Guid CreatedByRelativeId { get; set; }   // Added by
        public User Relative { get; set; }

        public ICollection<RelativeProfile> LinkedRelatives { get; set; } = new List<RelativeProfile>();
    }
}
