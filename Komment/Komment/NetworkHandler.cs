using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Komment
{
    public static class NetworkHandler
    {
        private static readonly string apiURL = "https://kommentapi.herokuapp.com";
        private static readonly HttpClient client = new HttpClient();


        public static async Task<string> LoadAllNotesAsync() 
        {
            var response = await client.GetAsync(apiURL + "/notes");
            return await response.Content.ReadAsStringAsync();
        }

        public static void Initialize()
        {
            client.DefaultRequestHeaders.Add("username", User.username);
            client.DefaultRequestHeaders.Add("password", User.password);
        }
    }
}
