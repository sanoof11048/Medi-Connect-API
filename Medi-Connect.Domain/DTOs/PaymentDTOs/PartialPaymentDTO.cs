using Medi_Connect.Domain.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.PaymentDTOs
{
    public class PartialPaymentDTO
    {
        public Guid AssignmentId { get; set; }
        public decimal Amount { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMode Mode { get; set; }
    }

}
