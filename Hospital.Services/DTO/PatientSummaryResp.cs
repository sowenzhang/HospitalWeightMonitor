using System;

namespace Hospital.Services.DTO
{
    public class PatientSummaryResp
    {
        public long Id { get; set; }
        public DateTimeOffset RegistrationDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double WeightInKg { get; set; }
        public DateTimeOffset LastWeightRecordDate { get; set; }
    }
}