using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace SystemEnhancer
{
    /// <summary>
    /// Main application entry point and configuration
    /// </summary>
    internal static class ApplicationManager
    {
        #region Version Configuration
        private static readonly Version ApplicationVersion = new Version(14, 8);
        private static readonly bool IsBetaRelease = false;

        /// <summary>
        /// Display scaling preference for the application
        /// </summary>
        internal static int DisplayScalingFactor;

        /// <summary>
        /// Gets the current application version as a string
        /// </summary>
        internal static string GetVersionString()
        {
            try
            {
                return ApplicationVersion.ToString(2);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ApplicationManager.GetVersionString", ex.Message, ex.StackTrace);
                return "14.8";
            }
        }

        /// <summary>
        /// Gets the current application version as a float
        /// </summary>
        internal static float GetVersionNumber()
        {
            try
            {
                return float.Parse(GetVersionString());
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ApplicationManager.GetVersionNumber", ex.Message, ex.StackTrace);
                return 14.8f;
            }
        }
        #endregion

        #region Application Configuration
        /// <summary>
        /// Enables advanced features for Windows Server environments
        /// </summary>
        internal static bool ServerFeaturesEnabled = false;

        private const string JsonLibraryName = "Optimizer.Newtonsoft.Json.dll";
        private const string MutexName = "OptimizerMutex";
        private const int MaxProcessRetries = 3;
        private const int ProcessRetryDelay = 1000; // milliseconds

        private static Mutex _applicationMutex;
        #endregion

        /// <summary>
        /// Application entry point
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                InitializeApplication();
                
                if (!EnsureSingleInstance())
                {
                    MessageBox.Show("Another instance is already running!", "Optimizer", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!ValidateEnvironment())
                {
                    MessageBox.Show("This application requires Windows 7 or higher!", "Optimizer", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ConfigureApplication();
                LaunchMainForm(args);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ApplicationManager.Main", ex.Message, ex.StackTrace);
                MessageBox.Show($"Unhandled error occurred: {ex.Message}", "Optimizer Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CleanupApplication();
            }
        }

        /// <summary>
        /// Initializes core application components
        /// </summary>
        private static void InitializeApplication()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LoadRequiredAssemblies();
            InitializeErrorLogging();
        }

        /// <summary>
        /// Ensures only one instance of the application is running
        /// </summary>
        private static bool EnsureSingleInstance()
        {
            try
            {
                _applicationMutex = new Mutex(true, MutexName, out bool createdNew);
                return createdNew;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ApplicationManager.EnsureSingleInstance", ex.Message, ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Validates the operating system environment
        /// </summary>
        private static bool ValidateEnvironment()
        {
            try
            {
                return Environment.OSVersion.Version.Major >= 6;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ApplicationManager.ValidateEnvironment", ex.Message, ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Configures application settings and dependencies
        /// </summary>
        private static void ConfigureApplication()
        {
            try
            {
                ConfigurationManager.LoadConfiguration();
                LoadLanguageResources();
                ConfigureDisplayScaling();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ApplicationManager.ConfigureApplication", ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Launches the main application form
        /// </summary>
        private static void LaunchMainForm(string[] args)
        {
            try
            {
                MainForm mainForm = new MainForm(args);
                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ApplicationManager.LaunchMainForm", ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Loads required assemblies for the application
        /// </summary>
        private static void LoadRequiredAssemblies()
        {
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                {
                    string resourceName = JsonLibraryName;
                    if (!args.Name.Contains("Newtonsoft.Json")) return null;

                    using (Stream stream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream(resourceName))
                    {
                        if (stream == null) return null;
                        byte[] assemblyData = new byte[stream.Length];
                        stream.Read(assemblyData, 0, assemblyData.Length);
                        return Assembly.Load(assemblyData);
                    }
                };
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ApplicationManager.LoadRequiredAssemblies", ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Initializes error logging system
        /// </summary>
        private static void InitializeErrorLogging()
        {
            try
            {
                string logDirectory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Optimizer",
                    "Logs"
                );

                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                // Clean old log files
                CleanupOldLogFiles(logDirectory);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing logging: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads language resources for the application
        /// </summary>
        private static void LoadLanguageResources()
        {
            try
            {
                ConfigurationManager.LoadTranslationData(ConfigurationManager.CurrentConfig.UserInterfaceLanguage);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ApplicationManager.LoadLanguageResources", ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Configures display scaling settings
        /// </summary>
        private static void ConfigureDisplayScaling()
        {
            try
            {
                DisplayScalingFactor = (int)(100f * Screen.PrimaryScreen.Bounds.Width / SystemInformation.PrimaryMonitorSize.Width);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ApplicationManager.ConfigureDisplayScaling", ex.Message, ex.StackTrace);
                DisplayScalingFactor = 100;
            }
        }

        /// <summary>
        /// Cleans up application resources
        /// </summary>
        private static void CleanupApplication()
        {
            try
            {
                _applicationMutex?.Close();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ApplicationManager.CleanupApplication", ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Removes old log files from the specified directory
        /// </summary>
        private static void CleanupOldLogFiles(string logDirectory)
        {
            try
            {
                var oldFiles = Directory.GetFiles(logDirectory, "*.log")
                    .Select(f => new FileInfo(f))
                    .Where(f => f.CreationTime < DateTime.Now.AddDays(-7))
                    .ToList();

                foreach (var file in oldFiles)
                {
                    try
                    {
                        file.Delete();
                    }
                    catch
                    {
                        // Ignore deletion errors for old logs
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("ApplicationManager.CleanupOldLogFiles", ex.Message, ex.StackTrace);
            }
        }
    }
}
