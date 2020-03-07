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

        public static async Task<string> RegisterAsync()
        {
            RegistrationResponse registrationResponse = await NetworkHandler.RegisterAsync();
            if(registrationResponse == RegistrationResponse.Success)
            {
                IsLoggedIn = true;
                return null;
            }
            else if(registrationResponse == RegistrationResponse.UserExists)
            {
                IsLoggedIn = false;
                return "A user with this username allready exists.";
            }
            else
            {
                IsLoggedIn = false;
                return "Error registering you. Try again later.";
            }
        }

        public static class Data
        {
            public static List<Note> notes;
        }
    }
}
