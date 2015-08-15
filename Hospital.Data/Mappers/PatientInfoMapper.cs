using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Hospital.Data.Entities;

namespace Hospital.Data.Mappers
{
    public class PatientInfoMapper : EntityTypeConfiguration<PatientInfo>
    {
        public PatientInfoMapper()
        {
            HasKey(s => s.Id);
            Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
            Property(s => s.FirstName).IsRequired();
            Property(s => s.FirstName).HasMaxLength(150);
            Property(s => s.LastName).IsRequired();
            Property(s => s.LastName).HasMaxLength(150);
        }
    }
}