using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komment
{
    public static class UserData
    {
        private static readonly string _appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static readonly string _savePath = Path.Combine(_appDataPath, "Komment");

        public static List<Note> Notes = new List<Note>();

        public async static Task<bool> LoadUser()
        {
            if (File.Exists(Path.Combine(_savePath, "userdata.kmt")))
            {
                try
                {
                    var lines = File.ReadAllLines(Path.Combine(_savePath, "userdata.kmt"));
                    User.username = lines[0];
                    User.password = lines[1];
                    await User.LogInAsync();
                    return true;
                }
                catch (Exception e)
                {
                    _ = Logger.LogException(e);
                    return false;
                }
            }
            else
                return false;

        }

        public static async Task SaveUser()
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(_savePath, "userdata.kmt")))
            {
                await outputFile.WriteLineAsync(User.username);
                await outputFile.WriteLineAsync(User.password);
            }
    }
    }
}
