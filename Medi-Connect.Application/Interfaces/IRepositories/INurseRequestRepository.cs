using Medi_Connect.Domain.DTOs.NurseDTO;
using Medi_Connect.Domain.Models.Admin;
using Medi_Connect.Domain.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.IRepositories
{
    public interface INurseRequestRepository
    {
        Task<NurseRequest> CreateAsync(NurseRequest request);
        Task<NurseRequest?> GetByIdAsync(Guid requestId);
        Task AssignNurse(NurseAssignment assignment);
        Task<NurseAssignment> GetByIdAssignment(Guid Id);
        Task UpdateAssignment(NurseAssignment assignment);
        Task<IEnumerable<NurseAssignment>> GetAllAssignmentsAsync();
        Task<IEnumerable<NurseAssignment>> GetAssignmentsByRelativeId(Guid relativeId);
        Task<IEnumerable<NurseRequest>> GetAllRequestsAsync();
        Task<NurseRequest?> GetByIdWithRelationsAsync(Guid id);
    }
}
