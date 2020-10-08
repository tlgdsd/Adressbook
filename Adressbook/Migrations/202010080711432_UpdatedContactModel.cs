namespace Adressbook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedContactModel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Contacts", "Adress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contacts", "Adress", c => c.String());
        }
    }
}
