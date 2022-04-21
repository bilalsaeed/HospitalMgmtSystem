namespace HospitalMgmtSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class helloworld : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PatientMedicalRecords", "DoctorId", "dbo.Users");
            DropIndex("dbo.PatientMedicalRecords", new[] { "DoctorId" });
            DropColumn("dbo.PatientMedicalRecords", "DoctorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PatientMedicalRecords", "DoctorId", c => c.Int());
            CreateIndex("dbo.PatientMedicalRecords", "DoctorId");
            AddForeignKey("dbo.PatientMedicalRecords", "DoctorId", "dbo.Users", "Id");
        }
    }
}
