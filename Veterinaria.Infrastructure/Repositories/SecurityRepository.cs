using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.Entities;
using Veterinaria.Core.Interfaces;
using Veterinaria.Infrastructure.Data;

namespace Veterinaria.Infrastructure.Repositories
{
    public class SecurityRepository : BaseRepository<Security>, ISecurityRepository
    {
        public SecurityRepository(VeterinariaContext context)
            : base(context)
        {
        }

        public async Task<Security?> GetLoginByCredentials(UserLogin login)
        {
            return await _entities.FirstOrDefaultAsync(x => x.Login == login.User);
        }
    }
}