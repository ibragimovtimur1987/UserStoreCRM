using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStore.BLL
{
    public class Constants
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
    }
}
