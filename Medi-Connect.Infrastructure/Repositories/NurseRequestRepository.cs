using AutoMapper.QueryableExtensions;
using Azure.Core;
using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Domain.DTOs.NurseDTO;
using Medi_Connect.Domain.Models.Other;
using Medi_Connect.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

public class NurseRequestRepository : INurseRequestRepository
{
    private readonly AppDbContext _context;

    public NurseRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<NurseRequest> CreateAsync(NurseRequest request)
    {
        _context.NurseRequests.Add(request);
        await _context.SaveChangesAsync();
        return request;
    }

    public async Task<IEnumerable<NurseRequest>> GetRequestsByPatientId(Guid patientId)
    {
        return await _context.NurseRequests
            .Include(x => x.Patient)
            .Include(x => x.RequestedBy)
            .Where(r => r.PatientId == patientId)
            .ToListAsync();
    }

    public async Task<NurseRequest?> GetByIdAsync(Guid requestId)
    {
        return await _context.NurseRequests
            .Include(r => r.Patient)
            .Include(r => r.RequestedBy)
            .FirstOrDefaultAsync(r => r.Id == requestId);
    }
    
    public async Task AssignNurse(NurseAssignment assignment)
    {
        _context.NurseAssignments.Add(assignment);
        await _context.SaveChangesAsync();
    }

    public async Task<NurseAssignment?> GetByIdAssignment(Guid Id)
    {
        return await _context.NurseAssignments.Include(n => n.Patient)
        .FirstOrDefaultAsync(r => r.Id == Id);
    }
    public async Task UpdateAssignment(NurseAssignment assignment)
    {
        _context.NurseAssignments.UpdateRange(assignment);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<NurseAssignment>> GetAllAssignmentsAsync()
    {
            var assignments = await _context.NurseAssignments
                .Include(x => x.Patient)
                .Include(x => x.Nurse).ThenInclude(xt=>xt.User)
                .Include(x => x.Request)
                    .ThenInclude(r => r.RequestedBy)
                .ToListAsync();

        return assignments;
    }

    public async Task<IEnumerable<NurseAssignment>> GetAssignmentsByRelativeId(Guid relativeId)
    {
        return await _context.NurseAssignments
            .Include(a => a.Nurse).ThenInclude(n => n.User)
            .Include(a => a.Patient)
            .Where(a => a.Patient.RelativeId == relativeId)
            .ToListAsync();
    }

    public async Task<IEnumerable<NurseRequest>> GetAllRequestsAsync()
    {
        return await _context.NurseRequests
            .Include(n => n.Patient)
            .Include(n => n.RequestedBy)
            .ToListAsync();
    }

    public async Task<NurseRequest?> GetByIdWithRelationsAsync(Guid id)
    {
        return await _context.NurseRequests
            .Include(n => n.Patient)
            .Include(n => n.RequestedBy)
            .FirstOrDefaultAsync(n => n.Id == id);
    }



}
