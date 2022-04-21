namespace HospitalMgmtSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class somemodelchanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prescriptions", "AddedById", c => c.Int(nullable: false));
            AddColumn("dbo.Prescriptions", "AddedAt", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Prescriptions", "AddedById");
            AddForeignKey("dbo.Prescriptions", "AddedById", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prescriptions", "AddedById", "dbo.Users");
            DropIndex("dbo.Prescriptions", new[] { "AddedById" });
            DropColumn("dbo.Prescriptions", "AddedAt");
            DropColumn("dbo.Prescriptions", "AddedById");
        }
    }
}
