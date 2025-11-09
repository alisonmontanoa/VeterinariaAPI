using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.DTOs;

namespace Veterinaria.Infrastructure.Validators
{
    public class ServicioDtoValidator : AbstractValidator<ServicioDto>
    {
        public ServicioDtoValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(300);
            RuleFor(x => x.Precio).GreaterThan(0);
        }
    }
}