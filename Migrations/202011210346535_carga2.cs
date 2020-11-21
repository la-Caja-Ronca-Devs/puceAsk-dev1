namespace puceAsk_dev1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class carga2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ask.Mensaje",
                c => new
                    {
                        id_mensaje = c.Int(nullable: false, identity: true),
                        ReceptorId = c.Int(nullable: false),
                        EmisorId = c.Int(nullable: false),
                        mensaje = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id_mensaje)
                .ForeignKey("ask.Cuenta", t => t.EmisorId, cascadeDelete: true)
                .ForeignKey("ask.Cuenta", t => t.ReceptorId, cascadeDelete: false)
                .Index(t => t.ReceptorId)
                .Index(t => t.EmisorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("ask.Mensaje", "ReceptorId", "ask.Cuenta");
            DropForeignKey("ask.Mensaje", "EmisorId", "ask.Cuenta");
            DropIndex("ask.Mensaje", new[] { "EmisorId" });
            DropIndex("ask.Mensaje", new[] { "ReceptorId" });
            DropTable("ask.Mensaje");
        }
    }
}
