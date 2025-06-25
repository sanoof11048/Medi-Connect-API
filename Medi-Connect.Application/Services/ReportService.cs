using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Domain.DTOs.ReportDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Services
{
    public class ReportService : IReportService
    {
        public async Task<PatientReportDTO> GenerateReportPdf()
        {
            return new PatientReportDTO();
        }
    }
}
