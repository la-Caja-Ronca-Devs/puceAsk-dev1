namespace puceAsk_dev1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class carga1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ask.Categoria",
                c => new
                    {
                        id_categoria = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        nombre_categoria = c.String(nullable: false),
                        desc_categoria = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id_categoria);
            
            CreateTable(
                "ask.pregunta",
                c => new
                    {
                        id_pregunta = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        titulo_pregunta = c.String(nullable: false, maxLength: 150),
                        desc_pregunta = c.String(nullable: false),
                        fecha_pregunta = c.DateTime(nullable: false),
                        Categoria_CategoriaId = c.Int(nullable: false),
                        Cuenta_CuentaId = c.Int(nullable: false),
                        MejorRespuesta_RespuestaId = c.Int(),
                    })
                .PrimaryKey(t => t.id_pregunta)
                .ForeignKey("ask.Categoria", t => t.Categoria_CategoriaId, cascadeDelete: true)
                .ForeignKey("ask.Cuenta", t => t.Cuenta_CuentaId, cascadeDelete: true)
                .ForeignKey("ask.Respuestas", t => t.MejorRespuesta_RespuestaId)
                .Index(t => t.Categoria_CategoriaId)
                .Index(t => t.Cuenta_CuentaId)
                .Index(t => t.MejorRespuesta_RespuestaId);
            
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
                "ask.Respuestas",
                c => new
                    {
                        id_respuesta = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        fecha_publicacion = c.DateTime(nullable: false),
                        desc_respuesta = c.String(nullable: false),
                        PreguntaId = c.Int(nullable: false),
                        Cuenta_CuentaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_respuesta)
                .ForeignKey("ask.Cuenta", t => t.Cuenta_CuentaId, cascadeDelete: false)
                .ForeignKey("ask.pregunta", t => t.PreguntaId, cascadeDelete: false)
                .Index(t => t.PreguntaId)
                .Index(t => t.Cuenta_CuentaId);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FechaNacimiento = c.DateTime(nullable: false),
                        Foto = c.Binary(),
                        Nombre = c.String(nullable: false),
                        Apellido = c.String(nullable: false),
                        Sexo = c.Boolean(nullable: false),
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
            DropForeignKey("ask.Respuestas", "PreguntaId", "ask.pregunta");
            DropForeignKey("ask.pregunta", "MejorRespuesta_RespuestaId", "ask.Respuestas");
            DropForeignKey("ask.pregunta", "Cuenta_CuentaId", "ask.Cuenta");
            DropForeignKey("dbo.IdentityUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("ask.Cuenta", "Usuario_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.IdentityUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("ask.Respuestas", "Cuenta_CuentaId", "ask.Cuenta");
            DropForeignKey("ask.pregunta", "Categoria_CategoriaId", "ask.Categoria");
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserLogins", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaims", new[] { "ApplicationUser_Id" });
            DropIndex("ask.Respuestas", new[] { "Cuenta_CuentaId" });
            DropIndex("ask.Respuestas", new[] { "PreguntaId" });
            DropIndex("ask.Cuenta", new[] { "Usuario_Id" });
            DropIndex("ask.pregunta", new[] { "MejorRespuesta_RespuestaId" });
            DropIndex("ask.pregunta", new[] { "Cuenta_CuentaId" });
            DropIndex("ask.pregunta", new[] { "Categoria_CategoriaId" });
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.IdentityUserClaims");
            DropTable("dbo.ApplicationUsers");
            DropTable("ask.Respuestas");
            DropTable("ask.Cuenta");
            DropTable("ask.pregunta");
            DropTable("ask.Categoria");
        }
    }
}
