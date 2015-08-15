using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace Hospital.Data.Entities
{
    public class Patient : Entity
    {
        public Patient()
        {
            PatientInfo = new List<PatientInfo>();
            Weights = new HashSet<PatientWeight>();
        }

        public long Id { get; set; }
        public DateTimeOffset RegistrationTime { get; set; }

        public ICollection<PatientInfo> PatientInfo { get; set; }
        public ICollection<PatientWeight> Weights { get; set; }
    }
}