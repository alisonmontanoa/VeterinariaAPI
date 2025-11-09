using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veterinaria.Core.CustomEntities
{
    public class Message
    {
        /// <summary>Tipo de mensaje (success, warning, information, error)</summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>Descripcion o detalle del mensaje.</summary>
        public string Description { get; set; } = string.Empty;
    }
}
