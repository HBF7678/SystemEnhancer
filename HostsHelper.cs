using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SystemEnhancer
{
    /// <summary>
    /// Provides functionality for managing and modifying the system's hosts file
    /// </summary>
    internal static class HostsFileManager
    {
        private static readonly string SystemNewLine = Environment.NewLine;
        private static readonly string SystemHostsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.System), 
            "drivers", 
            "etc", 
            "hosts"
        );

        private static readonly Lazy<HttpClient> NetworkClient = new Lazy<HttpClient>(() => new HttpClient());
        private static readonly object FileLock = new object();

        /// <summary>
        /// Restores the hosts file to its default state
        /// </summary>
        internal static async Task RestoreDefaultHostsConfigurationAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    lock (FileLock)
                    {
                        if (File.Exists(SystemHostsFilePath))
                        {
                            File.Delete(SystemHostsFilePath);
                        }

                        File.WriteAllBytes(SystemHostsFilePath, Properties.Resources.hosts);
                    }
                });
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("HostsFileManager.RestoreDefaultHostsConfiguration", 
                    $"Failed to restore hosts file: {ex.Message}", ex.StackTrace);
                throw new HostsFileException("Failed to restore default hosts configuration", ex);
            }
        }

        /// <summary>
        /// Retrieves the current content of the hosts file
        /// </summary>
        internal static async Task<string> GetCurrentHostsContentAsync()
        {
            try
            {
                return await Task.Run(() =>
                {
                    lock (FileLock)
                    {
                        return File.Exists(SystemHostsFilePath) 
                            ? File.ReadAllText(SystemHostsFilePath, Encoding.UTF8) 
                            : string.Empty;
                    }
                });
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("HostsFileManager.GetCurrentHostsContent", 
                    $"Failed to read hosts file: {ex.Message}", ex.StackTrace);
                return string.Empty;
            }
        }

        /// <summary>
        /// Updates the hosts file with new content while preserving essential entries
        /// </summary>
        internal static async Task UpdateHostsContentAsync(string newContent)
        {
            if (string.IsNullOrEmpty(newContent)) return;

            try
            {
                var contentBuilder = new StringBuilder();
                contentBuilder.AppendLine("# Hosts file managed by System Enhancer");
                contentBuilder.AppendLine($"# Last updated: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss UTC}");
                contentBuilder.AppendLine();
                contentBuilder.Append(newContent);

                await Task.Run(() =>
                {
                    lock (FileLock)
                    {
                        File.WriteAllText(SystemHostsFilePath, contentBuilder.ToString(), Encoding.UTF8);
                    }
                });
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("HostsFileManager.UpdateHostsContent", 
                    $"Failed to update hosts file: {ex.Message}", ex.StackTrace);
                throw new HostsFileException("Failed to update hosts content", ex);
            }
        }

        /// <summary>
        /// Retrieves the hosts entries as a collection
        /// </summary>
        internal static async Task<IReadOnlyCollection<HostsEntry>> GetHostsEntriesAsync()
        {
            var entries = new List<HostsEntry>();
            var content = await GetCurrentHostsContentAsync();

            foreach (var line in content.Split(new[] { SystemNewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#")) continue;

                var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2)
                {
                    entries.Add(new HostsEntry(parts[0], parts[1]));
                }
            }

            return entries.AsReadOnly();
        }

        /// <summary>
        /// Adds a new entry to the hosts file
        /// </summary>
        internal static async Task AddEntryAsync(HostsEntry entry)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry));

            try
            {
                var formattedEntry = $"{SystemNewLine}{entry}";
                await Task.Run(() =>
                {
                    lock (FileLock)
                    {
                        File.AppendAllText(SystemHostsFilePath, formattedEntry);
                    }
                });
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("HostsFileManager.AddEntry", 
                    $"Failed to add entry {entry}: {ex.Message}", ex.StackTrace);
                throw new HostsFileException($"Failed to add hosts entry: {entry}", ex);
            }
        }

        /// <summary>
        /// Removes an entry from the hosts file
        /// </summary>
        internal static async Task RemoveEntryAsync(HostsEntry entry)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry));

            try
            {
                await Task.Run(() =>
                {
                    lock (FileLock)
                    {
                        var lines = File.ReadAllLines(SystemHostsFilePath)
                            .Where(x => !x.Contains(entry.ToString()))
                            .ToList();
                        File.WriteAllLines(SystemHostsFilePath, lines);
                    }
                });
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("HostsFileManager.RemoveEntry", 
                    $"Failed to remove entry {entry}: {ex.Message}", ex.StackTrace);
                throw new HostsFileException($"Failed to remove hosts entry: {entry}", ex);
            }
        }

        /// <summary>
        /// Removes multiple entries from the hosts file
        /// </summary>
        internal static async Task RemoveEntriesAsync(IEnumerable<HostsEntry> entries)
        {
            if (entries == null) throw new ArgumentNullException(nameof(entries));

            try
            {
                var entriesToRemove = entries.Select(e => e.ToString()).ToHashSet();
                await Task.Run(() =>
                {
                    lock (FileLock)
                    {
                        var lines = File.ReadAllLines(SystemHostsFilePath)
                            .Where(line => !entriesToRemove.Contains(line))
                            .ToList();
                        File.WriteAllLines(SystemHostsFilePath, lines);
                    }
                });
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("HostsFileManager.RemoveEntries", 
                    $"Failed to remove multiple entries: {ex.Message}", ex.StackTrace);
                throw new HostsFileException("Failed to remove multiple hosts entries", ex);
            }
        }

        /// <summary>
        /// Gets or sets the read-only status of the hosts file
        /// </summary>
        internal static bool IsReadOnly
        {
            get
            {
                try
                {
                    lock (FileLock)
                    {
                        return new FileInfo(SystemHostsFilePath).IsReadOnly;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.LogError("HostsFileManager.IsReadOnly", 
                        $"Failed to get read-only status: {ex.Message}", ex.StackTrace);
                    return false;
                }
            }
            set
            {
                try
                {
                    lock (FileLock)
                    {
                        new FileInfo(SystemHostsFilePath).IsReadOnly = value;
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.LogError("HostsFileManager.IsReadOnly", 
                        $"Failed to set read-only status: {ex.Message}", ex.StackTrace);
                    throw new HostsFileException("Failed to set hosts file read-only status", ex);
                }
            }
        }

        /// <summary>
        /// Validates and sanitizes a hosts entry
        /// </summary>
        internal static string SanitizeEntry(string entry)
        {
            if (string.IsNullOrWhiteSpace(entry)) return string.Empty;

            // Remove multiple whitespaces and normalize to single space
            entry = Regex.Replace(entry.Trim(), @"\s+", " ");

            // Validate IP address format
            var parts = entry.Split(' ');
            if (parts.Length < 2) return string.Empty;

            if (!System.Net.IPAddress.TryParse(parts[0], out _))
            {
                return string.Empty;
            }

            return entry;
        }
    }

    /// <summary>
    /// Represents a single hosts file entry
    /// </summary>
    public class HostsEntry
    {
        public string IpAddress { get; }
        public string Hostname { get; }

        public HostsEntry(string ipAddress, string hostname)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                throw new ArgumentException("IP address cannot be empty", nameof(ipAddress));
            if (string.IsNullOrWhiteSpace(hostname))
                throw new ArgumentException("Hostname cannot be empty", nameof(hostname));

            if (!System.Net.IPAddress.TryParse(ipAddress, out _))
                throw new ArgumentException("Invalid IP address format", nameof(ipAddress));

            IpAddress = ipAddress;
            Hostname = hostname;
        }

        public override string ToString() => $"{IpAddress} {Hostname}";
    }

    /// <summary>
    /// Custom exception for hosts file operations
    /// </summary>
    public class HostsFileException : Exception
    {
        public HostsFileException(string message) : base(message) { }
        public HostsFileException(string message, Exception innerException) : base(message, innerException) { }
    }
}
