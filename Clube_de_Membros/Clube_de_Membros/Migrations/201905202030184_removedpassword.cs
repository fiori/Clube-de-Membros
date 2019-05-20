namespace Clube_de_Membros.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedpassword : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Members", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Members", "Password", c => c.String(nullable: false));
        }
    }
}
