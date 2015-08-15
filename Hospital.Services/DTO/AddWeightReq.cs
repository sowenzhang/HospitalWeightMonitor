using System;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Services.DTO
{
    public class AddWeightReq
    {
        public long PatientId { get; set; }

        // this attribute will be used if we validate the incoming request in the operation 
        [Required]
        public double WeightInKg { get; set; }
        public string Notes { get; set; }

        // this value is optional 
        public DateTimeOffset? RecordDate { get; set; } 
    }
}