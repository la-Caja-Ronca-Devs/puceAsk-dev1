using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace puceAsk_dev1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public DateTime FechaNacimiento { get; set; }
        public byte[] Foto { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        public bool Sexo { get; set; }
        public int SaldoCuenta { get; set; }
        public ICollection<Pregunta> Preguntas { get; set; }
        public ICollection<Respuesta> Respuestas { get; set; }
        public ICollection<Mensaje> Recibidos { get; set; }
        public ICollection<Mensaje> Enviados { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("pa_con", throwIfV1Schema: false)
        {
        }

        public DbSet<Pregunta> Pregunta { get; set; }
        public DbSet<Respuesta> Respuesta { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>().HasMany<Pregunta>(s => s.Preguntas)
                .WithRequired(s => s.Categoria)
                .HasForeignKey<int>(s => s.CategoriaId);
            modelBuilder.Entity<Pregunta>().HasMany<Respuesta>(s => s.Respuestas)
                .WithRequired(s => s.Pregunta)
                .HasForeignKey<int>(s => s.PreguntaId);
            modelBuilder.Entity<ApplicationUser>().HasMany<Pregunta>(s => s.Preguntas)
                .WithRequired(s => s.Usuario)
                .HasForeignKey<string>(s => s.UsuarioId);
            modelBuilder.Entity<ApplicationUser>().HasMany<Respuesta>(s => s.Respuestas)
                .WithRequired(s => s.Usuario)
                .HasForeignKey<string>(s => s.UsuarioId);
            modelBuilder.Entity<ApplicationUser>().HasMany<Mensaje>(s => s.Enviados)
               .WithRequired(s => s.Emisor)
               .HasForeignKey<string>(s => s.EmisorId);
            modelBuilder.Entity<ApplicationUser>().HasMany<Mensaje>(s => s.Recibidos)
               .WithRequired(s => s.Receptor)
               .HasForeignKey<string>(s => s.ReceptorId);

            base.OnModelCreating(modelBuilder);
         
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<puceAsk_dev1.Models.Mensaje> Mensajes { get; set; }
        
    }
}