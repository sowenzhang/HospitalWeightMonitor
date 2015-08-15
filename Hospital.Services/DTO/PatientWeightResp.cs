using System;

namespace Hospital.Services.DTO
{
    public class PatientWeightResp
    {
        public long Id { get; set; }
        public double WeightInKg { get; set; }
        public DateTimeOffset RecordDate { get; set; }
        public string Notes { get; set; }
    }
}