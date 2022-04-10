namespace HospitalMgmtSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewfieldsinUsermodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Nationality", c => c.String());
            AddColumn("dbo.Users", "Ethnicity", c => c.String());
            AddColumn("dbo.Users", "Address", c => c.String());
            AddColumn("dbo.Users", "Occupation", c => c.String());
            AddColumn("dbo.Users", "AlternatePhoneNumber", c => c.String());
            AddColumn("dbo.Users", "EmailAddress", c => c.String());
            AddColumn("dbo.Users", "PreviousGPName", c => c.String());
            AddColumn("dbo.Users", "GPAddress", c => c.String());
            AddColumn("dbo.Users", "GPPostalCode", c => c.String());
            AddColumn("dbo.Users", "DoctorName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "DoctorName");
            DropColumn("dbo.Users", "GPPostalCode");
            DropColumn("dbo.Users", "GPAddress");
            DropColumn("dbo.Users", "PreviousGPName");
            DropColumn("dbo.Users", "EmailAddress");
            DropColumn("dbo.Users", "AlternatePhoneNumber");
            DropColumn("dbo.Users", "Occupation");
            DropColumn("dbo.Users", "Address");
            DropColumn("dbo.Users", "Ethnicity");
            DropColumn("dbo.Users", "Nationality");
        }
    }
}
