using System;
using System.Diagnostics;

namespace SystemEnhancer
{
    /// <summary>
    /// Represents a network adapter and provides network speed monitoring capabilities
    /// </summary>
    public class NetworkAdapter : IDisposable
    {
        private long _downloadSpeed;
        private long _uploadSpeed;
        private long _downloadValue;
        private long _uploadValue;
        private long _downloadValueOld;
        private long _uploadValueOld;
        private bool _isDisposed;
        private readonly object _lockObject = new object();

        public string Name { get; }
        internal PerformanceCounter DownloadCounter { get; set; }
        internal PerformanceCounter UploadCounter { get; set; }

        /// <summary>
        /// Creates a new instance of NetworkAdapter
        /// </summary>
        internal NetworkAdapter(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Adapter name cannot be empty", nameof(name));

            Name = name;
        }

        /// <summary>
        /// Initializes the network adapter counters
        /// </summary>
        internal void Initialize()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(NetworkAdapter));

            try
            {
                lock (_lockObject)
                {
                    ValidateCounters();
                    _downloadValueOld = DownloadCounter.NextSample().RawValue;
                    _uploadValueOld = UploadCounter.NextSample().RawValue;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("NetworkAdapter.Initialize", 
                    $"Failed to initialize adapter {Name}: {ex.Message}", ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Refreshes the network adapter statistics
        /// </summary>
        internal void Refresh()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(NetworkAdapter));

            try
            {
                lock (_lockObject)
                {
                    ValidateCounters();
                    _downloadValue = DownloadCounter.NextSample().RawValue;
                    _uploadValue = UploadCounter.NextSample().RawValue;

                    _downloadSpeed = _downloadValue - _downloadValueOld;
                    _uploadSpeed = _uploadValue - _uploadValueOld;

                    _downloadValueOld = _downloadValue;
                    _uploadValueOld = _uploadValue;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("NetworkAdapter.Refresh", 
                    $"Failed to refresh adapter {Name}: {ex.Message}", ex.StackTrace);
                throw;
            }
        }

        private void ValidateCounters()
        {
            if (DownloadCounter == null || UploadCounter == null)
            {
                throw new InvalidOperationException(
                    "Network counters are not properly initialized. Ensure the adapter is properly configured.");
            }
        }

        #region Speed Properties
        /// <summary>
        /// Gets the current download speed in bytes per second
        /// </summary>
        public long DownloadSpeed
        {
            get
            {
                lock (_lockObject)
                {
                    return _downloadSpeed;
                }
            }
        }

        /// <summary>
        /// Gets the current upload speed in bytes per second
        /// </summary>
        public long UploadSpeed
        {
            get
            {
                lock (_lockObject)
                {
                    return _uploadSpeed;
                }
            }
        }

        /// <summary>
        /// Gets the current download speed in kilobytes per second
        /// </summary>
        public double DownloadSpeedKbps
        {
            get
            {
                lock (_lockObject)
                {
                    return _downloadSpeed / 1024.0;
                }
            }
        }

        /// <summary>
        /// Gets the current upload speed in kilobytes per second
        /// </summary>
        public double UploadSpeedKbps
        {
            get
            {
                lock (_lockObject)
                {
                    return _uploadSpeed / 1024.0;
                }
            }
        }

        /// <summary>
        /// Gets the current download speed in megabytes per second
        /// </summary>
        public double DownloadSpeedMbps
        {
            get
            {
                lock (_lockObject)
                {
                    return _downloadSpeed / (1024.0 * 1024.0);
                }
            }
        }

        /// <summary>
        /// Gets the current upload speed in megabytes per second
        /// </summary>
        public double UploadSpeedMbps
        {
            get
            {
                lock (_lockObject)
                {
                    return _uploadSpeed / (1024.0 * 1024.0);
                }
            }
        }
        #endregion

        public override string ToString() => Name;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                DownloadCounter?.Dispose();
                UploadCounter?.Dispose();
            }

            _isDisposed = true;
        }
    }
}
