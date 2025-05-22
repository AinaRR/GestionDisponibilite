using FluentValidation;
using GestionDisponibilite.DTOs;

namespace GestionDisponibilite.Validator
{
    public class CreateEmployeProjetDtoValidator : AbstractValidator<CreateEmployeProjetDto>
    {
        public CreateEmployeProjetDtoValidator()
        {
            RuleFor(x => x.EmployeId).NotEmpty().WithMessage("EmployeId is required.");
            RuleFor(x => x.ProjetId).NotEmpty().WithMessage("ProjetId is required.");
        }
    }
}
