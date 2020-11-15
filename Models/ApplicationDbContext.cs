using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace tpEstudiante
{
    public class ApplicationDbContext : IdentityDbContext
    {
         public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<EstudianteMateria>().HasKey(x => new { x.EstudianteId, x.MateriaId });
            modelBuilder.Entity<IdentityUser>().ToTable("Usuarios", "Seguridad");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles", "Seguridad");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UsuariosRoles", "Seguridad");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RolesClaims", "Seguridad");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UsuariosClaims", "Seguridad");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UsuariosLogin", "Seguridad");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UsuariosToken", "Seguridad");

        }

        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Carrera>  Carreras { get; set; }
        public DbSet<EstudianteMateria> EstudianteMaterias { get; set; }
        public DbSet<Materia> Materias { get; set; }
    }
}
