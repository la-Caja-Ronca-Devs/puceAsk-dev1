namespace puceAsk_dev1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Carga2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUsers", "FechaNacimiento", c => c.DateTime(nullable: false));
            AddColumn("dbo.ApplicationUsers", "Foto", c => c.Binary());
            AddColumn("dbo.ApplicationUsers", "Nombre", c => c.String(nullable: false));
            AddColumn("dbo.ApplicationUsers", "Apellido", c => c.String(nullable: false));
            AddColumn("dbo.ApplicationUsers", "Sexo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationUsers", "Sexo");
            DropColumn("dbo.ApplicationUsers", "Apellido");
            DropColumn("dbo.ApplicationUsers", "Nombre");
            DropColumn("dbo.ApplicationUsers", "Foto");
            DropColumn("dbo.ApplicationUsers", "FechaNacimiento");
        }
    }
}
