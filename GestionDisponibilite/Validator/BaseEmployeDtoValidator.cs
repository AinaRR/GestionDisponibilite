using FluentValidation;
using GestionDisponibilite.DTOs;

namespace GestionDisponibilite.Validator
{
    public class BaseEmployeDtoValidator<T> : AbstractValidator<T> where T : BaseEmployeDto
    {
        public BaseEmployeDtoValidator()
        {
            RuleFor(x => x.Nom)
                .NotEmpty().WithMessage("Le nom est requis.")
                .MaximumLength(100).WithMessage("Le nom ne doit pas dépasser 100 caractères.");

            RuleFor(x => x.Prenom)
                .NotEmpty().WithMessage("Le prénom est requis.")
                .MaximumLength(100).WithMessage("Le prénom ne doit pas dépasser 100 caractères.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("L'email est requis.")
                .EmailAddress().WithMessage("L'email n'est pas valide.");
        }
    }
}
