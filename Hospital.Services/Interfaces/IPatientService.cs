using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Data.Entities;
using Hospital.Services.DTO;
using Service.Pattern;

namespace Hospital.Services.Interfaces
{
    public interface IPatientService : IService<Patient>
    {
        PatientSummaryResp AddPatient(AddPatientReq req);
        Task<PatientSummaryResp> UpdatePatientNameAsync(UpdatePatientNameReq req);
        Task<PatientSummaryResp> AddWeightReqAsync(AddWeightReq req);

        Task<IList<PatientSummaryResp>> GetAllPatientsAsync();
        Task<PatientSummaryResp> GetPatientSummaryAsync(long id);
        Task<IList<PatientWeightResp>> GetPatientWeightHistoriesAsync(long id);

        Task<bool> RemoveWeightAsync(long id);
    }
}