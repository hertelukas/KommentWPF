using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komment
{
    public static class User
    {
        public static string username;
        public static string password;
        public static bool IsLoggedIn;

        public static async Task LogInAsync()
        {
            IsLoggedIn = await NetworkHandler.AuthenticateAsync();
        }

        public static async Task RegisterAsync()
        {
            IsLoggedIn = await NetworkHandler.RegisterAsync();
        }

        public static class Data
        {
            public static List<Note> notes;
        }
    }
}
