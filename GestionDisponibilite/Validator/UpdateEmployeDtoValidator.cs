using FluentValidation;
using GestionDisponibilite.DTOs;

namespace GestionDisponibilite.Validator
{
    public class UpdateEmployeDtoValidator : AbstractValidator<UpdateEmployeDto>
    {
        public UpdateEmployeDtoValidator()
        {
            RuleFor(x => x.Nom).NotEmpty();
            RuleFor(x => x.Prenom).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Telephone).Matches(@"^\+?[0-9\s\-]{7,15}$").When(x => !string.IsNullOrEmpty(x.Telephone));
            RuleFor(x => x.Username).MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Username));
            RuleFor(x => x.Degree).MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Degree));
        }
    }
}
