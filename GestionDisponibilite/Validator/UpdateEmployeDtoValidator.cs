using FluentValidation;
using GestionDisponibilite.DTOs;

namespace GestionDisponibilite.Validator
{
    public class UpdateEmployeDtoValidator : AbstractValidator<UpdateEmployeDto>
    {
        public UpdateEmployeDtoValidator()
        {
            RuleFor(x => x.Nom)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Le nom est obligatoire.")
                .MaximumLength(100).WithMessage("Le nom ne doit pas dépasser 100 caractères.");

            RuleFor(x => x.Prenom)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Le prénom est obligatoire.")
                .MaximumLength(100).WithMessage("Le prénom ne doit pas dépasser 100 caractères.");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("L'adresse e-mail est obligatoire.")
                .EmailAddress().WithMessage("Format d'adresse e-mail invalide.")
                .MaximumLength(255).WithMessage("L'adresse e-mail ne doit pas dépasser 255 caractères.");

            RuleFor(x => x.Telephone)
                .Cascade(CascadeMode.Stop)
                .Matches(@"^\+?[0-9\s\-]{7,15}$").WithMessage("Le format du numéro de téléphone est invalide.")
                .When(x => !string.IsNullOrWhiteSpace(x.Telephone));

            RuleFor(x => x.Username)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(100).WithMessage("Le nom d'utilisateur ne doit pas dépasser 100 caractères.")
                .When(x => !string.IsNullOrWhiteSpace(x.Username));

            RuleFor(x => x.Degree)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(100).WithMessage("Le diplôme ne doit pas dépasser 100 caractères.")
                .When(x => !string.IsNullOrWhiteSpace(x.Degree));
        }
    }
}
