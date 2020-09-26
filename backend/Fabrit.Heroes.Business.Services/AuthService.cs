using Fabrit.Heroes.Business.Services.Contracts;
using Fabrit.Heroes.Data;
using Fabrit.Heroes.Data.Entities;
using Fabrit.Heroes.Data.Entities.Exception;
using Fabrit.Heroes.Data.Entities.Hero;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fabrit.Heroes.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly HeroesDbContext _context;
        private readonly AppSettings _appSettings;
        private readonly IHashingService _hashingService;

        public AuthService(HeroesDbContext context, IOptions<AppSettings> appSettings, IHashingService hashingService)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _hashingService = hashingService;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(
                x => x.Email == model.Email &&
                x.Password == _hashingService.GetHashedPassword(model.Password) && x.IsActivated == true);

            if (user == null) throw new LoginFailedException("Failed login attempt");

            var token = GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IAsyncEnumerable<User> GetAll()
        {
            return _context.Users.AsAsyncEnumerable();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
