namespace puceAsk_dev1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class load1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ask.Categoria",
                c => new
                    {
                        CategoriaId = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        NombreCategoria = c.String(nullable: false),
                        DescCategoria = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CategoriaId);
            
            CreateTable(
                "ask.pregunta",
                c => new
                    {
                        PreguntaId = c.Int(nullable: false, identity: true),
                        UsuarioId = c.String(nullable: false, maxLength: 128),
                        CategoriaId = c.Int(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        TituloPregunta = c.String(nullable: false),
                        DescPregunta = c.String(nullable: false),
                        Fechapregunta = c.DateTime(nullable: false),
                        MejorUsuarioRespuestaId = c.String(),
                    })
                .PrimaryKey(t => t.PreguntaId)
                .ForeignKey("dbo.AspNetUsers", t => t.UsuarioId, cascadeDelete: false)
                .ForeignKey("ask.Categoria", t => t.CategoriaId, cascadeDelete: false)
                .Index(t => t.UsuarioId)
                .Index(t => t.CategoriaId);
            
            CreateTable(
                "ask.Respuestas",
                c => new
                    {
                        UsuarioId = c.String(nullable: false, maxLength: 128),
                        PreguntaId = c.Int(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        FechaPublicacion = c.DateTime(nullable: false),
                        DescRespuesta = c.String(nullable: false),
                    })
                .PrimaryKey(t => new { t.UsuarioId, t.PreguntaId })
                .ForeignKey("dbo.AspNetUsers", t => t.UsuarioId, cascadeDelete: false)
                .ForeignKey("ask.pregunta", t => t.PreguntaId, cascadeDelete: false)
                .Index(t => t.UsuarioId)
                .Index(t => t.PreguntaId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FechaNacimiento = c.DateTime(nullable: false),
                        Foto = c.Binary(),
                        Nombre = c.String(nullable: false),
                        Apellido = c.String(nullable: false),
                        Sexo = c.Boolean(nullable: false),
                        SaldoCuenta = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId);
            
            CreateTable(
                "ask.Mensaje",
                c => new
                    {
                        MensajeId = c.Int(nullable: false, identity: true),
                        ReceptorId = c.String(nullable: false, maxLength: 128),
                        EmisorId = c.String(nullable: false, maxLength: 128),
                        MensajeDesc = c.String(nullable: false),
                        FechaMensaje = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MensajeId)
                .ForeignKey("dbo.AspNetUsers", t => t.EmisorId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.ReceptorId, cascadeDelete: false)
                .Index(t => t.ReceptorId)
                .Index(t => t.EmisorId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("ask.pregunta", "CategoriaId", "ask.Categoria");
            DropForeignKey("ask.Respuestas", "PreguntaId", "ask.pregunta");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("ask.Respuestas", "UsuarioId", "dbo.AspNetUsers");
            DropForeignKey("ask.Mensaje", "ReceptorId", "dbo.AspNetUsers");
            DropForeignKey("ask.pregunta", "UsuarioId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("ask.Mensaje", "EmisorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("ask.Mensaje", new[] { "EmisorId" });
            DropIndex("ask.Mensaje", new[] { "ReceptorId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("ask.Respuestas", new[] { "PreguntaId" });
            DropIndex("ask.Respuestas", new[] { "UsuarioId" });
            DropIndex("ask.pregunta", new[] { "CategoriaId" });
            DropIndex("ask.pregunta", new[] { "UsuarioId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("ask.Mensaje");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("ask.Respuestas");
            DropTable("ask.pregunta");
            DropTable("ask.Categoria");
        }
    }
}
