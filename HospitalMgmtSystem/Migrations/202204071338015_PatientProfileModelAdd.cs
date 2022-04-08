namespace HospitalMgmtSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PatientProfileModelAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PatientProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
                        City = c.String(),
                        Country = c.String(),
                        PostalCode = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PatientProfiles", "UserId", "dbo.Users");
            DropIndex("dbo.PatientProfiles", new[] { "UserId" });
            DropTable("dbo.PatientProfiles");
        }
    }
}
