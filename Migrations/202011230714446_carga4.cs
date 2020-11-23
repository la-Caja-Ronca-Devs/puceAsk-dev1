namespace puceAsk_dev1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class carga4 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "ask.pregunta", name: "Cuenta_CuentaId", newName: "CuentaId");
            RenameColumn(table: "ask.Respuestas", name: "Cuenta_CuentaId", newName: "CuentaId");
            RenameIndex(table: "ask.pregunta", name: "IX_Cuenta_CuentaId", newName: "IX_CuentaId");
            RenameIndex(table: "ask.Respuestas", name: "IX_Cuenta_CuentaId", newName: "IX_CuentaId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "ask.Respuestas", name: "IX_CuentaId", newName: "IX_Cuenta_CuentaId");
            RenameIndex(table: "ask.pregunta", name: "IX_CuentaId", newName: "IX_Cuenta_CuentaId");
            RenameColumn(table: "ask.Respuestas", name: "CuentaId", newName: "Cuenta_CuentaId");
            RenameColumn(table: "ask.pregunta", name: "CuentaId", newName: "Cuenta_CuentaId");
        }
    }
}
