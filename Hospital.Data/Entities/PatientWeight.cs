using System;
using Hospital.Data.Enums;
using Newtonsoft.Json;
using Repository.Pattern.Ef6;

namespace Hospital.Data.Entities
{
    public class PatientWeight:Entity
    {
        public PatientWeight()
        {
            ActiveState = ActiveState.Active;
        }

        public long Id { get; set; }
        public double WeightInKg { get; set; }
        public DateTimeOffset RecordDate { get; set; }
        public string Notes { get; set; }
        public long PatientId { get; set; }

        public ActiveState ActiveState { get; set; }

        [JsonIgnore]
        public virtual Patient Patient { get; set; }
    }
}