namespace HospitalMgmtSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HelloPeter : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "DateOfBirth", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "DateOfBirth", c => c.DateTime(nullable: false));
        }
    }
}
