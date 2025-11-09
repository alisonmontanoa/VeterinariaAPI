using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.DTOs;

namespace Veterinaria.Infrastructure.Validators
{
    public class VeterinarioDtoValidator : AbstractValidator<VeterinarioDto>
    {
        public VeterinarioDtoValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Especialidad).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Telefono).NotEmpty().MaximumLength(20);
        }
    }
}
