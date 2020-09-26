using Fabrit.Heroes.Data.Entities.Hero;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fabrit.Heroes.Business.Services.Contracts
{
    public interface IUserService
    {
        IAsyncEnumerable<User> GetAll();
        Task<User> GetUser(int id);
        Task<User> AddUser(User user);
        Task<User> UpdateUser(User user);
        Task DeleteUser(int id);
        Task<User> ActivateAccount(int id);
        Task SendConfirmationEmail(User user);
        Task SendResetPasswordEmail(string password, User user);
        string GeneratePassword();
        Task<User> ResetPassword(string email);
        string Uploadfile(IFormFile file, int id);
    }
}
