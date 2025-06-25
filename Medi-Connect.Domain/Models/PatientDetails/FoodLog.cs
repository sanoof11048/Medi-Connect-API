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
    public class FoodLog : BaseEntity
    {
        [Key]
        public Guid MealId { get; set; }

        [Required]
        public Guid PatientId { get; set; }

        [Required]
        [StringLength(100)]
        public string MealType { get; set; } // e.g., "Breakfast", "Lunch", "Dinner"

        [Required]
        [StringLength(500)]
        public string Description { get; set; } // Food items, calories, etc.

        [StringLength(200)]
        public string Notes { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }


    }
}
