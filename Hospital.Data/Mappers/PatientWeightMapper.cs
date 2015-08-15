using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using Hospital.Data.Entities;

namespace Hospital.Data.Mappers
{
    public class PatientWeightMapper : EntityTypeConfiguration<PatientWeight>
    {
        public PatientWeightMapper()
        {
            ToTable("PatientWeight");
            HasKey(s => s.Id);
            Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
            Property(s => s.WeightInKg).IsRequired();
            // Property(s => s.RecordDate).HasColumnAnnotation("IDX_WEIGHT_DATE", new IndexAnnotation(new IndexAttribute("IDX_WEIGHT_DATE")));
        }
    }
}