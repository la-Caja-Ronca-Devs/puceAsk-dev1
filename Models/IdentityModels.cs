using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace puceAsk_dev1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual Cuenta Cuenta { get; set; }
        [Required]
        public DateTime FechaNacimiento { get; set; }
        public byte[] Foto { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        public bool Sexo { get; set; }
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

        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Pregunta> Pregunta { get; set; }
        public DbSet<Respuesta> Respuesta { get; set; }
        public DbSet<Categoria> Categoria { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                        .HasOptional(s => s.Cuenta)
                        .WithRequired(ad => ad.Usuario);
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(s => s.UserId);
            modelBuilder.Entity<IdentityUserRole>().HasKey<string>(s => s.UserId);
            modelBuilder.Entity<Pregunta>().HasMany<Respuesta>(s => s.Respuestas)
                .WithRequired(s => s.Pregunta)
                .HasForeignKey<int>(s => s.PreguntaId);
            modelBuilder.Entity<Cuenta>().HasMany<Mensaje>(s => s.Enviados)
               .WithRequired(s => s.Emisor)
               .HasForeignKey<int>(s => s.EmisorId);
            modelBuilder.Entity<Cuenta>().HasMany<Mensaje>(s => s.Recibidos)
               .WithRequired(s => s.Receptor)
               .HasForeignKey<int>(s => s.ReceptorId);
        }

    public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<puceAsk_dev1.Models.Mensaje> Mensajes { get; set; }
    }
}