namespace HospitalMgmtSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hellotesting123 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prescriptions", "PatientId", c => c.Int());
            CreateIndex("dbo.Prescriptions", "PatientId");
            AddForeignKey("dbo.Prescriptions", "PatientId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prescriptions", "PatientId", "dbo.Users");
            DropIndex("dbo.Prescriptions", new[] { "PatientId" });
            DropColumn("dbo.Prescriptions", "PatientId");
        }
    }
}
