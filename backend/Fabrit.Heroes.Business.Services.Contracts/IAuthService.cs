using Fabrit.Heroes.Data.Entities;
using Fabrit.Heroes.Data.Entities.Hero;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fabrit.Heroes.Business.Services.Contracts
{
    public interface IAuthService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<User> GetById(int id);
        IAsyncEnumerable<User> GetAll();
    }
}
