using GestionDisponibilite.DTOs;
using GestionDisponibilite.Model;
using Mapster;

namespace GestionDisponibilite.Mapping
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            // ============================
            // Employe  ➜  EmployeDto
            // ============================
            TypeAdapterConfig<Employe, EmployeDto>.NewConfig()
                .Map(dest => dest.Id, src => src.EmployeId)
                .Map(dest => dest.NombreDeProjet, src => src.EmployeProjets.Count)
                .Map(dest => dest.Role, src => src.Role)
                .PreserveReference(true);

            // ============================
            // CreateEmployeDto ➜ Employe
            // ============================
            TypeAdapterConfig<CreateEmployeDto, Employe>.NewConfig()
                .Map(dest => dest.EmployeId, _ => Guid.NewGuid())
                .Ignore(dest => dest.PasswordHash)
                .Ignore(dest => dest.Role) // rôle défini dans le service
                .Ignore(dest => dest.EmployeProjets);

            // ============================
            // RegisterEmployeDto ➜ Employe
            // ============================
            TypeAdapterConfig<RegisterEmployeDto, Employe>.NewConfig()
                .Map(dest => dest.EmployeId, _ => Guid.NewGuid())
                .Ignore(dest => dest.PasswordHash)
                .Ignore(dest => dest.Role)  
                .Ignore(dest => dest.EmployeProjets);

            // ============================
            // UpdateEmployeDto ➜ Employe
            // ============================
            TypeAdapterConfig<UpdateEmployeDto, Employe>.NewConfig();

            // ============================
            // EmployeProjet ➜ EmployeProjetDto
            // ============================
            TypeAdapterConfig<EmployeProjet, EmployeProjetDto>.NewConfig();

            // ============================
            // CreateEmployeProjetDto ➜ EmployeProjet
            // ============================
            TypeAdapterConfig<CreateEmployeProjetDto, EmployeProjet>.NewConfig()
                .Map(dest => dest.EmployeId, src => src.EmployeId)
                .Map(dest => dest.ProjetId, src => src.ProjetId);

            // ============================
            // EmployeProjet ➜ EmployeProjetDetailDto
            // ============================
            TypeAdapterConfig<EmployeProjet, EmployeProjetDetailDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.EmployeNom, src => src.Employe.Nom)
                .Map(dest => dest.ProjetNom, src => src.Projet.NomDuProjet)
                .Map(dest => dest.DateFinProjet, src => src.Projet.FinProjet);

            // ============================
            // Projet ➜ ProjetDto
            // ============================
            TypeAdapterConfig<Projet, ProjetDto>.NewConfig();

            // ============================
            // CreateProjetDto ➜ Projet
            // ============================
            TypeAdapterConfig<CreateProjetDto, Projet>.NewConfig()
                .Map(dest => dest.ProjetId, _ => Guid.NewGuid());

            // ============================
            // UpdateProjetDto ➜ Projet
            // ============================
            TypeAdapterConfig<UpdateProjetDto, Projet>.NewConfig();
        }
    }
}

