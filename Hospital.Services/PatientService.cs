using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Hospital.Data.Entities;
using Hospital.Data.Enums;
using Hospital.Services.DTO;
using Hospital.Services.DTO.Interim;
using Hospital.Services.Exceptions;
using Hospital.Services.Interfaces;
using Hospital.Services.Translators;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using Service.Pattern;

namespace Hospital.Services
{
    public class PatientService : Service<Patient>, IPatientService
    {
        private readonly IRepositoryAsync<PatientInfo> _infoRepository;
        private readonly IRepositoryAsync<PatientWeight> _weightRepostiroy;

        public PatientService(IRepositoryAsync<Patient> repository,
            IRepositoryAsync<PatientInfo> infoRepository,
            IRepositoryAsync<PatientWeight> weightRepostiroy)
            : base(repository)
        {
            _infoRepository = infoRepository;
            _weightRepostiroy = weightRepostiroy;
        }

        public PatientSummaryResp AddPatient(AddPatientReq req)
        {
            GuardPatientName(req);

            var newPatient = new Patient
            {
                RegistrationTime = DateTimeOffset.Now,
                ObjectState = ObjectState.Added
            };

            var info = new PatientInfo
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                ObjectState = ObjectState.Added
            };
            newPatient.PatientInfo.Add(info);

            Insert(newPatient);

            return newPatient.ToPatientSummary();
        }

        public async Task<PatientSummaryResp> UpdatePatientNameAsync(UpdatePatientNameReq req)
        {
            GuardPatientName(req);

            // we don't actually "update", we only insert in this solution (fact-based db design) 
            IQueryable<PatientOverviewQuery> query = from p in Queryable().Include(a => a.Weights)
                where p.Id == req.PatientId
                select new PatientOverviewQuery
                {
                    Patient = p,
                    LastWeight =
                        p.Weights.Where(w => w.ActiveState == ActiveState.Active)
                            .OrderByDescending(w => w.RecordDate)
                            .Take(1)
                };

            PatientOverviewQuery found = await query.FirstOrDefaultAsync();
            if (found == null)
                throw new RootObjectNotFoundException("Patient Id " + req.PatientId + " cannot be found");

            var newInfo = new PatientInfo
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                PatientId = req.PatientId,
                ObjectState = ObjectState.Added
            };
            _infoRepository.Insert(newInfo);

            found.LastInfo = new List<PatientInfo> {newInfo};
            return found.ToPatientSummary();
        }

        public async Task<PatientSummaryResp> AddWeightReqAsync(AddWeightReq req)
        {
            IQueryable<PatientOverviewQuery> query = from p in Queryable().Include(a => a.PatientInfo)
                where p.Id == req.PatientId
                select new PatientOverviewQuery
                {
                    Patient = p,
                    LastInfo = p.PatientInfo.OrderByDescending(a => a.Id).Take(1)
                };
            PatientOverviewQuery patient = await query.FirstOrDefaultAsync();

            if (patient == null)
                throw new RootObjectNotFoundException("Patient Id " + req.PatientId + " cannot be found");

            DateTimeOffset recordDate = DateTimeOffset.Now;
            if (req.RecordDate.HasValue)
                recordDate = req.RecordDate.Value;

            var newWeight = new PatientWeight
            {
                Notes = req.Notes,
                RecordDate = recordDate,
                ObjectState = ObjectState.Added,
                PatientId = req.PatientId,
                WeightInKg = req.WeightInKg
            };

            _weightRepostiroy.Insert(newWeight);
            patient.LastWeight = new List<PatientWeight> {newWeight};
            return patient.ToPatientSummary();
        }

        public async Task<IList<PatientSummaryResp>> GetAllPatientsAsync()
        {
            // TODO: I don't think this is the best efficent query, might be better to profile the generated query just double check
            // the ony concern I have is: I am trying to get only 1 child, but because of using Include, will I still get the entire child collection? 
            IQueryable<PatientOverviewQuery> query = from p in Queryable()
                .Include(a => a.Weights)
                .Include(a => a.PatientInfo)
                select new PatientOverviewQuery
                {
                    Patient = p,
                    LastWeight =
                        p.Weights.Where(a => a.ActiveState == ActiveState.Active)
                            .OrderByDescending(a => a.RecordDate)
                            .Take(1),
                    LastInfo = p.PatientInfo.OrderByDescending(a => a.Id).Take(1)
                };

            List<PatientOverviewQuery> results = await query.ToListAsync();
            return results.Select(a => a.ToPatientSummary()).ToList();
        }

        public async Task<PatientSummaryResp> GetPatientSummaryAsync(long id)
        {
            IQueryable<PatientOverviewQuery> query =
                from p in
                    Queryable()
                        .Include(a => a.PatientInfo)
                        .Include(a => a.Weights)
                where p.Id == id
                select new PatientOverviewQuery
                {
                    Patient = p,
                    LastWeight = p.Weights.Where(a => a.ActiveState == ActiveState.Active).OrderByDescending(w => w.Id).Take(1),
                    LastInfo = p.PatientInfo.OrderByDescending(a => a.Id).Take(1)
                };

            PatientOverviewQuery found = await query.FirstOrDefaultAsync();
            if (found == null)
                throw new RootObjectNotFoundException("Patient Id " + id + " cannot be found");

            return found.ToPatientSummary();
        }

        public async Task<IList<PatientWeightResp>> GetPatientWeightHistoriesAsync(long id)
        {
            IQueryable<PatientWeightResp> query = from w in _weightRepostiroy.Queryable()
                where w.PatientId == id && w.ActiveState == ActiveState.Active
                orderby w.RecordDate descending
                // we show the most recent one first
                select new PatientWeightResp
                {
                    Id = w.Id,
                    Notes = w.Notes,
                    RecordDate = w.RecordDate,
                    WeightInKg = w.WeightInKg
                };

            List<PatientWeightResp> found = await query.ToListAsync();
            return found;
        }

        public async Task<bool> RemoveWeightAsync(long id)
        {
            IQueryable<PatientWeight> query = from w in _weightRepostiroy.Queryable() where w.Id == id select w;
            PatientWeight found = await query.FirstOrDefaultAsync();
            if (found == null)
                throw new RootObjectNotFoundException("Patient weight record " + id + " is not found");

            found.ActiveState = ActiveState.Inactive;
            found.ObjectState = ObjectState.Modified;
            found.RecordDate = DateTimeOffset.Now;
            _weightRepostiroy.Update(found);
            return true;
        }

        private static void GuardPatientName(BasePatientReq req)
        {
            if (string.IsNullOrEmpty(req.FirstName) || string.IsNullOrEmpty(req.LastName))
            {
                throw new BusinessRuleViolationException("Patient must have both First Name and Last Name");
            }
        }
    }
}