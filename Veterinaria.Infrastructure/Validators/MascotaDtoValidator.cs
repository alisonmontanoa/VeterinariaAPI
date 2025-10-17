using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.DTOs;

namespace Veterinaria.Infrastructure.Validators
{
    public class MascotaDtoValidator : AbstractValidator<MascotaDto>
    {
        public MascotaDtoValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Especie).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Raza).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Edad).InclusiveBetween(0, 30);
            RuleFor(x => x.DuenoId).GreaterThan(0);
        }
    }
}
