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
                .Map(dest => dest.Username, src => src.Username)   // ← ajouté
                .Map(dest => dest.Role, src => src.Role)       // ← ajouté
                .PreserveReference(true);

            // ============================
            // CreateEmployeDto ➜ Employe
            // ============================
            TypeAdapterConfig<CreateEmployeDto, Employe>.NewConfig()
                .Map(dest => dest.EmployeId, _ => Guid.NewGuid())
                .Ignore(dest => dest.PasswordHash)          // sera renseigné par le service
                .Map(dest => dest.Role, src => src.Role ?? "User")
                .Ignore(dest => dest.EmployeProjets);       // évite la recopie vide

            // ============================
            // RegisterEmployeDto ➜ Employe
            // ============================
            TypeAdapterConfig<RegisterEmployeDto, Employe>.NewConfig()
                .Map(dest => dest.EmployeId, _ => Guid.NewGuid())
                .Ignore(dest => dest.PasswordHash)
                .Map(dest => dest.Role, src => src.Role ?? "User")
                .Ignore(dest => dest.EmployeProjets);
            // Username vient directement du DTO (obligatoire), pas de génération automatique

            // ============================
            // UpdateEmployeDto ➜ Employe
            // ============================
            TypeAdapterConfig<UpdateEmployeDto, Employe>.NewConfig();

            // ============================
            // EmployeProjet  ➜  Dto
            // ============================
            TypeAdapterConfig<EmployeProjet, EmployeProjetDto>.NewConfig();

            TypeAdapterConfig<CreateEmployeProjetDto, EmployeProjet>.NewConfig()
                .Map(dest => dest.EmployeId, src => src.EmployeId)
                .Map(dest => dest.ProjetId, src => src.ProjetId);

            TypeAdapterConfig<EmployeProjet, EmployeProjetDetailDto>.NewConfig()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.EmployeNom, src => src.Employe.Nom)
                .Map(dest => dest.ProjetNom, src => src.Projet.NomDuProjet)
                .Map(dest => dest.DateFinProjet, src => src.Projet.FinProjet);

            // ============================
            // Projet  ➜  Dto
            // ============================
            TypeAdapterConfig<Projet, ProjetDto>.NewConfig();

            TypeAdapterConfig<CreateProjetDto, Projet>.NewConfig()
                .Map(dest => dest.ProjetId, _ => Guid.NewGuid());

            TypeAdapterConfig<UpdateProjetDto, Projet>.NewConfig();
        }
    }
}
