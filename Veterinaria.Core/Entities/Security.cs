using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.Enums;

namespace Veterinaria.Core.Entities
{
    public class Security : BaseEntity
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public RoleType Role { get; set; }
    }
}
