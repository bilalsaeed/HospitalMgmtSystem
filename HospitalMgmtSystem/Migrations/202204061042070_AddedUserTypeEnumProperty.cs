namespace HospitalMgmtSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserTypeEnumProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "UserType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "UserType");
        }
    }
}
