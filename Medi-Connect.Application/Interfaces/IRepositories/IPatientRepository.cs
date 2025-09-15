using Medi_Connect.Domain.Models.PatientDetails;
using Medi_Connect.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medi_Connect.Application.Interfaces.IRepositories
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient?> GetPatientById(Guid id);
        Task AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
        Task DeleteAsync(Guid id);

        Task<ICollection<Patient>> GetPatientsByRelativeId(Guid relativeId);
        Task<User?> GetRelativeByIdAsync(Guid relativeId);

        Task<IEnumerable<Vital>> GetVitalsByPatientIdAsync(Guid id);
        Task<IEnumerable<FoodLog>> GetFoodLogsByPatientIdAsync(Guid patientId);
        Task<IEnumerable<MedicationLog>> GetMedicationLogsByPatientIdAsync(Guid patientId);
        Task<IEnumerable<Patient>> GetPatientReports(int fromAge, int toAge, CareServiceType servicetype, string name);

    }
}
