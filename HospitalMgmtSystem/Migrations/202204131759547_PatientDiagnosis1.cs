namespace HospitalMgmtSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PatientDiagnosis1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PatientDiagnosis", "DoctorId", "dbo.Users");
            DropIndex("dbo.PatientDiagnosis", new[] { "DoctorId" });
            AddColumn("dbo.PatientDiagnosis", "PatientId", c => c.Int());
            AlterColumn("dbo.PatientDiagnosis", "DoctorId", c => c.Int());
            CreateIndex("dbo.PatientDiagnosis", "DoctorId");
            CreateIndex("dbo.PatientDiagnosis", "PatientId");
            AddForeignKey("dbo.PatientDiagnosis", "PatientId", "dbo.Users", "Id");
            AddForeignKey("dbo.PatientDiagnosis", "DoctorId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PatientDiagnosis", "DoctorId", "dbo.Users");
            DropForeignKey("dbo.PatientDiagnosis", "PatientId", "dbo.Users");
            DropIndex("dbo.PatientDiagnosis", new[] { "PatientId" });
            DropIndex("dbo.PatientDiagnosis", new[] { "DoctorId" });
            AlterColumn("dbo.PatientDiagnosis", "DoctorId", c => c.Int(nullable: false));
            DropColumn("dbo.PatientDiagnosis", "PatientId");
            CreateIndex("dbo.PatientDiagnosis", "DoctorId");
            AddForeignKey("dbo.PatientDiagnosis", "DoctorId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
