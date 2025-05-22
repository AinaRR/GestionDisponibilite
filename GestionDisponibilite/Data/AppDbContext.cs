using GestionDisponibilite.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace GestionDisponibilite.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Projet> Projets { get; set; }
        public DbSet<Employe> Employes { get; set; }
        public DbSet<EmployeProjet> EmployeProjets { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<EmployeRole> EmployeRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ===============================
            // Clés et relations
            // ===============================

            modelBuilder.Entity<Employe>()
                .Property(e => e.EmployeId)
                .ValueGeneratedNever(); // On garde les GUID fournis par le seed

            modelBuilder.Entity<EmployeProjet>()
                .HasOne(ep => ep.Employe)
                .WithMany(e => e.EmployeProjets)
                .HasForeignKey(ep => ep.EmployeId);

            modelBuilder.Entity<EmployeProjet>()
                .HasOne(ep => ep.Projet)
                .WithMany(p => p.EmployeProjets)
                .HasForeignKey(ep => ep.ProjetId);

            modelBuilder.Entity<EmployeRole>()
                .HasKey(er => new { er.EmployeId, er.RoleId });

            modelBuilder.Entity<EmployeRole>()
                .HasOne(er => er.Employe)
                .WithMany(e => e.EmployeRoles)
                .HasForeignKey(er => er.EmployeId);

            modelBuilder.Entity<EmployeRole>()
                .HasOne(er => er.Role)
                .WithMany(r => r.EmployeRoles)
                .HasForeignKey(er => er.RoleId);

            // ===============================
            // Configuration du modèle Projet
            // ===============================
            modelBuilder.Entity<Projet>();

            // ===============================
            // Seed data via classe dédiée
            // ===============================
            AppDbContextSeed.SeedRolesAndAdmin(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
