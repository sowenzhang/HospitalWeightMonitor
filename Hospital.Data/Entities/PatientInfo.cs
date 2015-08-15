using System;
using Newtonsoft.Json;
using Repository.Pattern.Ef6;

namespace Hospital.Data.Entities
{
    public class PatientInfo:Entity
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset RecordDate { get; set; }

        public long PatientId { get; set; }

        [JsonIgnore]
        public virtual Patient Patient { get; set; }
    }
}