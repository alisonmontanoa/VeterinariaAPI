using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinaria.Core.Entities;
using Veterinaria.Core.Interfaces;

namespace Veterinaria.Core.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;

        public SecurityService(IUnitOfWork unitOfWork, IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
        }

        public async Task<Security?> GetLoginByCredentials(UserLogin userLogin)
        {
            var user = await _unitOfWork.SecurityRepository
                .GetLoginByCredentials(userLogin);

            if (user == null)
                return null;

            var valid = _passwordService.Check(user.Password, userLogin.Password);

            return valid ? user : null;
        }

        public async Task RegisterUser(Security security)
        {
            _unitOfWork.SecurityRepository.Add(security);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}