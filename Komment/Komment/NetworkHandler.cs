using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Komment
{
    public static class NetworkHandler
    {
        private static readonly string apiURL = "https://kommentapi.herokuapp.com";
        private static readonly HttpClient client = new HttpClient();

        public static void Initialize()
        {
            client.DefaultRequestHeaders.Add("username", User.username);
            client.DefaultRequestHeaders.Add("password", User.password);
        }

        //Return functions
        public static async Task<List<Note>> LoadAllNotesAsync() 
        {
            try
            {
                var response = await client.GetAsync(apiURL + "/notes");
                var responseString = await response.Content.ReadAsStringAsync();

                if (String.IsNullOrEmpty(responseString))
                {
                    await Logger.LogError("Empty or no response received.");
                    return null;
                }

                try
                {
                    return ParseGETNotes(responseString);
                }
                catch (Exception e)
                {
                    await Logger.LogException(e);
                    return null;
                }
            }
            catch(Exception e)
            {
                await Logger.LogException(e);
                return null;
            }
        }

        public static async Task PostNoteAsync(Note note)
        {
            var values = new Dictionary<string, string>
            {
                {"title" , note.Title }
            };

            var content = new FormUrlEncodedContent(values);

            try
            {
                var response = await client.PostAsync(apiURL + "/notes", content);

                var responseString = await response.Content.ReadAsStringAsync();
                await Logger.LogInfo(responseString);
            }
            catch(Exception e)
            {
                await Logger.LogException(e);
            }
        }



        //Private functions 
        private static List<Note> ParseGETNotes(string stringToParse)
        {
            JObject JResultObject = JObject.Parse(stringToParse);
            JObject JResultUser = JResultObject.Value<JObject>("user");

            JArray JNotes = (JArray)JResultUser["notes"];
            return JNotes.ToObject<List<Note>>();
        }
    }
}
