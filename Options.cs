using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Optimizer
{
    /// <summary>
    /// Represents the application configuration settings that can be serialized to JSON
    /// </summary>
    [Serializable]
    public sealed class ApplicationConfiguration
    {
        // UI and Application Settings
        public Color InterfaceTheme { get; set; }
        public string ApplicationsDirectory { get; set; }
        public bool SystemTrayEnabled { get; set; }
        public bool LaunchAtStartup { get; set; }
        public bool DarkThemeEnabled { get; set; }
        public LanguageCode UserInterfaceLanguage { get; set; }

        // System Performance Settings
        public bool OptimizeSystemPerformance { get; set; }
        public bool OptimizeNetworkSettings { get; set; }
        public bool DisableWindowsSecurity { get; set; }
        public bool DisableSystemBackup { get; set; }
        public bool DisablePrinterService { get; set; }
        public bool DisableMediaSharing { get; set; }
        public bool DisableErrorReporting { get; set; }
        public bool DisableHomeGroupServices { get; set; }
        public bool OptimizeSystemCache { get; set; }
        public bool DisableDataCollection { get; set; }
        public bool DisableCompatAssist { get; set; }
        public bool DisableFaxServices { get; set; }
        public bool DisableSecurityScreen { get; set; }
        public bool DisableCloudSync { get; set; }
        public bool DisableAccessibilityKeys { get; set; }
        public bool DisablePowerSaving { get; set; }
        public bool DisableFileSharingV1 { get; set; }
        public bool DisableFileSharingV2 { get; set; }
        public bool OptimizeFileSystem { get; set; }
        public bool OptimizeSearchService { get; set; }

        // Application Telemetry Settings
        public bool DisableOfficeTelemetry { get; set; }
        public bool DisableVSTelemetry { get; set; }
        public bool DisableFirefoxMetrics { get; set; }
        public bool DisableChromeMetrics { get; set; }
        public bool DisableGPUTelemetry { get; set; }

        // Cloud Services
        public bool DisableCloudStorage { get; set; }
    }

    /// <summary>
    /// Manages application settings, themes, and translations
    /// </summary>
    public static class ConfigurationManager
    {
        private static readonly string ConfigurationFilePath = Path.Combine(Required.CoreFolder, "Config.json");
        private static readonly string TranslationsDirectory = Path.Combine(Required.CoreFolder, "Langs");
        
        private static JObject _translationData;
        public static ApplicationConfiguration CurrentConfig { get; private set; }

        /// <summary>
        /// Calculates the optimal contrast color for text based on the background
        /// </summary>
        public static Color GetOptimalTextColor(Color background)
        {
            int brightness = (background.R * 299 + background.G * 587 + background.B * 114) / 1000;
            return brightness > 128 ? Color.Black : Color.White;
        }

        /// <summary>
        /// Applies the current theme to a control and its children
        /// </summary>
        public static void ApplyThemeToControl(Control control)
        {
            if (control == null) return;

            try
            {
                if (CurrentConfig.DarkThemeEnabled)
                {
                    control.BackColor = CurrentConfig.InterfaceTheme;
                    control.ForeColor = GetOptimalTextColor(control.BackColor);
                }

                foreach (Control childControl in control.Controls)
                {
                    ApplyThemeToControl(childControl);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ConfigurationManager.ApplyThemeToControl", ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates the application theme mode
        /// </summary>
        public static void UpdateApplicationTheme()
        {
            try
            {
                if (CurrentConfig.DarkThemeEnabled)
                {
                    CurrentConfig.InterfaceTheme = ColorTranslator.FromHtml("#1F1F1F");
                }
                else
                {
                    CurrentConfig.InterfaceTheme = SystemColors.Control;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ConfigurationManager.UpdateApplicationTheme", ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Sets the application theme color
        /// </summary>
        public static void SetApplicationTheme(Color themeColor)
        {
            try
            {
                CurrentConfig.InterfaceTheme = themeColor;
                SaveConfiguration();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ConfigurationManager.SetApplicationTheme", ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Saves the current configuration to file
        /// </summary>
        public static void SaveConfiguration()
        {
            try
            {
                string configJson = JsonConvert.SerializeObject(CurrentConfig, Formatting.Indented);
                File.WriteAllText(ConfigurationFilePath, configJson);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ConfigurationManager.SaveConfiguration", ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Loads configuration from file or creates default if not exists
        /// </summary>
        public static void LoadConfiguration()
        {
            try
            {
                if (!File.Exists(ConfigurationFilePath))
                {
                    InitializeDefaultConfiguration();
                    return;
                }

                string configJson = File.ReadAllText(ConfigurationFilePath);
                CurrentConfig = JsonConvert.DeserializeObject<ApplicationConfiguration>(configJson);
                ValidateConfiguration();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ConfigurationManager.LoadConfiguration", ex.Message, ex.StackTrace);
                InitializeDefaultConfiguration();
            }
        }

        /// <summary>
        /// Validates the current configuration
        /// </summary>
        private static void ValidateConfiguration()
        {
            if (CurrentConfig == null)
            {
                InitializeDefaultConfiguration();
                return;
            }

            // Ensure critical paths exist
            if (string.IsNullOrEmpty(CurrentConfig.ApplicationsDirectory))
            {
                CurrentConfig.ApplicationsDirectory = Path.Combine(Required.CoreFolder, "AppsList");
            }

            if (!Directory.Exists(CurrentConfig.ApplicationsDirectory))
            {
                Directory.CreateDirectory(CurrentConfig.ApplicationsDirectory);
            }
        }

        /// <summary>
        /// Initializes default configuration settings
        /// </summary>
        private static void InitializeDefaultConfiguration()
        {
            CurrentConfig = new ApplicationConfiguration
            {
                InterfaceTheme = SystemColors.Control,
                ApplicationsDirectory = Path.Combine(Required.CoreFolder, "AppsList"),
                SystemTrayEnabled = true,
                LaunchAtStartup = false,
                DarkThemeEnabled = false,
                UserInterfaceLanguage = LanguageCode.EN,
                OptimizeSystemPerformance = false,
                OptimizeNetworkSettings = false,
                DisableWindowsSecurity = false,
                DisableSystemBackup = false,
                DisablePrinterService = false,
                DisableMediaSharing = false,
                DisableErrorReporting = false,
                DisableHomeGroupServices = false,
                OptimizeSystemCache = false,
                DisableDataCollection = false,
                DisableCompatAssist = false,
                DisableFaxServices = false,
                DisableSecurityScreen = false,
                DisableCloudSync = false,
                DisableAccessibilityKeys = false,
                DisablePowerSaving = false,
                DisableFileSharingV1 = false,
                DisableFileSharingV2 = false,
                OptimizeFileSystem = false,
                OptimizeSearchService = false,
                DisableOfficeTelemetry = false,
                DisableVSTelemetry = false,
                DisableFirefoxMetrics = false,
                DisableChromeMetrics = false,
                DisableGPUTelemetry = false,
                DisableCloudStorage = false
            };

            SaveConfiguration();
        }

        /// <summary>
        /// Loads translation data for the specified language
        /// </summary>
        public static void LoadTranslationData(LanguageCode languageCode)
        {
            try
            {
                string translationFile = Path.Combine(TranslationsDirectory, $"{languageCode}.json");
                if (!File.Exists(translationFile))
                {
                    translationFile = Path.Combine(TranslationsDirectory, "EN.json");
                }

                string jsonContent = File.ReadAllText(translationFile);
                _translationData = JObject.Parse(jsonContent);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ConfigurationManager.LoadTranslationData", ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Gets the translated text for a given key
        /// </summary>
        public static string GetTranslatedString(string key)
        {
            try
            {
                return _translationData?[key]?.ToString() ?? key;
            }
            catch
            {
                return key;
            }
        }
    }
}
