using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;

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
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("username", User.username);
            client.DefaultRequestHeaders.Add("password", User.password);
            initialized = true;
        }

        #region Web Requests
        public static async Task LoadAllNotesAsync() 
        {
            if (!initialized && User.username != null)
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
                        var parsedDict = ParseBasicResponse(responseString);

                        if (!parsedDict.TryGetValue("message", out string message))
                        {
                            _ = Logger.LogError("No message found in registration response");
                            return RegistrationResponse.Error;
                        }
                        else if (parsedDict.TryGetValue("code", out string code))
                        {
                            if (int.TryParse(code, out int result))
                            {
                                if (result == 100)
                                {
                                    return RegistrationResponse.Success;
                                }

                                else if (result == 101)
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

        public static async Task<LoginResponse> LoginAsync()
        {
            Initialize();
            try
            {
                var response = await client.GetAsync(apiURL + "/users");
                var responseString = await response.Content.ReadAsStringAsync();
                if (String.IsNullOrEmpty(responseString))
                {
                    _ = Logger.LogError("Empty or no response received.");
                    return LoginResponse.Error;
                }
                else
                {
                    var parsedDict = ParseBasicResponse(responseString);

                    if (!parsedDict.TryGetValue("message", out string message))
                    {
                        _ = Logger.LogError("No message found in registration response");
                        return LoginResponse.Error;
                    }
                    else if (parsedDict.TryGetValue("code", out string code))
                    {
                        _ = Logger.LogInfo(message);
                        if (int.TryParse(code, out int result))
                        {
                            if (result == 104)
                            {
                                return LoginResponse.Success;
                            }
                            else
                            {
                                _ = Logger.LogInfo("Login failed because: " + message);
                                return LoginResponse.Unauthanticated;
                            }
                        }
                        else
                        {
                            _ = Logger.LogError(message);
                            return LoginResponse.Error;
                        }
                    }
                    else
                    {
                        return LoginResponse.Error;
                    }
                }
            }
            catch(Exception e)
            {
                _ = Logger.LogException(e);
                return LoginResponse.Error;
            }
        }

        public static async Task<UpdateNoteResponse> UpdateNoteAsync(Note note)
        {
            if(note.Title != null)
            {
                var httpPutRequest = (HttpWebRequest)WebRequest.Create(apiURL + "/notes/" + note._id);

                //string postData = "content=" + note.Content + "&title=" + note.Title;
                string postData = JsonConvert.SerializeObject(note);

                postData = postData.Replace("Folders", "folders");
                postData = postData.Replace("Title", "title");
                postData = postData.Replace("Content", "content");

                UTF8Encoding encoding = new UTF8Encoding();
                byte[] byte1 = encoding.GetBytes (postData);

                httpPutRequest.ContentType = "application/json; charset=UTF-8";
                httpPutRequest.Method = "PUT";
                httpPutRequest.ContentLength = byte1.Length;
                httpPutRequest.Headers.Add("username", User.username);
                httpPutRequest.Headers.Add("password", User.password);

                Stream stream = httpPutRequest.GetRequestStream();

                stream.Write(byte1, 0, byte1.Length);

                var response = await httpPutRequest.GetResponseAsync();
                
                using(var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var responseString = streamReader.ReadToEnd();
                    _ = Logger.LogInfo(responseString);
                    return UpdateNoteResponse.Success;
                }

            }
            else
            {
                return UpdateNoteResponse.Error;
            }
        }

        public static async Task<DeleteNoteRespnse> DeleteNoteAsync(Note note)
        {            
            return await DeleteNoteAsync(note._id);            

        }

        public static async Task<DeleteNoteRespnse> DeleteNoteAsync(string id)
        {
            var httpPutRequest = (HttpWebRequest)WebRequest.Create(apiURL + "/notes/" + id);


            ASCIIEncoding encoding = new ASCIIEncoding();

            httpPutRequest.Method = "DELETE";
            httpPutRequest.Headers.Add("username", User.username);
            httpPutRequest.Headers.Add("password", User.password);


            var response = await httpPutRequest.GetResponseAsync();

            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var responseString = streamReader.ReadToEnd();
                _ = Logger.LogInfo(responseString);
                return DeleteNoteRespnse.Success;
            }

        }
        #endregion

        #region Private functions
        private static List<Note> ParseGETNotes(string stringToParse)
        {
            JObject JResultObject = JObject.Parse(stringToParse);
            JObject JResultUser = JResultObject.Value<JObject>("user");

            JArray JNotes = (JArray)JResultUser["notes"];
            var notes = JNotes.ToObject<List<Note>>();
            return notes;
        }
        
        private static Dictionary<string, string> ParseBasicResponse(string stringToParse)
        {
            JObject JResultObject = JObject.Parse(stringToParse);
            string message;
            string code;

            message = (string)JResultObject["message"];
            code = (string)JResultObject["code"];

            Dictionary<string, string> response = new Dictionary<string, string>
            {
                { "message", message },
                { "code", code }
            };

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
