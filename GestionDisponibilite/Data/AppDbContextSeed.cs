using GestionDisponibilite.Model;
using GestionDisponibilite.RoleUser;
using GestionDisponibilite.Service;
using Microsoft.EntityFrameworkCore;

namespace GestionDisponibilite.Data
{
    public static class AppDbContextSeed
    {
        public static void SeedRolesAndAdmin(ModelBuilder modelBuilder)
        {
            // IDs constants
            var adminRoleId = Guid.Parse("53b4d5dd-ce00-4f79-86de-df5aa32a726f");
            var userRoleId = Guid.Parse("65d4adff-ec87-41bd-b521-9243d61ffbb3");
            var employeAdminId = Guid.Parse("5bcd15a0-78d7-4a7e-98c0-731d2bdcdc59");
            var employeUserId = Guid.Parse("4bae40a4-6316-454e-b02e-12683a1cebdd");

            /* ---------- Roles ---------- */
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = adminRoleId, Name = Roles.Admin },
                new Role { Id = userRoleId, Name = Roles.User }
            );

            /* ---------- Employés ---------- */
            // Hash pour le mot de passe "Admin@2024!"
            const string adminHash =
                "$argon2id$v=19$m=65536,t=3,p=1$da7YgjEXCLmxQ258xLuyHA$BVTvhrpu8jWJt4H5BYilCaqIPMwIAt4y2xn+44Z6urg";

            // Hash pour le mot de passe "User@2024!" (optionnel, montré pour l'exemple)
            const string userHash =
                "$argon2id$v=19$m=65536,t=3,p=1$J+EZTnIFqFnXOs0/HIaFSQ$+T8AE6mc1g4VE1ScQMHOWrw33/RUiIEaC86f/XKv1mw";

            modelBuilder.Entity<Employe>().HasData(
                new Employe
                {
                    EmployeId = employeAdminId,
                    Username = "adminuser",
                    Email = "admin@example.com",
                    Nom = "Admin",
                    Prenom = "Root",
                    PasswordHash = adminHash,
                    Role = Roles.Admin,
                    SurProjet = false
                },
                new Employe
                {
                    EmployeId = employeUserId,
                    Username = "normaluser",
                    Email = "user@example.com",
                    Nom = "User",
                    Prenom = "Normal",
                    PasswordHash = userHash,
                    Role = Roles.User,
                    SurProjet = false
                }
            );

            /* ---------- Liaisons Employe‑Role ---------- */
            modelBuilder.Entity<EmployeRole>().HasData(
                new EmployeRole { EmployeId = employeAdminId, RoleId = adminRoleId },
                new EmployeRole { EmployeId = employeUserId, RoleId = userRoleId }
            );
        }
    }
}
