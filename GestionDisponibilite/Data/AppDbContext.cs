using GestionDisponibilite.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace GestionDisponibilite.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Projet> Projets { get; set; }
        public DbSet<Employe> Employes { get; set; }
        public DbSet<EmployeProjet> EmployeProjets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Projet>(); // No need to configure ID defaults

            modelBuilder.Entity<Employe>()
                .Property(e => e.EmployeID)
                .ValueGeneratedNever();

            modelBuilder.Entity<EmployeProjet>()
                .HasOne(ep => ep.Employe)
                .WithMany(e => e.EmployeProjets)
                .HasForeignKey(ep => ep.EmployeId);

            modelBuilder.Entity<EmployeProjet>()
                .HasOne(ep => ep.Projet)
                .WithMany(p => p.EmployeProjets)
                .HasForeignKey(ep => ep.ProjetId);
        }
    }
}
