namespace puceAsk_dev1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class load4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("ask.Categoria", "RowVersion");
            DropColumn("ask.pregunta", "RowVersion");
            DropColumn("ask.Respuestas", "RowVersion");
        }
        
        public override void Down()
        {
            AddColumn("ask.Respuestas", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("ask.pregunta", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("ask.Categoria", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
    }
}
