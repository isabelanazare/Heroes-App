using Fabrit.Heroes.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fabrit.Heroes.Business.Services.Contracts
{
    public interface IPowerService
    {
        IAsyncEnumerable<Power> GetAll();
        Task<Power> GetPower(int id);
        Task<Power> GetPowerByName(string name);
        Task<Power> AddPower(Power power);
        Task<Power> UpdatePower(Power power);
        Task DeletePower(int id);
    }
}
