using FluentValidation;
using GestionDisponibilite.DTOs;

namespace GestionDisponibilite.Validator
{
    public class CreateEmployeDtoValidator : AbstractValidator<CreateEmployeDto>
    {
        public CreateEmployeDtoValidator()
        {
            RuleFor(x => x.Nom).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Prenom).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Role).MaximumLength(50);
        }
    }
}
