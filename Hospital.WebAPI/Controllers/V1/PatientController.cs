using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Web.Http;
using Hospital.Services.DTO;
using Hospital.Services.Interfaces;
using Hospital.Web.Common.Routing;
using NLog;
using Repository.Pattern.UnitOfWork;

namespace Hospital.WebAPI.Controllers.V1
{
    [ApiVersionRoutePrefix("patient")]
    public class PatientController : BaseController
    {
        private readonly IPatientService _patientService;
        private readonly Logger _logger;

        public PatientController(
            IPatientService patientService,
            IUnitOfWorkAsync unitOfWork) : base(unitOfWork)
        {
            _patientService = patientService;
             _logger = LogManager.GetLogger(GetType().Name);
        }

        #region HttpGet

        [Route("{id}")]
        [HttpGet]
        public async Task<PatientSummaryResp> GetPatient(long id)
        {
            PatientSummaryResp result = await _patientService.GetPatientSummaryAsync(id);
            return result;
        }

        [Route("")]
        [HttpGet]
        public async Task<ListResp<PatientSummaryResp>> GetPatients()
        {
            IList<PatientSummaryResp> result = await _patientService.GetAllPatientsAsync();
            return new ListResp<PatientSummaryResp>(result);
        }

        [Route("{id}/weight")]
        public async Task<ListResp<PatientWeightResp>> GetPaitentWeightRecords(long id)
        {
            IList<PatientWeightResp> result = await _patientService.GetPatientWeightHistoriesAsync(id);
            return new ListResp<PatientWeightResp>(result);
        }
        #endregion

        #region HttpPost

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> CreatePatient(AddPatientReq request)
        {
            var result = _patientService.AddPatient(request);

            if (result != null)
            {
                try
                {
                    await UnitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    _logger.Error(ex, "Creating patient failed");
                    throw;
                }
            }

            return Ok(result);
        }

        [Route("{id}/weight")]
        [HttpPost]
        public async Task<IHttpActionResult> AddPatientWeight(long id, AddWeightReq request)
        {
            if (id != request.PatientId)
            {
                return BadRequest("Requesting incosnsitent patient Id");
            }

            PatientSummaryResp result =  await _patientService.AddWeightReqAsync(request);

            if (result != null)
            {
                try
                {
                    await UnitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    _logger.Error(ex, "Adding patient weight failed");
                    throw;
                }
            }

            return Ok(result);
        }
        #endregion

        #region HttpPut/Delete

        [Route("{id}/name")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdatePatientName(long id, UpdatePatientNameReq request)
        {
            if (id != request.PatientId)
            {
                return BadRequest("Requesting incosnsitent patient Id");
            }

            PatientSummaryResp result = await _patientService.UpdatePatientNameAsync(request);
            if (result != null)
            {
                try
                {
                    await UnitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    _logger.Error(ex, "Updating patient name failed");
                    throw;
                }
            }

            return Updated(result);
        }

        [Route("weight/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeletePatientWeight(long id)
        {
            bool result = await _patientService.RemoveWeightAsync(id);
            if (result)
            {
                try
                {
                    await UnitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    _logger.Error(ex, "Removing patient weight failed");
                    throw;
                }
            }

            return Ok();
        }

        // this is only here for testing purpose 
        [Route("{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeletePatient(long id)
        {
            bool result = await _patientService.DeleteAsync(id);
            if (result)
            {
                try
                {
                    await UnitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    _logger.Error(ex, "Removing patient failed");
                    throw;
                }
            }

            return Ok();
        }
        #endregion
    }
}