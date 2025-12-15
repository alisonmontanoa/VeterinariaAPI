using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.DTOs;

namespace Veterinaria.Infrastructure.Validators
{
    public class ActualizarEstadoCitaDtoValidator
        : AbstractValidator<ActualizarEstadoCitaDto>
    {
        public ActualizarEstadoCitaDtoValidator()
        {
            RuleFor(x => x.CitaId)
                .GreaterThan(0)
                .WithMessage("El ID de la cita debe ser valido.");

            RuleFor(x => x.Estado)
                .NotEmpty()
                .WithMessage("El estado es obligatorio.")
                .Must(EstadoPermitido)
                .WithMessage("Estado no valido. Valores permitidos: Pendiente, Atendida, Cancelada");
        }

        private bool EstadoPermitido(string estado)
        {
            var estados = new[] { "Pendiente", "Atendida", "Cancelada" };
            return estados.Contains(estado, StringComparer.OrdinalIgnoreCase);
        }
    }
}