namespace Clube_de_Membros.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpateMembers : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Members", "Image", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Members", "Image", c => c.Binary());
        }
    }
}
