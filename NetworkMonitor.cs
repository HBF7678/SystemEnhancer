using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace SystemEnhancer
{
    /// <summary>
    /// Monitors network adapters and provides real-time network statistics
    /// </summary>
    public class NetworkMonitor : IDisposable
    {
        private readonly Timer _monitorTimer;
        private readonly ConcurrentDictionary<string, NetworkAdapter> _adapters;
        private readonly ConcurrentDictionary<string, NetworkAdapter> _monitoredAdapters;
        private bool _isDisposed;
        private readonly object _lockObject = new object();
        private const int REFRESH_INTERVAL = 1000; // 1 second

        public event EventHandler<NetworkAdapterEventArgs> AdapterStatusChanged;
        public event EventHandler<NetworkStatsEventArgs> NetworkStatsUpdated;

        public NetworkMonitor()
        {
            _adapters = new ConcurrentDictionary<string, NetworkAdapter>();
            _monitoredAdapters = new ConcurrentDictionary<string, NetworkAdapter>();
            
            _monitorTimer = new Timer(REFRESH_INTERVAL);
            _monitorTimer.Elapsed += OnTimerElapsed;

            InitializeNetworkAdapters();
        }

        /// <summary>
        /// Initializes and enumerates available network adapters
        /// </summary>
        private void InitializeNetworkAdapters()
        {
            try
            {
                var category = new PerformanceCounterCategory("Network Interface");
                var instances = category.GetInstanceNames();

                foreach (var name in instances.Where(IsValidAdapter))
                {
                    try
                    {
                        var adapter = new NetworkAdapter(name)
                        {
                            DownloadCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", name, true),
                            UploadCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", name, true)
                        };

                        _adapters.TryAdd(name, adapter);
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.LogError("NetworkMonitor.InitializeNetworkAdapters", 
                            $"Failed to initialize adapter {name}: {ex.Message}", ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("NetworkMonitor.InitializeNetworkAdapters", 
                    "Failed to enumerate network adapters", ex.StackTrace);
            }
        }

        /// <summary>
        /// Validates if the adapter should be monitored
        /// </summary>
        private static bool IsValidAdapter(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;

            var lowerName = name.ToLowerInvariant();
            return !lowerName.Contains("loopback") &&
                   !lowerName.Contains("virtual") &&
                   !lowerName.Contains("hyper-v");
        }

        /// <summary>
        /// Timer callback to refresh network statistics
        /// </summary>
        private async void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_isDisposed) return;

            try
            {
                await Task.Run(() =>
                {
                    foreach (var adapter in _monitoredAdapters.Values)
                    {
                        adapter.Refresh();
                        NetworkStatsUpdated?.Invoke(this, new NetworkStatsEventArgs(adapter));
                    }
                });
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("NetworkMonitor.OnTimerElapsed", 
                    "Error refreshing network stats", ex.StackTrace);
            }
        }

        /// <summary>
        /// Gets all available network adapters
        /// </summary>
        public IReadOnlyList<NetworkAdapter> Adapters => _adapters.Values.ToList().AsReadOnly();

        /// <summary>
        /// Starts monitoring all available network adapters
        /// </summary>
        public async Task StartMonitoringAsync()
        {
            if (_isDisposed) throw new ObjectDisposedException(nameof(NetworkMonitor));

            await Task.Run(() =>
            {
                lock (_lockObject)
                {
                    if (!_adapters.Any()) return;

                    foreach (var adapter in _adapters.Values)
                    {
                        StartMonitoringAdapter(adapter);
                    }

                    _monitorTimer.Start();
                }
            });
        }

        /// <summary>
        /// Starts monitoring a specific network adapter
        /// </summary>
        public async Task StartMonitoringAsync(NetworkAdapter adapter)
        {
            if (_isDisposed) throw new ObjectDisposedException(nameof(NetworkMonitor));
            if (adapter == null) throw new ArgumentNullException(nameof(adapter));

            await Task.Run(() =>
            {
                lock (_lockObject)
                {
                    StartMonitoringAdapter(adapter);
                    _monitorTimer.Start();
                }
            });
        }

        private void StartMonitoringAdapter(NetworkAdapter adapter)
        {
            if (_monitoredAdapters.TryAdd(adapter.Name, adapter))
            {
                try
                {
                    adapter.Initialize();
                    AdapterStatusChanged?.Invoke(this, new NetworkAdapterEventArgs(adapter, true));
                }
                catch (Exception ex)
                {
                    ErrorLogger.LogError("NetworkMonitor.StartMonitoringAdapter", 
                        $"Failed to start monitoring adapter {adapter.Name}", ex.StackTrace);
                    _monitoredAdapters.TryRemove(adapter.Name, out _);
                }
            }
        }

        /// <summary>
        /// Stops monitoring all network adapters
        /// </summary>
        public async Task StopMonitoringAsync()
        {
            if (_isDisposed) throw new ObjectDisposedException(nameof(NetworkMonitor));

            await Task.Run(() =>
            {
                lock (_lockObject)
                {
                    _monitorTimer.Stop();
                    foreach (var adapter in _monitoredAdapters.Values)
                    {
                        AdapterStatusChanged?.Invoke(this, new NetworkAdapterEventArgs(adapter, false));
                    }
                    _monitoredAdapters.Clear();
                }
            });
        }

        /// <summary>
        /// Stops monitoring a specific network adapter
        /// </summary>
        public async Task StopMonitoringAsync(NetworkAdapter adapter)
        {
            if (_isDisposed) throw new ObjectDisposedException(nameof(NetworkMonitor));
            if (adapter == null) throw new ArgumentNullException(nameof(adapter));

            await Task.Run(() =>
            {
                lock (_lockObject)
                {
                    if (_monitoredAdapters.TryRemove(adapter.Name, out _))
                    {
                        AdapterStatusChanged?.Invoke(this, new NetworkAdapterEventArgs(adapter, false));
                    }

                    if (!_monitoredAdapters.Any())
                    {
                        _monitorTimer.Stop();
                    }
                }
            });
        }

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
                _monitorTimer.Stop();
                _monitorTimer.Dispose();

                foreach (var adapter in _adapters.Values)
                {
                    adapter.Dispose();
                }

                _adapters.Clear();
                _monitoredAdapters.Clear();
            }

            _isDisposed = true;
        }
    }

    public class NetworkAdapterEventArgs : EventArgs
    {
        public NetworkAdapter Adapter { get; }
        public bool IsMonitoring { get; }

        public NetworkAdapterEventArgs(NetworkAdapter adapter, bool isMonitoring)
        {
            Adapter = adapter;
            IsMonitoring = isMonitoring;
        }
    }

    public class NetworkStatsEventArgs : EventArgs
    {
        public NetworkAdapter Adapter { get; }

        public NetworkStatsEventArgs(NetworkAdapter adapter)
        {
            Adapter = adapter;
        }
    }
}
