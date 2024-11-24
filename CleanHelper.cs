using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;

namespace SystemEnhancer
{
    /// <summary>
    /// Manages system maintenance and disk cleanup operations for Windows
    /// </summary>
    internal static class StorageOptimizer
    {
        #region Native Methods
        [DllImport("Shell32.dll")]
        private static extern int SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlag dwFlags);
        #endregion

        #region System Locations
        private static class WindowsPaths
        {
            public static readonly string SystemRoot = Environment.GetFolderPath(Environment.SpecialFolder.System);
            public static readonly string TempDirectory = Path.GetTempPath();
            public static readonly string UserDataRoaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            public static readonly string SharedData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            public static readonly string UserDataLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            public static readonly string WindowsDirectory = Environment.GetEnvironmentVariable("WINDIR", EnvironmentVariableTarget.Machine);
        }
        #endregion

        #region Web Browser Data Locations
        private static class BrowserDataPaths
        {
            public static class LegacyBrowser
            {
                public static readonly string[] CacheLocations = {
                    Path.Combine(WindowsPaths.UserDataLocal, "Microsoft", "Windows", "INetCache", "IE"),
                    Path.Combine(WindowsPaths.UserDataLocal, "Microsoft", "Windows", "WebCache.old")
                };
            }

            public static class ChromeBrowser
            {
                private static readonly string UserDataPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "AppData", "Local", "Google", "Chrome", "User Data"
                );

                public static readonly string[] CacheLocations = {
                    Path.Combine(UserDataPath, "Default", "Cache"),
                    Path.Combine(UserDataPath, "Default", "Code Cache"),
                    Path.Combine(UserDataPath, "Default", "GPUCache"),
                    Path.Combine(UserDataPath, "ShaderCache"),
                    Path.Combine(UserDataPath, "Default", "Service Worker", "CacheStorage"),
                    Path.Combine(UserDataPath, "Default", "Service Worker", "ScriptCache"),
                    Path.Combine(UserDataPath, "GrShaderCache", "GPUCache")
                };

                public static readonly string[] SessionData = {
                    Path.Combine(UserDataPath, "Default", "Sessions"),
                    Path.Combine(UserDataPath, "Default", "Session Storage"),
                    Path.Combine(UserDataPath, "Default", "Extension State")
                };

                public static readonly string[] UserData = {
                    Path.Combine(UserDataPath, "Default", "IndexedDB"),
                    Path.Combine(UserDataPath, "Default", "Cookies"),
                    Path.Combine(UserDataPath, "Default", "Cookies-journal")
                };

                public static readonly string[] BrowsingData = {
                    Path.Combine(UserDataPath, "Default", "History"),
                    Path.Combine(UserDataPath, "Default", "History Provider Cache"),
                    Path.Combine(UserDataPath, "Default", "History-journal")
                };

                public static readonly string SecurityData = Path.Combine(UserDataPath, "Default", "Login Data");
            }
        }
        #endregion

        #region Cleanup Management
        private static readonly List<string> PendingCleanupPaths = new List<string>();
        private static ByteSize EstimatedRecovery = new ByteSize(0);

        /// <summary>
        /// Scans a location for cleanup and estimates potential space savings
        /// </summary>
        private static async Task ScanLocationAsync(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            try
            {
                if (File.Exists(path))
                {
                    PendingCleanupPaths.Add(path);
                    EstimatedRecovery = EstimatedRecovery.Add(await GetFileSizeAsync(path));
                    return;
                }

                if (!Directory.Exists(path)) return;

                await Task.Run(() =>
                {
                    var directory = new DirectoryInfo(path);
                    foreach (var item in directory.EnumerateFileSystemInfos("*", SearchOption.AllDirectories))
                    {
                        try
                        {
                            PendingCleanupPaths.Add(item.FullName);
                            if (item is FileInfo file)
                            {
                                EstimatedRecovery = EstimatedRecovery.Add(ByteSize.FromBytes(file.Length));
                            }
                        }
                        catch (Exception ex) when (ex is UnauthorizedAccessException || ex is SecurityException)
                        {
                            ErrorLogger.LogError("StorageOptimizer.ScanLocation", 
                                $"Access denied to {item.FullName}", ex.StackTrace);
                        }
                        catch (Exception ex)
                        {
                            ErrorLogger.LogError("StorageOptimizer.ScanLocation", 
                                $"Error scanning {item.FullName}: {ex.Message}", ex.StackTrace);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("StorageOptimizer.ScanLocation", 
                    $"Error processing path {path}: {ex.Message}", ex.StackTrace);
            }
        }

        /// <summary>
        /// Executes cleanup for all pending items
        /// </summary>
        internal static async Task ExecuteCleanupAsync()
        {
            foreach (string path in PendingCleanupPaths.ToList())
            {
                if (string.IsNullOrEmpty(path)) continue;

                await Task.Run(() =>
                {
                    try
                    {
                        var attributes = File.GetAttributes(path);
                        if (attributes.HasFlag(FileAttributes.Directory))
                        {
                            if (Directory.Exists(path))
                            {
                                Directory.Delete(path, true);
                            }
                        }
                        else
                        {
                            if (File.Exists(path))
                            {
                                File.SetAttributes(path, FileAttributes.Normal);
                                File.Delete(path);
                            }
                        }
                    }
                    catch (Exception ex) when (ex is UnauthorizedAccessException || ex is SecurityException)
                    {
                        ErrorLogger.LogError("StorageOptimizer.ExecuteCleanup", 
                            $"Access denied to {path}", ex.StackTrace);
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.LogError("StorageOptimizer.ExecuteCleanup", 
                            $"Error cleaning {path}: {ex.Message}", ex.StackTrace);
                    }
                });
            }
        }

        /// <summary>
        /// Safely empties the Windows Recycle Bin
        /// </summary>
        internal static void ClearRecycleBin()
        {
            try
            {
                SHEmptyRecycleBin(IntPtr.Zero, null, 
                    RecycleFlag.SHERB_NOSOUND | RecycleFlag.SHERB_NOCONFIRMATION);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("StorageOptimizer.ClearRecycleBin", ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Asynchronously gets the size of a file
        /// </summary>
        private static async Task<ByteSize> GetFileSizeAsync(string path)
        {
            if (string.IsNullOrEmpty(path)) return new ByteSize(0);

            try
            {
                var fileInfo = new FileInfo(path);
                if (fileInfo.Exists)
                {
                    return await Task.Run(() => ByteSize.FromBytes(fileInfo.Length));
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("StorageOptimizer.GetFileSize", 
                    $"Error getting size for {path}: {ex.Message}", ex.StackTrace);
            }

            return new ByteSize(0);
        }
    }
}
