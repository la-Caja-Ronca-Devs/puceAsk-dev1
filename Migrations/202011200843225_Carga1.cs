namespace puceAsk_dev1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Carga1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ask.Cuenta",
                c => new
                    {
                        id_cuenta = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        fecha_creacion = c.DateTime(nullable: false),
                        saldo_cuenta = c.Int(nullable: false),
                        Usuario_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.id_cuenta)
                .ForeignKey("dbo.ApplicationUsers", t => t.Usuario_Id)
                .Index(t => t.Usuario_Id);
            
            CreateTable(
                "ask.pregunta",
                c => new
                    {
                        id_pregunta = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        titulo_pregunta = c.String(nullable: false, maxLength: 150),
                        desc_pregunta = c.String(nullable: false),
                        fecha_pregunta = c.DateTime(nullable: false),
                        Cuenta_CuentaId = c.Int(nullable: false),
                        MejorRespuesta_RespuestaId = c.Int(),
                    })
                .PrimaryKey(t => t.id_pregunta)
                .ForeignKey("ask.Cuenta", t => t.Cuenta_CuentaId, cascadeDelete: true)
                .ForeignKey("ask.Respuestas", t => t.MejorRespuesta_RespuestaId)
                .Index(t => t.Cuenta_CuentaId)
                .Index(t => t.MejorRespuesta_RespuestaId);
            
            CreateTable(
                "ask.Respuestas",
                c => new
                    {
                        id_respuesta = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        fecha_publicacion = c.DateTime(nullable: false),
                        desc_pegunta = c.String(nullable: false),
                        Cuenta_CuentaId = c.Int(nullable: false),
                        Pregunta_PreguntaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_respuesta)
                .ForeignKey("ask.Cuenta", t => t.Cuenta_CuentaId, cascadeDelete: false)
                .ForeignKey("ask.pregunta", t => t.Pregunta_PreguntaId, cascadeDelete: false)
                .Index(t => t.Cuenta_CuentaId)
                .Index(t => t.Pregunta_PreguntaId);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.IdentityRoles", t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.IdentityRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles");
            DropForeignKey("dbo.IdentityUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("ask.Cuenta", "Usuario_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("ask.Respuestas", "Pregunta_PreguntaId1", "ask.pregunta");
            DropForeignKey("ask.pregunta", "MejorRespuesta_RespuestaId", "ask.Respuestas");
            DropForeignKey("ask.Respuestas", "Pregunta_PreguntaId", "ask.pregunta");
            DropForeignKey("ask.Respuestas", "Cuenta_CuentaId", "ask.Cuenta");
            DropForeignKey("ask.pregunta", "Cuenta_CuentaId", "ask.Cuenta");
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("ask.Respuestas", new[] { "Pregunta_PreguntaId1" });
            DropIndex("ask.Respuestas", new[] { "Pregunta_PreguntaId" });
            DropIndex("ask.Respuestas", new[] { "Cuenta_CuentaId" });
            DropIndex("ask.pregunta", new[] { "MejorRespuesta_RespuestaId" });
            DropIndex("ask.pregunta", new[] { "Cuenta_CuentaId" });
            DropIndex("ask.Cuenta", new[] { "Usuario_Id" });
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.IdentityUserClaims");
            DropTable("dbo.ApplicationUsers");
            DropTable("ask.Respuestas");
            DropTable("ask.pregunta");
            DropTable("ask.Cuenta");
        }
    }
}
