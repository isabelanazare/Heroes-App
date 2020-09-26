using Fabrit.Heroes.Business.Services.Contracts;
using Fabrit.Heroes.Data;
using Fabrit.Heroes.Data.Entities.Exception;
using Fabrit.Heroes.Data.Entities.Hero;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PasswordGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Fabrit.Heroes.Business.Services
{
    public class UserService : IUserService
    {
        private readonly HeroesDbContext _context;
        private readonly IHashingService _hashingService;
        private readonly IEmailService _emailService;

        public UserService(HeroesDbContext context, IHashingService hashingService, IEmailService emailService)
        {
            _context = context;
            _hashingService = hashingService;
            _emailService = emailService;
        }

        public IAsyncEnumerable<User> GetAll()
        {
            return _context.Users.AsAsyncEnumerable();
        }

        public async Task<User> GetUser(int id)
        {
            User user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }

            return user;
        }

        private void ValidateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.ImgPath))
            {
                throw new InvalidEntityException("User is invalid");
            }
            var addr = new System.Net.Mail.MailAddress(user.Email);
            if (addr.Address != user.Email)
            {
                throw new InvalidEntityException("User is invalid");
            }
        }

        public async Task<User> AddUser(User user)
        {
            ValidateUser(user);

            string hashedPassword = _hashingService.GetHashedPassword(user.Password);
            user.Password = hashedPassword;

            _context.Users.Add(user);

            await _context.SaveChangesAsync();
            await SendConfirmationEmail(user);
            return user;
        }

        public async Task<User> UpdateUser(User user)
        {
            var dbUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == user.Id);

            if (dbUser == null)
            {
                throw new EntityNotFoundException("User doesn't exist");
            }

            var oldPassword = user.Password;
            user.Password = _hashingService.GetHashedPassword(oldPassword);
            user.IsActivated = dbUser.IsActivated;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException("User doesn't exist");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> ActivateAccount(int id)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException("User doesn't exist");
            }
            user.IsActivated = true;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task SendConfirmationEmail(User user)

        {
            var verifyUrl = $"{Constants.UsersUrl}/activate/{user.Id}";

            var message = $@" {Constants.ConfirmEmailMessage}
                             <p> <a href=""{verifyUrl}"">{verifyUrl}</a> </p>";
            var html = $@" <p> {message} </p>";

            await _emailService.Send(user.Email, Constants.ConfirmEmailTitle, html);
        }

        public async Task SendResetPasswordEmail(string password, User user)

        {
            var message = $@" {Constants.ResetPswEmailMessage}
                             <p> {password} </p>";
            var html = $@" <p> {message} </p>";

            await _emailService.Send(user.Email, Constants.ResetPswEmailTitle, html);
        }

        public string GeneratePassword()
        {
            var passwordGenerator = new Password();
            return passwordGenerator.Next();
        }

        public async Task<User> ResetPassword(string email)
        {
            var newPassword = GeneratePassword();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            user.Password = newPassword;

            var updatedUser = await UpdateUser(user);

            await SendResetPasswordEmail(newPassword, user);
            return updatedUser;
        }

        private void ValidateFileName(string fileName)
        {
            var extension = fileName.Substring(fileName.Length - 3);

            if (Array.IndexOf(Constants.ImgExtensions, extension) == -1)
            {
                throw new InvalidFileException("Bad file extension");
            }
        }

        public string Uploadfile(IFormFile file, int id)
        {
            var folderName = Path.Combine(Constants.PicturesFolderName, Constants.PictureUsersSubFolderName, id.ToString());
            Directory.CreateDirectory(folderName);

            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            string dbPath;

            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"').ToString();

                ValidateFileName(fileName);

                var fullPath = Path.Combine(pathToSave, fileName);
                dbPath = Path.Combine(folderName, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return dbPath;
            }
            else throw new InvalidFileException("File length < 0");
        }
    }
}
