using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.DTOs;

namespace Veterinaria.Infrastructure.Validators
{
    public class CitaDtoValidator : AbstractValidator<CitaDto>
    {
        public CitaDtoValidator()
        {
            RuleFor(x => x.Fecha)
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("La fecha debe ser igual o posterior a hoy.");

            RuleFor(x => x.Motivo)
                .NotEmpty().WithMessage("Debe especificar el motivo de la cita.");

            RuleFor(x => x.Estado)
                .Must(x => x == "Pendiente" || x == "Atendida" || x == "Cancelada")
                .WithMessage("Estado invalido.");
        }
    }
}
