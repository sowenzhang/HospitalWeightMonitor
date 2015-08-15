using System.Data.Entity;
using Hospital.Data.Constants;
using Hospital.Data.Entities;
using Hospital.Data.Mappers;
using Repository.Pattern.Ef6;

namespace Hospital.Data.Contexts
{
    public class HospitalContext: DataContext, IHospitalContext
    {
        public HospitalContext() : base(HospitalEnvironment.ConnectionString)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<HospitalContext>());

            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientInfo> PatientInfos { get; set; }
        public DbSet<PatientWeight> PatientWeights { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PatientMapper());
            modelBuilder.Configurations.Add(new PatientInfoMapper());
            modelBuilder.Configurations.Add(new PatientWeightMapper());

            base.OnModelCreating(modelBuilder);
        }
    }
}