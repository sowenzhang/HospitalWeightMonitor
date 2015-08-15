using System;

namespace Hospital.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Contexts.HospitalContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            
            string path = @"E:\Projects\BostonChildrenHospital\Database";
            
            // TODO: this is not right, the migration executable location is a temp folder
            if (executable.Contains("Hospital")) 
                path = (System.IO.Path.GetDirectoryName(executable)) + "..\\Database";
            
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
        }

        protected override void Seed(Contexts.HospitalContext context)
        {

        }
    }
}
