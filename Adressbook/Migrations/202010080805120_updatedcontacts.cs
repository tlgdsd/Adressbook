namespace Adressbook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedcontacts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contacts", "Surname", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contacts", "Surname");
        }
    }
}
