namespace puceAsk_dev1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class carga3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ask.Categoria",
                c => new
                    {
                        id_categoria = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        nombre_categoria = c.DateTime(nullable: false),
                        desc_categoria = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_categoria);
            
            AddColumn("ask.pregunta", "Categoria_CategoriaId", c => c.Int(nullable: false));
            CreateIndex("ask.pregunta", "Categoria_CategoriaId");
            AddForeignKey("ask.pregunta", "Categoria_CategoriaId", "ask.Categoria", "id_categoria", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("ask.pregunta", "Categoria_CategoriaId", "ask.Categoria");
            DropIndex("ask.pregunta", new[] { "Categoria_CategoriaId" });
            DropColumn("ask.pregunta", "Categoria_CategoriaId");
            DropTable("ask.Categoria");
        }
    }
}
