using System;
using System.IO;
using System.Text;

namespace Optimizer
{
    internal static class Required
    {
        internal readonly static string CoreFolder = Path.Combine(CleanHelper.ProgramData, "Optimizer");
        internal readonly static string ReadyMadeMenusFolder = Path.Combine(CoreFolder, "ReadyMadeMenus");
        internal readonly static string ScriptsFolder = Path.Combine(CoreFolder, "Required");
        internal readonly static string ExtractedIconsFolder = Path.Combine(CoreFolder, "ExtractedIcons");
        internal readonly static string FavIconsFolder = Path.Combine(CoreFolder, "FavIcons");
        internal readonly static string StartupItemsBackupFolder = Path.Combine(CoreFolder, "StartupBackup");

        readonly static string[] readyMadeMenusItems =
        {
            Path.Combine(ReadyMadeMenusFolder, "DesktopShortcuts.reg"),
            Path.Combine(ReadyMadeMenusFolder, "SystemShortcuts.reg"),
            Path.Combine(ReadyMadeMenusFolder, "PowerMenu.reg"),
            Path.Combine(ReadyMadeMenusFolder, "SystemTools.reg"),
            Path.Combine(ReadyMadeMenusFolder, "WindowsApps.reg"),
            Path.Combine(ReadyMadeMenusFolder, "InstallTakeOwnership.reg"),
            Path.Combine(ReadyMadeMenusFolder, "RemoveTakeOwnership.reg"),
        };

        readonly static string[] readyMadeMenusFiles =
        {
            Properties.Resources.DesktopShortcuts,
            Properties.Resources.SystemShortcuts,
            Properties.Resources.PowerMenu,
            Properties.Resources.SystemTools,
            Properties.Resources.WindowsApps,
            Properties.Resources.InstallTakeOwnership,
            Properties.Resources.RemoveTakeOwnership
        };

        readonly static string[] scriptItems =
        {
            Path.Combine(ScriptsFolder, "DisableOfficeTelemetryTasks.bat"),
            Path.Combine(ScriptsFolder, "DisableOfficeTelemetryTasks.reg"),
            Path.Combine(ScriptsFolder, "EnableOfficeTelemetryTasks.bat"),
            Path.Combine(ScriptsFolder, "EnableOfficeTelemetryTasks.reg"),
            Path.Combine(ScriptsFolder, "DisableTelemetryTasks.bat"),
            Path.Combine(ScriptsFolder, "EnableTelemetryTasks.bat"),
            Path.Combine(ScriptsFolder, "DisableXboxTasks.bat"),
            Path.Combine(ScriptsFolder, "EnableXboxTasks.bat"),
            Path.Combine(ScriptsFolder, "OneDrive_Uninstaller.cmd"),
            Path.Combine(ScriptsFolder, "GPEditEnablerInHome.bat"),
            Path.Combine(ScriptsFolder, "AddOpenWithCMD.reg")
        };

        readonly static string[] scriptFiles =
        {
            Properties.Resources.DisableOfficeTelemetryTasks,
            Properties.Resources.DisableOfficeTelemetry,
            Properties.Resources.EnableOfficeTelemetryTasks,
            Properties.Resources.EnableOfficeTelemetry,
            Properties.Resources.DisableTelemetryTasks,
            Properties.Resources.EnableTelemetryTasks,
            Properties.Resources.DisableXboxTasks,
            Properties.Resources.EnableXboxTasks,
            Encoding.UTF8.GetString(Properties.Resources.OneDrive_Uninstaller),
            Properties.Resources.GPEditEnablerInHome,
            Properties.Resources.AddOpenWithCMD
        };

        internal static void Deploy()
        {
            try
            {
                // Create all required directories
                Directory.CreateDirectory(CoreFolder);
                Directory.CreateDirectory(ReadyMadeMenusFolder);
                Directory.CreateDirectory(ScriptsFolder);
                Directory.CreateDirectory(ExtractedIconsFolder);
                Directory.CreateDirectory(FavIconsFolder);
                Directory.CreateDirectory(StartupItemsBackupFolder);

                // Extract ready-made menus
                for (int i = 0; i < readyMadeMenusItems.Length; i++)
                {
                    if (!File.Exists(readyMadeMenusItems[i]))
                    {
                        File.WriteAllText(readyMadeMenusItems[i], readyMadeMenusFiles[i], Encoding.Unicode);
                    }
                }

                // Extract scripts
                for (int i = 0; i < scriptItems.Length; i++)
                {
                    if (!File.Exists(scriptItems[i]))
                    {
                        if (scriptItems[i].Contains("OneDrive"))
                        {
                            File.WriteAllBytes(scriptItems[i], Encoding.UTF8.GetBytes(scriptFiles[i]));
                        }
                        else
                        {
                            File.WriteAllText(scriptItems[i], scriptFiles[i], Encoding.Unicode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("Required.Deploy", ex.Message, ex.StackTrace);
            }
        }
    }
}
