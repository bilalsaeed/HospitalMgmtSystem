namespace HospitalMgmtSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testtablecreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TestTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        Name = c.String(),
                        FatherName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TestTables");
        }
    }
}
