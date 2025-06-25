using Medi_Connect.Domain.DTOs.ReportDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.ISerives
{
    public interface IReportService
    {
        Task<PatientReportDTO> GenerateReportPdf(); 
    }
}
