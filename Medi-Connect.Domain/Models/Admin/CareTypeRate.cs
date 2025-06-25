using Medi_Connect.Domain.Common;
using Medi_Connect.Domain.Models.PatientDetails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Medi_Connect.Domain.Models.Admin
{
    public class CareTypeRate : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CareServiceType ServiceType { get; set; }

        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal FixedPayment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OfferPrice { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? FeaturesSerialized { get; set; }

        [NotMapped]
        public List<string>? Features
        {
            get => string.IsNullOrWhiteSpace(FeaturesSerialized)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(FeaturesSerialized);
            set => FeaturesSerialized = JsonSerializer.Serialize(value);
        }
    }
}
