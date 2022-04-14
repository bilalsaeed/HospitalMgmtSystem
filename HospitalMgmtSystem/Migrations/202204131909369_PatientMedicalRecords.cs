namespace HospitalMgmtSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PatientMedicalRecords : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PatientMedicalRecords",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FileName = c.String(),
                        DoctorId = c.Int(),
                        PatientId = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        LastAccessedAt = c.DateTime(nullable: false),
                        LastAccessedById = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.DoctorId)
                .ForeignKey("dbo.Users", t => t.LastAccessedById)
                .ForeignKey("dbo.Users", t => t.PatientId)
                .Index(t => t.DoctorId)
                .Index(t => t.PatientId)
                .Index(t => t.LastAccessedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PatientMedicalRecords", "PatientId", "dbo.Users");
            DropForeignKey("dbo.PatientMedicalRecords", "LastAccessedById", "dbo.Users");
            DropForeignKey("dbo.PatientMedicalRecords", "DoctorId", "dbo.Users");
            DropIndex("dbo.PatientMedicalRecords", new[] { "LastAccessedById" });
            DropIndex("dbo.PatientMedicalRecords", new[] { "PatientId" });
            DropIndex("dbo.PatientMedicalRecords", new[] { "DoctorId" });
            DropTable("dbo.PatientMedicalRecords");
        }
    }
}
