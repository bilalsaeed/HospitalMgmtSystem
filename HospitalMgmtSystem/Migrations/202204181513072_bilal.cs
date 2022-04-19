namespace HospitalMgmtSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bilal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "Room", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "Room");
        }
    }
}
