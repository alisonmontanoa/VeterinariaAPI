using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.QueryFilters
{
    /// <summary>
    /// Filtros de busqueda para mascotas.
    /// </summary>
    /// <remarks>
    /// Permite aplicar filtros opcionales para consultar mascotas
    /// junto con parametros de paginacion.
    /// 
    /// Los filtros pueden combinarse entre si.
    /// </remarks>
    public class MascotaQueryFilter : PaginationQueryFilter
    {
        /// Nombre de la mascota.
        /// </summary>
        /// <remarks>
        /// Permite realizar busquedas parciales por nombre.
        /// </remarks>
        /// <example>Luna</example>
        public string? Nombre { get; set; }

        /// <summary>
        /// Especie de la mascota.
        /// </summary>
        /// <remarks>
        /// Ejemplos comunes: Perro, Gato.
        /// </remarks>
        /// <example>Perro</example>
        public string? Especie { get; set; }

        /// <summary>
        /// Raza de la mascota.
        /// </summary>
        /// <remarks>
        /// Permite busquedas parciales por raza.
        /// </remarks>
        /// <example>Labrador</example>
        public string? Raza { get; set; }

        /// <summary>
        /// Identificador del dueño de la mascota.
        /// </summary>
        /// <remarks>
        /// Filtra las mascotas asociadas a un dueño especifico.
        /// </remarks>
        /// <example>3</example>
        public int? DuenoId { get; set; }

        /// <summary>
        /// Edad de la mascota.
        /// </summary>
        /// <remarks>
        /// Filtra mascotas por edad exacta.
        /// </remarks>
        /// <example>5</example>
        public int? Edad { get; set; }
    }
}
