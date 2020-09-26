using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fabrit.Heroes.Business.Services.Contracts
{
    public interface IEmailService
    {
        Task Send( string to, string subject, string html, string from = null);
    }
}
