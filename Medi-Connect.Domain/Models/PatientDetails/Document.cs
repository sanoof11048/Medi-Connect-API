using Medi_Connect.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medi_Connect.Domain.Common;

namespace Medi_Connect.Domain.Models.PatientDetails
{
    public class Document : BaseEntity
    {
        [Key]
        public Guid DocumentId { get; set; }

        [Required]
        public Guid PatientId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(200)]
        public string FilePath { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
    }
}
