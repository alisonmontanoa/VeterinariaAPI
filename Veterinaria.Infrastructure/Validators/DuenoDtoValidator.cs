using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.DTOs;

namespace Veterinaria.Infrastructure.Validators
{
    public class DuenoDtoValidator : AbstractValidator<DuenoDto>
    {
        public DuenoDtoValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre del duenio es obligatorio.")
                .MaximumLength(100);

            RuleFor(x => x.Telefono)
                .NotEmpty().WithMessage("El telefono es obligatorio.")
                .MaximumLength(20)
                .Matches(@"^[0-9+\- ]+$")
                .WithMessage("Solo se permiten digitos, '+', '-', o espacios.");

            RuleFor(x => x.Direccion)
                .NotEmpty().WithMessage("La direccion es obligatoria.")
                .MaximumLength(200);
        }
    }

}
