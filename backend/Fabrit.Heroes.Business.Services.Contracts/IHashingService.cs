using System;
using System.Collections.Generic;
using System.Text;

namespace Fabrit.Heroes.Business.Services.Contracts
{
    public interface IHashingService
    {
        string GetHashedPassword(string password);
        int GetPasswordMinLength();
    }
}
