namespace puceAsk_dev1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class carga3 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "ask.pregunta", name: "Categoria_CategoriaId", newName: "CategoriaId");
            RenameIndex(table: "ask.pregunta", name: "IX_Categoria_CategoriaId", newName: "IX_CategoriaId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "ask.pregunta", name: "IX_CategoriaId", newName: "IX_Categoria_CategoriaId");
            RenameColumn(table: "ask.pregunta", name: "CategoriaId", newName: "Categoria_CategoriaId");
        }
    }
}
