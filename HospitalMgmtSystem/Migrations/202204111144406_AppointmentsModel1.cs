namespace HospitalMgmtSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppointmentsModel1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Appointments", "AppointmentTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Appointments", "Duration", c => c.Int());
            AlterColumn("dbo.Appointments", "Bill", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Appointments", "Bill", c => c.Int(nullable: false));
            AlterColumn("dbo.Appointments", "Duration", c => c.Int(nullable: false));
            AlterColumn("dbo.Appointments", "AppointmentTime", c => c.String());
        }
    }
}
