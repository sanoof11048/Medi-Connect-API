using Medi_Connect.Domain.Common;
using Medi_Connect.Domain.Models.PatientDetails;
using Medi_Connect.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Medi_Connect.Domain.Models.Other
{
    public class NurseRequest : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

        public string? Requirements { get; set; }
        public DateTime StartDate { get; set; }

        public int DurationDays { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        public CareServiceType CareType { get; set; }

        public Guid RequestedById { get; set; }
        public User RequestedBy { get; set; } = null!;

    }


    public enum RequestStatus
    {
        Pending,
        Approved,
        Rejected,
        Urgent
    }
    public enum AssignmentStatus
    {
        Active,
        Completed,
        Cancelled
    }
}
