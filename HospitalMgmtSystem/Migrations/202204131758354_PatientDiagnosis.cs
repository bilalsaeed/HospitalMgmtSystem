namespace HospitalMgmtSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PatientDiagnosis : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PatientDiagnosis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Diagnosis = c.String(),
                        AddedAt = c.DateTime(nullable: false),
                        DoctorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.DoctorId, cascadeDelete: true)
                .Index(t => t.DoctorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PatientDiagnosis", "DoctorId", "dbo.Users");
            DropIndex("dbo.PatientDiagnosis", new[] { "DoctorId" });
            DropTable("dbo.PatientDiagnosis");
        }
    }
}
