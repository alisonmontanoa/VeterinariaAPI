using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.DTOs;
using Veterinaria.Core.Entities;
using Veterinaria.Core.Enums;
using Veterinaria.Core.Exceptions;
using Veterinaria.Core.Interfaces;
using Veterinaria.Core.QueryFilters;

namespace Veterinaria.Core.Services
{
    public class ServicioService : IServicioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CrearServicioAsync(ServicioDto servicioDto)
        {
            // Validaciones de negocio

            if (string.IsNullOrWhiteSpace(servicioDto.Nombre) ||
                string.IsNullOrWhiteSpace(servicioDto.Descripcion) ||
                servicioDto.Precio <= 0)
            {
                throw new BusinessException("Todos los campos del servicio son obligatorios.", 400);
            }

            // Verificar duplicidad
            var existe = await _unitOfWork.Servicios.ExistsByNombreAsync(servicioDto.Nombre);
            if (existe)
            {
                throw new BusinessException($"Ya existe un servicio con el nombre '{servicioDto.Nombre}'.", 409);
            }

            // Crear entidad
            var servicio = new Servicio
            {
                Nombre = servicioDto.Nombre.Trim(),
                Descripcion = servicioDto.Descripcion.Trim(),
                Precio = servicioDto.Precio
            };

            // Guardar
            await _unitOfWork.Servicios.AddAsync(servicio);
            await _unitOfWork.SaveChangesAsync();

            return servicio.Id;
        }

        public async Task<IEnumerable<Servicio>> ObtenerServiciosAsync(ServicioQueryFilter filters)
        {
            var servicios = await _unitOfWork.Servicios.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(filters.Nombre))
                servicios = servicios.Where(s => s.Nombre.Contains(filters.Nombre, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(filters.Descripcion))
                servicios = servicios.Where(s => s.Descripcion.Contains(filters.Descripcion, StringComparison.OrdinalIgnoreCase));

            return servicios.OrderByDescending(s => s.Id);
        }
    }
}