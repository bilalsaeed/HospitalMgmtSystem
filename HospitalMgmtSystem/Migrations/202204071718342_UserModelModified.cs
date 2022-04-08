namespace HospitalMgmtSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserModelModified : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PatientProfiles", "UserId", "dbo.Users");
            DropIndex("dbo.PatientProfiles", new[] { "UserId" });
            AddColumn("dbo.Users", "DateOfBirth", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "City", c => c.String());
            AddColumn("dbo.Users", "Country", c => c.String());
            AddColumn("dbo.Users", "PostalCode", c => c.String());
            DropTable("dbo.PatientProfiles");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Users", "PostalCode");
            DropColumn("dbo.Users", "Country");
            DropColumn("dbo.Users", "City");
            DropColumn("dbo.Users", "DateOfBirth");
            CreateIndex("dbo.PatientProfiles", "UserId");
            AddForeignKey("dbo.PatientProfiles", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
