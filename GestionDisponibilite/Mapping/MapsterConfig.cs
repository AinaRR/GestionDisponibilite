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
            // Employe Mappings
            // ============================

            // Entity → DTO
            TypeAdapterConfig<Employe, EmployeDto>.NewConfig()
                .Map(dest => dest.Id, src => src.EmployeID)
                .Map(dest => dest.Nom, src => src.Nom)
                .Map(dest => dest.Prenom, src => src.Prenom)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.NombreDeProjet, src => src.EmployeProjets.Count);

            // CreateEmployeDto → Entity
            TypeAdapterConfig<CreateEmployeDto, Employe>.NewConfig()
                .Map(dest => dest.EmployeID, _ => Guid.NewGuid())
                .Map(dest => dest.Nom, src => src.Nom)
                .Map(dest => dest.Prenom, src => src.Prenom)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.PasswordHash, _ => "") // should be set in service layer
                .Map(dest => dest.Degree, src => src.Degree)
                .Map(dest => dest.Username, src => src.Username)
                .Map(dest => dest.Role, src => src.Role ?? "User")
                .Ignore(dest => dest.Adresse)
                .Ignore(dest => dest.Telephone)
                .Ignore(dest => dest.DateDeNaissance)
                .Ignore(dest => dest.EmployeProjets);

            // RegisterEmployeDto → Entity
            TypeAdapterConfig<RegisterEmployeDto, Employe>.NewConfig()
                .Map(dest => dest.EmployeID, _ => Guid.NewGuid())
                .Map(dest => dest.Nom, src => src.Nom)
                .Map(dest => dest.Prenom, src => src.Prenom)
                .Map(dest => dest.Email, src => src.Email)
                .Map(dest => dest.PasswordHash, _ => "") // set in service
                .Map(dest => dest.Telephone, src => src.Telephone)
                .Map(dest => dest.DateDeNaissance, src => src.DateDeNaissance)
                .Map(dest => dest.Adresse, src => src.Adresse)
                .Ignore(dest => dest.Degree)
                .Ignore(dest => dest.Username)
                .Ignore(dest => dest.Role)
                .Ignore(dest => dest.EmployeProjets);

            // UpdateEmployeDto → Entity
            TypeAdapterConfig<UpdateEmployeDto, Employe>.NewConfig()
                .Map(dest => dest.Nom, src => src.Nom)
                .Map(dest => dest.Prenom, src => src.Prenom)
                .Map(dest => dest.Adresse, src => src.Adresse)
                .Map(dest => dest.Username, src => src.Username)
                .Map(dest => dest.Degree, src => src.Degree)
                .Map(dest => dest.Email, src => src.Email);

            // ============================
            // EmployeProjet Mappings
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
            // Projet Mappings
            // ============================

            TypeAdapterConfig<Projet, ProjetDto>.NewConfig();

            TypeAdapterConfig<CreateProjetDto, Projet>.NewConfig()
                .Map(dest => dest.ProjetId, _ => Guid.NewGuid());

            TypeAdapterConfig<UpdateProjetDto, Projet>.NewConfig();
        }
    }
}
