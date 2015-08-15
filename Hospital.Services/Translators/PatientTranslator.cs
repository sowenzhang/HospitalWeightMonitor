using System.Linq;
using Hospital.Data.Entities;
using Hospital.Services.DTO;
using Hospital.Services.DTO.Interim;

namespace Hospital.Services.Translators
{
    public static class PatientTranslator
    {
        public static PatientSummaryResp ToPatientSummary(this Patient patient)
        {
            if (patient.PatientInfo == null || !patient.PatientInfo.Any())
                return null;

            PatientSummaryResp retval = new PatientSummaryResp();
            retval.Id = patient.Id;
            retval.FirstName = patient.PatientInfo.Last().FirstName;
            retval.LastName = patient.PatientInfo.Last().LastName;
            retval.RegistrationDate = patient.RegistrationTime;

            if (patient.Weights.Any())
            {
                retval.WeightInKg = patient.Weights.Last().WeightInKg;
                retval.LastWeightRecordDate = patient.Weights.Last().RecordDate;
            }

            return retval;
        }

        internal static PatientSummaryResp ToPatientSummary(this PatientOverviewQuery patient)
        {
            PatientSummaryResp retval = new PatientSummaryResp();
            retval.Id = patient.Patient.Id;
            retval.RegistrationDate = patient.Patient.RegistrationTime;

            if (patient.LastInfo != null && patient.LastInfo.Any())
            {
                retval.FirstName = patient.LastInfo.First().FirstName;
                retval.LastName = patient.LastInfo.First().LastName;
            }

            if (patient.LastWeight  != null && patient.LastWeight.Any())
            {
                retval.WeightInKg = patient.LastWeight.First().WeightInKg;
                retval.LastWeightRecordDate = patient.LastWeight.First().RecordDate;
            }

            return retval;
        } 
    }
}