using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.QueryFilters
{
    /// <summary>
    /// Filtros de busqueda para citas.
    /// </summary>
    /// <remarks>
    /// Permite aplicar filtros opcionales para consultar citas
    /// junto con parametros de paginacion.
    /// 
    /// Los filtros pueden combinarse entre si.
    /// </remarks>
    public class CitaQueryFilter : PaginationQueryFilter
    {
        /// <summary>
        /// Identificador del veterinario con cita programada.
        /// </summary>
        /// <remarks>
        /// Filtra los veterinarios asociados a una cita especifica.
        /// </remarks>
        /// <example>2</example>
        public int? VeterinarioId { get; set; }

        /// <summary>
        /// Identificador de la mascota con cita asignada.
        /// </summary>
        /// <remarks>
        /// Filtra las mascotas asociadas a una cita especifica.
        /// </remarks>
        /// <example>8</example>
        public int? MascotaId { get; set; }

        /// <summary>
        /// Identificador del dueño de la mascota asociada a una cita.
        /// </summary>
        /// <remarks>
        /// Filtra los dueños asociadas a una cita especifica.
        /// </remarks>
        /// <example>3</example>
        public int? DuenoId { get; set; }

        /// Fecha programada de la cita.
        /// </summary>
        /// <remarks>
        /// Permite realizar busquedas parciales por fechas.
        /// </remarks>
        /// <example>2025-08-04</example>
        public DateTime? Fecha { get; set; }

        /// Estado de la cita.
        /// </summary>
        /// <remarks>
        /// Permite realizar busquedas parciales por estado.
        /// </remarks>
        /// <example>Cancelada</example>
        public string? Estado { get; set; }
    }
}
