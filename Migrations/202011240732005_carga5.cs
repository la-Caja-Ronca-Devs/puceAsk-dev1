namespace puceAsk_dev1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class carga5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("ask.Mensaje", "fechaMensaje", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("ask.Mensaje", "fechaMensaje");
        }
    }
}
