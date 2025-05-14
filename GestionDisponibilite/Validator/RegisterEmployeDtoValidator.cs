using FluentValidation;
using GestionDisponibilite.DTOs;

namespace GestionDisponibilite.Validator
{
    public class RegisterEmployeDtoValidator : BaseEmployeDtoValidator<RegisterEmployeDto>
    {
        public RegisterEmployeDtoValidator()
        {
            RuleFor(x => x.Telephone)
                .Matches(@"^\+?[0-9]{7,15}$")
                .When(x => !string.IsNullOrWhiteSpace(x.Telephone))
                .WithMessage("Le numéro de téléphone n'est pas valide.");

            RuleFor(x => x.Adresse)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.Adresse))
                .WithMessage("L'adresse ne doit pas dépasser 200 caractères.");
        }
    }
}
