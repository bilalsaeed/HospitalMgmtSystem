namespace HospitalMgmtSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PatientMedicalRecords1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PatientMedicalRecords", "FileExtension", c => c.String());
            AddColumn("dbo.PatientMedicalRecords", "CreatedById", c => c.Int());
            CreateIndex("dbo.PatientMedicalRecords", "CreatedById");
            AddForeignKey("dbo.PatientMedicalRecords", "CreatedById", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PatientMedicalRecords", "CreatedById", "dbo.Users");
            DropIndex("dbo.PatientMedicalRecords", new[] { "CreatedById" });
            DropColumn("dbo.PatientMedicalRecords", "CreatedById");
            DropColumn("dbo.PatientMedicalRecords", "FileExtension");
        }
    }
}
