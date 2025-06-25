using Medi_Connect.Domain.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.NurseDTO
{
    public class NurseAssignmentResponseDTO
    {
        public Guid Id { get; set; }

        public string PatientName { get; set; }

        public string NurseName { get; set; }

        public string RequestedBy { get; set; }

        public DateTime StartDate { get; set; }

        public int DurationDays { get; set; }
        public decimal PaymentAmount { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AssignmentStatus Status { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus PaymentToAdmin { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus PaymentToNurse { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
