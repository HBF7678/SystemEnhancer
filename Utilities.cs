using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemEnhancer
{
    /// <summary>
    /// Provides system-level utility functions for Windows optimization and management
    /// </summary>
    internal static class SystemUtilities
    {
        private static readonly string _productName = string.Empty;
        private static readonly string _buildNumber = string.Empty;
        
        internal static WindowsVersion CurrentWindowsVersion { get; private set; } = WindowsVersion.Unsupported;

        /// <summary>
        /// Sets a control property in a thread-safe manner
        /// </summary>
        internal static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            try
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(new Action(() => SetControlPropertyThreadSafe(control, propertyName, propertyValue)));
                }
                else
                {
                    control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new[] { propertyValue });
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("SystemUtilities.SetControlPropertyThreadSafe", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Recursively gets all child controls of a parent control
        /// </summary>
        internal static IEnumerable<Control> GetAllChildControls(Control parent)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            return parent.Controls.Cast<Control>()
                .SelectMany(control => GetAllChildControls(control))
                .Concat(new[] { parent });
        }

        /// <summary>
        /// Converts a color to grayscale
        /// </summary>
        internal static Color ToGrayScale(this Color color)
        {
            if (color == Color.Transparent) return color;
            
            int grayScale = (int)((color.R * 0.299) + (color.G * 0.587) + (color.B * 0.114));
            return Color.FromArgb(grayScale, grayScale, grayScale);
        }

        /// <summary>
        /// Gets the Windows 10 build version
        /// </summary>
        internal static string GetWindows10Build()
        {
            try
            {
                return Registry.GetValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                    "DisplayVersion",
                    string.Empty)?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("SystemUtilities.GetWindows10Build", ex.Message, ex.StackTrace);
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the current Windows OS version
        /// </summary>
        internal static string GetOS()
        {
            try
            {
                var productName = Registry.GetValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                    "ProductName",
                    string.Empty)?.ToString() ?? string.Empty;

                CurrentWindowsVersion = DetermineWindowsVersion(productName);
                return productName;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("SystemUtilities.GetOS", ex.Message, ex.StackTrace);
                return "Unknown Windows Version";
            }
        }

        private static WindowsVersion DetermineWindowsVersion(string productName)
        {
            if (string.IsNullOrEmpty(productName)) return WindowsVersion.Unsupported;

            if (productName.Contains("Windows 7")) return WindowsVersion.Windows7;
            if (productName.Contains("Windows 8")) return WindowsVersion.Windows8;

            if (productName.Contains("Windows 10"))
            {
                var buildNumber = Registry.GetValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                    "CurrentBuild",
                    string.Empty)?.ToString();

                if (!string.IsNullOrEmpty(buildNumber) && int.TryParse(buildNumber, out int build))
                {
                    return build >= 22000 ? WindowsVersion.Windows11 : WindowsVersion.Windows10;
                }
            }

            if (Program.UNSAFE_MODE)
            {
                if (productName.Contains("Windows Server 2008")) return WindowsVersion.Windows7;
                if (productName.Contains("Windows Server 2012")) return WindowsVersion.Windows8;
                if (productName.Contains("Windows Server 2016") || 
                    productName.Contains("Windows Server 2019") || 
                    productName.Contains("Windows Server 2022")) return WindowsVersion.Windows10;
            }

            return WindowsVersion.Unsupported;
        }

        /// <summary>
        /// Gets the system architecture (32/64 bit)
        /// </summary>
        internal static string GetSystemArchitecture() => 
            Environment.Is64BitOperatingSystem ? "64-bit system" : "32-bit system";

        /// <summary>
        /// Checks if the current process has administrative privileges
        /// </summary>
        internal static bool IsAdministrator() =>
            new WindowsPrincipal(WindowsIdentity.GetCurrent())
                .IsInRole(WindowsBuiltInRole.Administrator);

        /// <summary>
        /// Checks if the current Windows version is compatible
        /// </summary>
        internal static bool IsCompatibleWindows()
        {
            var os = GetOS();
            return !os.Contains("XP") && 
                   !os.Contains("Vista") && 
                   !os.Contains("Server 2003");
        }

        /// <summary>
        /// Runs a batch file asynchronously
        /// </summary>
        internal static async Task RunBatchFileAsync(string batchFile)
        {
            if (!File.Exists(batchFile))
                throw new FileNotFoundException("Batch file not found", batchFile);

            try
            {
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        CreateNoWindow = true,
                        FileName = batchFile,
                        UseShellExecute = false
                    }
                };

                process.Start();
                await process.WaitForExitAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("SystemUtilities.RunBatchFileAsync", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Imports a registry script
        /// </summary>
        internal static async Task ImportRegistryScriptAsync(string scriptFile)
        {
            if (!File.Exists(scriptFile))
                throw new FileNotFoundException("Registry script not found", scriptFile);

            try
            {
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "regedit.exe",
                        Arguments = $"/s \"{scriptFile}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                await process.WaitForExitAsync();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("SystemUtilities.ImportRegistryScriptAsync", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Restarts the system
        /// </summary>
        internal static void RestartSystem()
        {
            try
            {
                Options.SaveSettings();
                Process.Start("shutdown.exe", "/r /t 0");
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("SystemUtilities.RestartSystem", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Manages system hibernation
        /// </summary>
        internal static async Task ManageHibernationAsync(bool enable)
        {
            try
            {
                await RunCommandAsync($"powercfg -h {(enable ? "on" : "off")}");
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("SystemUtilities.ManageHibernationAsync", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Runs a command asynchronously
        /// </summary>
        internal static async Task RunCommandAsync(string command)
        {
            if (string.IsNullOrEmpty(command))
                throw new ArgumentNullException(nameof(command));

            try
            {
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = "cmd.exe",
                        Arguments = $"/C {command}",
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true
                    }
                };

                process.Start();
                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    var error = await process.StandardError.ReadToEndAsync();
                    throw new Exception($"Command failed with exit code {process.ExitCode}: {error}");
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("SystemUtilities.RunCommandAsync", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Manages Windows services
        /// </summary>
        internal static async Task ManageServiceAsync(string serviceName, bool start)
        {
            if (string.IsNullOrEmpty(serviceName))
                throw new ArgumentNullException(nameof(serviceName));

            try
            {
                if (!ServiceExists(serviceName)) return;

                using var sc = new ServiceController(serviceName);
                if (start)
                {
                    if (sc.Status != ServiceControllerStatus.Running)
                    {
                        sc.Start();
                        await Task.Run(() => sc.WaitForStatus(ServiceControllerStatus.Running));
                    }
                }
                else
                {
                    if (sc.Status != ServiceControllerStatus.Stopped && sc.CanStop)
                    {
                        sc.Stop();
                        await Task.Run(() => sc.WaitForStatus(ServiceControllerStatus.Stopped));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("SystemUtilities.ManageServiceAsync", ex.Message, ex.StackTrace);
                throw;
            }
        }

        private static bool ServiceExists(string serviceName) =>
            ServiceController.GetServices().Any(sc => sc.ServiceName.Equals(serviceName, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Sanitizes a file or folder name by removing invalid characters
        /// </summary>
        internal static string SanitizeFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            var invalids = Path.GetInvalidFileNameChars();
            return string.Join("_", fileName.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
        }

        /// <summary>
        /// Takes ownership of a registry key
        /// </summary>
        internal static void TakeRegistryKeyOwnership(RegistryKey key, string subKeyName)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrEmpty(subKeyName)) throw new ArgumentNullException(nameof(subKeyName));

            try
            {
                using var subKey = key.OpenSubKey(subKeyName, RegistryKeyPermissionCheck.ReadWriteSubTree, 
                    RegistryRights.TakeOwnership | RegistryRights.ChangePermissions);

                var security = subKey.GetAccessControl();
                var currentUser = WindowsIdentity.GetCurrent().User;
                
                security.SetOwner(currentUser);
                security.SetAccessRule(new RegistryAccessRule(
                    currentUser,
                    RegistryRights.FullControl,
                    InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                    PropagationFlags.None,
                    AccessControlType.Allow
                ));

                subKey.SetAccessControl(security);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("SystemUtilities.TakeRegistryKeyOwnership", ex.Message, ex.StackTrace);
                throw;
            }
        }
    }
}
