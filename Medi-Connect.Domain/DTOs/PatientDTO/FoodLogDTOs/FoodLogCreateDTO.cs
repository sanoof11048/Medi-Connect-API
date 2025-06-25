using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.PatientDTO.FoodLogDTOs
{
    public class FoodLogCreateDTO
    {
        public Guid PatientId { get; set; }
        public string? MealType { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
    }
}
