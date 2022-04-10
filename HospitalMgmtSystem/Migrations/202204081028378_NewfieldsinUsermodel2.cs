namespace HospitalMgmtSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewfieldsinUsermodel2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ContactNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ContactNumber");
        }
    }
}
