namespace Hospital.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PatientInfoes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 150),
                        LastName = c.String(nullable: false, maxLength: 150),
                        RecordDate = c.DateTimeOffset(nullable: false, precision: 7),
                        PatientId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patient", t => t.PatientId, cascadeDelete: true)
                .Index(t => t.PatientId);
            
            CreateTable(
                "dbo.Patient",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RegistrationTime = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PatientWeight",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        WeightInKg = c.Double(nullable: false),
                        RecordDate = c.DateTimeOffset(nullable: false, precision: 7),
                        Notes = c.String(),
                        PatientId = c.Long(nullable: false),
                        ActiveState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patient", t => t.PatientId, cascadeDelete: true)
                .Index(t => t.PatientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PatientWeight", "PatientId", "dbo.Patient");
            DropForeignKey("dbo.PatientInfoes", "PatientId", "dbo.Patient");
            DropIndex("dbo.PatientWeight", new[] { "PatientId" });
            DropIndex("dbo.PatientInfoes", new[] { "PatientId" });
            DropTable("dbo.PatientWeight");
            DropTable("dbo.Patient");
            DropTable("dbo.PatientInfoes");
        }
    }
}
