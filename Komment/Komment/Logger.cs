using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Komment
{
    public static class Logger
    {
        private static readonly string _appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static readonly string _docPath = Path.Combine(_appDataPath, "Komment");

        public static void InitializeFolders()
        {
            Directory.CreateDirectory(_docPath);
        }

        public async static Task LogException(Exception e)
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(_docPath, "ExceptionLogs.txt"), true))
            {
                await outputFile.WriteLineAsync($"{DateTime.Now.ToString()}: {e}");
            }
        }

        public async static Task LogError(string message)
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(_docPath, "ErrorLogs.txt"), true))
            {
                await outputFile.WriteLineAsync($"{DateTime.Now.ToString()}: {message}");
            }
        }

        public async static Task LogInfo(string message)
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(_docPath, "InfoLogs.txt"), true))
            {
                await outputFile.WriteLineAsync($"{DateTime.Now.ToString()}: {message}");
            }
        }

    }
}
