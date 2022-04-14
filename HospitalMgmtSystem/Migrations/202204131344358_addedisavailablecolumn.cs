namespace HospitalMgmtSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedisavailablecolumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "IsAvailable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "IsAvailable");
        }
    }
}
