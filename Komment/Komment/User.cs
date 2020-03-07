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

        public static async Task IsLoggedInAsync()
        {
            IsLoggedIn = await NetworkHandler.AuthenticateAsync();             
        }

        public static class Data
        {
            public static List<Note> notes;
        }
    }
}
