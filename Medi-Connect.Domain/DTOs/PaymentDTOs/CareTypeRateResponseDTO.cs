using Medi_Connect.Domain.Models.PatientDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.DTOs.PaymentDTOs
{
    public class CareTypeRateResponseDTO
    {     
        public Guid Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CareServiceType ServiceType { get; set; }
        public string? Description { get; set; }
        public decimal FixedPayment { get; set; }
        public decimal? OfferPrice { get; set; }
        public List<string>? Features { get; set; }

    }

}
