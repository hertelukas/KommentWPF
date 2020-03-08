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

        public static event EventHandler NotesLoaded;

        private static bool initialized = false;

        public static void Initialize()
        {
            //Authenticate user here
            client.DefaultRequestHeaders.Add("username", User.username);
            client.DefaultRequestHeaders.Add("password", User.password);
            initialized = true;
        }

        #region Web Requests
        public static async Task LoadAllNotesAsync() 
        {
            if (!initialized)
                Initialize();

            try
            {
                var response = await client.GetAsync(apiURL + "/notes");
                var responseString = await response.Content.ReadAsStringAsync();

                if (String.IsNullOrEmpty(responseString))
                {
                    _ = Logger.LogError("Empty or no response received.");
                }

                try
                {
                    UserData.Notes = ParseGETNotes(responseString);
                    OnNotesLoaded();
                }
                catch (Exception e)
                {
                    _ = Logger.LogException(e);
                }
            }
            catch(Exception e)
            {
                _ = Logger.LogException(e);
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
                _ = Logger.LogInfo(responseString);
            }
            catch(Exception e)
            {
                _ = Logger.LogException(e);
            }
        }

        public static async Task<RegistrationResponse> RegisterAsync()
        {
            try
            {
                var response = await client.PostAsync(apiURL + "/users", null);
                var responseString = await response.Content.ReadAsStringAsync();
                if (String.IsNullOrEmpty(responseString))
                {
                    _ = Logger.LogError("Empty or no response received.");
                    return RegistrationResponse.Error;
                }
                else
                {
                    try
                    {
                        _ = Logger.LogInfo(responseString);
                        var parsedDict = ParsePOSTUsers(responseString);
                        string code;
                        string message;

                        if (!parsedDict.TryGetValue("message", out message))
                        {
                            _ = Logger.LogError("No message found in registration response");
                            return RegistrationResponse.Error;
                        }
                        else if (parsedDict.TryGetValue("code", out code))
                        {
                            if (int.TryParse(code, out int result))
                            {
                                if (result == 100)
                                {
                                    return RegistrationResponse.Success;
                                }

                                else if(result == 101)                                
                                    return RegistrationResponse.UserExists;
                                
                                else
                                {
                                    _ = Logger.LogError(message);
                                    return RegistrationResponse.Error;
                                }
                            }
                            else
                            {
                                _ = Logger.LogError(message);
                                return RegistrationResponse.Error;
                            }
                        }
                        else
                        {
                            _ = Logger.LogError("No code found in registration");
                            return RegistrationResponse.Error;
                        }
                    }
                    catch (Exception e)
                    {
                        _ = Logger.LogException(e);
                        return RegistrationResponse.Error;
                    }
                }

            }
            catch(Exception e)
            {
                _ = Logger.LogException(e);
                return RegistrationResponse.Error;
            }
        }

        public static async Task<bool> AuthenticateAsync()
        {
            return true;
        }

        #endregion

        #region Private functions
        private static List<Note> ParseGETNotes(string stringToParse)
        {
            JObject JResultObject = JObject.Parse(stringToParse);
            JObject JResultUser = JResultObject.Value<JObject>("user");

            JArray JNotes = (JArray)JResultUser["notes"];
            var notes = JNotes.ToObject<List<Note>>();
            _ = Logger.LogInfo($"There were {JNotes.ToObject<List<Note>>().Count} notes found.");
            return notes;
        }
        
        private static Dictionary<string, string> ParsePOSTUsers(string stringToParse)
        {
            JObject JResultObject = JObject.Parse(stringToParse);
            string message;
            string code;

            message = (string)JResultObject["message"];
            code = (string)JResultObject["code"];

            Dictionary<string, string> response = new Dictionary<string, string>();

            response.Add("message", message);
            response.Add("code", code);

            return response;
        }
        #endregion

        #region Events
        static void OnNotesLoaded()
        {
            NotesLoaded?.Invoke(null, null);
        }

        #endregion
    }
}
