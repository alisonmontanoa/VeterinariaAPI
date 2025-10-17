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
            RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Telefono).NotEmpty().MaximumLength(20)
                .Matches(@"^[0-9+\- ]+$").WithMessage("Sólo dígitos, +, -, espacio.");
            RuleFor(x => x.Direccion).NotEmpty().MaximumLength(200);
        }
    }
}
