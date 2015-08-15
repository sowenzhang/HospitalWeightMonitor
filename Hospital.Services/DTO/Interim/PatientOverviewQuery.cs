using System.Collections.Generic;
using Hospital.Data.Entities;

namespace Hospital.Services.DTO.Interim
{
    // an interim entity for the sake of translation and reuse
    internal class PatientOverviewQuery
    {
        internal Patient Patient { get; set; }
        internal IEnumerable<PatientWeight> LastWeight { get; set; }
        internal IEnumerable<PatientInfo> LastInfo { get; set; }
    }
}