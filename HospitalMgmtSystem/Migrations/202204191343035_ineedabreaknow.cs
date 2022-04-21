namespace HospitalMgmtSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ineedabreaknow : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ConnectPost", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "ConnectSMS", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "ConnectEmail", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "TermsNCondition", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "CollectInformation", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "RelevantInformation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "RelevantInformation");
            DropColumn("dbo.Users", "CollectInformation");
            DropColumn("dbo.Users", "TermsNCondition");
            DropColumn("dbo.Users", "ConnectEmail");
            DropColumn("dbo.Users", "ConnectSMS");
            DropColumn("dbo.Users", "ConnectPost");
        }
    }
}
