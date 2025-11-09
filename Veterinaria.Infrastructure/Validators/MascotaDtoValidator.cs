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
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre de la mascota es obligatorio.")
                .MaximumLength(100);

            RuleFor(x => x.Especie)
                .NotEmpty().WithMessage("Debe indicar la especie de la mascota.")
                .MaximumLength(50);

            RuleFor(x => x.Raza)
                .NotEmpty().WithMessage("Debe indicar la raza de la mascota.")
                .MaximumLength(50);

            RuleFor(x => x.Edad)
                .InclusiveBetween(0, 30)
                .WithMessage("La edad debe estar entre 0 y 30 anios.");
        }
    }

}
