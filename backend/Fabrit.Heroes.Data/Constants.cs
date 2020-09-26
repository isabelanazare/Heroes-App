using System;

namespace Fabrit.Heroes.Data
{
    public class Constants
    {
        public const char Stack = 'a';
        public const string UsersUrl = "https://localhost:44324/api/users";
        public const string ResetPswEmailTitle = "Reset password";
        public const string ResetPswEmailMessage = "Use this temporary password for first login then change password from my profile page";
        public const string ConfirmEmailTitle = "Reset password";
        public const string ConfirmEmailMessage = "Click the link below to confirm password";
        public const string PicturesFolderName = "Pictures";
        public const string PictureUsersSubFolderName = "Users";
        public const string PictureHeroesSubFolderName = "Heroes";
        public static readonly string[] ImgExtensions = { "png", "jpg", "gif", "bmp" };
    }
}
