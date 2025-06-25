using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.Common
{
    public abstract class BaseEntity
    {
        [Required]
        public Guid  CreatedBy { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
