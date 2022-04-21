namespace HospitalMgmtSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prescriptionsModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Prescriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Medicine = c.String(),
                        Dosage = c.String(),
                        Quantity = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PatientMedicalRecords", "RecordType", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PatientMedicalRecords", "RecordType");
            DropTable("dbo.Prescriptions");
        }
    }
}
