using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Hospital.Data.Entities;

namespace Hospital.Data.Mappers
{
    public class PatientMapper: EntityTypeConfiguration<Patient>
    {
        public PatientMapper()
        {
            ToTable("Patient");
            HasKey(s => s.Id);
            Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(s => s.RegistrationTime).IsRequired();
            HasMany(s=>s.PatientInfo).WithRequired().HasForeignKey(s=>s.PatientId).WillCascadeOnDelete(true);
            HasMany(s=>s.Weights).WithRequired().HasForeignKey(s=>s.PatientId).WillCascadeOnDelete(true);
        }
         
    }
}