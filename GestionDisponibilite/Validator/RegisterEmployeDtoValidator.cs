using FluentValidation;
using GestionDisponibilite.DTOs;

namespace GestionDisponibilite.Validator
{
    public class RegisterEmployeDtoValidator : AbstractValidator<RegisterEmployeDto>
    {
        public RegisterEmployeDtoValidator()
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

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Le mot de passe est obligatoire.")
                .MinimumLength(8).WithMessage("Le mot de passe doit contenir au moins 8 caractères.")
                .MaximumLength(100).WithMessage("Le mot de passe ne doit pas dépasser 100 caractères.");

            RuleFor(x => x.Degree)
                .MaximumLength(100).WithMessage("Le diplôme ne doit pas dépasser 100 caractères.");
        }
    }
}
