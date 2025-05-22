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
            var adminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var userRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var employeAdminId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var employeUserId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

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
