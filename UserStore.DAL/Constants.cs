using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStore.DAL
{
    public static class Constants
    {
        public static class Path
        {
            public static string PathPoster = @"App_Data\uploads\poster";
        }
        public static class Role
        {
            public const string Manager = "manager";
            public const string User = "user";
        }
        public static class AdminData
        {
            public const string Email = "someemail@mail.ru";
            public const string UserName = "someemail@mail.ru";
            public const string Password = "ad57D_ewr45";
        }
        public static class SMTPSetting
        {
            public const string Host = "smtp.gmail.com";
            public const int Port = 587;
        }
    }
}
