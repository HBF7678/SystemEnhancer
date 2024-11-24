using System;
using System.IO;

namespace Optimizer
{
    internal static class ErrorLogger
    {
        internal static string ErrorLogFile = Path.Combine(Required.CoreFolder, "Optimizer.log");

        internal static void LogError(string functionName, string errorMessage, string errorStackTrace)
        {
            try
            {
                string logEntry = string.Empty;
                
                // Ensure directory exists
                string directory = Path.GetDirectoryName(ErrorLogFile);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                bool isNewFile = !File.Exists(ErrorLogFile) || (File.Exists(ErrorLogFile) && File.ReadAllText(ErrorLogFile).Trim() == string.Empty);

                if (isNewFile)
                {
                    string bitness = Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit";
                    string osInfo = Utilities.CurrentWindowsVersion == WindowsVersion.Windows10 
                        ? $"{Utilities.GetOS()} - {Utilities.GetWindows10Build()} ({bitness})"
                        : $"{Utilities.GetOS()} - ({bitness})";

                    logEntry = $"{osInfo}{Environment.NewLine}";
                    logEntry += $"Optimizer {Program.GetCurrentVersionTostring()} - .NET Framework {Utilities.GetNETFramework()} - Experimental build: {Program.EXPERIMENTAL_BUILD}";
                    logEntry += $"{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}";
                }

                logEntry += $"[ERROR] [{DateTime.Now:yyyy-MM-dd HH:mm:ss}] in function [{functionName}]{Environment.NewLine}";
                logEntry += $"{errorMessage}{Environment.NewLine}{Environment.NewLine}";
                logEntry += $"{errorStackTrace}{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}";

                File.AppendAllText(ErrorLogFile, logEntry);
            }
            catch { }
        }
    }
}
