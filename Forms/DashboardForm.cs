using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management;
using System.Collections.Generic;

namespace Optimizer
{
    public partial class DashboardForm : Form
    {
        private Timer _updateTimer;
        private PerformanceCounter _cpuCounter;
        private PerformanceCounter _ramCounter;
        private PerformanceCounter _networkSentCounter;
        private PerformanceCounter _networkRecvCounter;
        private Dictionary<string, CircularProgressBar> _progressBars;
        private Dictionary<string, (float Before, float After)> _changeTracking;
        private Panel _loadingPanel;
        private Label _loadingLabel;
        private ProgressBar _loadingBar;
        private Button _resetButton;
        private Button _exportButton;

        public DashboardForm()
        {
            InitializeComponent();
            _changeTracking = new Dictionary<string, (float, float)>();
            InitializePerformanceCounters();
            SetupTimer();
            InitializeProgressBars();
            InitializeLoadingPanel();
            InitializeButtons();
            CaptureInitialMetrics();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "System Dashboard";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(20, 20, 20);
            this.ForeColor = Color.White;

            // Initialize progress bars and labels
            InitializeControls();

            this.ResumeLayout(false);
        }

        private void InitializeControls()
        {
            _progressBars = new Dictionary<string, CircularProgressBar>();

            // CPU Usage
            var cpuProgress = CreateProgressBar("CPU", 50, 50);
            _progressBars.Add("CPU", cpuProgress);

            // RAM Usage
            var ramProgress = CreateProgressBar("RAM", 250, 50);
            _progressBars.Add("RAM", ramProgress);

            // Disk Usage
            var diskProgress = CreateProgressBar("Disk", 450, 50);
            _progressBars.Add("Disk", diskProgress);

            // Network Usage
            var networkProgress = CreateProgressBar("Network", 650, 50);
            _progressBars.Add("Network", networkProgress);

            // Add system info panel
            var sysInfoPanel = new Panel
            {
                Location = new Point(50, 250),
                Size = new Size(700, 300),
                BackColor = Color.FromArgb(30, 30, 30),
                BorderStyle = BorderStyle.FixedSingle
            };

            var sysInfoLabel = new Label
            {
                Text = GetSystemInfo(),
                Location = new Point(10, 10),
                Size = new Size(680, 280),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9f)
            };

            sysInfoPanel.Controls.Add(sysInfoLabel);
            this.Controls.Add(sysInfoPanel);
        }

        private CircularProgressBar CreateProgressBar(string name, int x, int y)
        {
            var progress = new CircularProgressBar
            {
                Location = new Point(x, y),
                Size = new Size(150, 150),
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.FromArgb(0, 120, 212),
                Value = 0,
                Maximum = 100,
                Text = "0%"
            };

            var label = new Label
            {
                Text = name,
                Location = new Point(x + 60, y + 160),
                Size = new Size(100, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };

            this.Controls.Add(progress);
            this.Controls.Add(label);

            return progress;
        }

        private void InitializePerformanceCounters()
        {
            try
            {
                _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                _ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
                
                // Initialize network counters
                string networkInterface = GetActiveNetworkInterface();
                if (!string.IsNullOrEmpty(networkInterface))
                {
                    _networkSentCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", networkInterface);
                    _networkRecvCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", networkInterface);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("DashboardForm.InitializePerformanceCounters", ex.Message, ex.StackTrace);
                MessageBox.Show("Error initializing performance counters: " + ex.Message);
            }
        }

        private string GetActiveNetworkInterface()
        {
            try
            {
                PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");
                string[] instanceNames = category.GetInstanceNames();
                
                foreach (string name in instanceNames)
                {
                    using (PerformanceCounter counter = new PerformanceCounter("Network Interface", 
                           "Bytes Total/sec", name))
                    {
                        if (counter.NextValue() > 0)
                        {
                            return name;
                        }
                        System.Threading.Thread.Sleep(100);
                        if (counter.NextValue() > 0)
                        {
                            return name;
                        }
                    }
                }
                return instanceNames.Length > 0 ? instanceNames[0] : "";
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("DashboardForm.GetActiveNetworkInterface", ex.Message, ex.StackTrace);
                return "";
            }
        }

        private void SetupTimer()
        {
            _updateTimer = new Timer
            {
                Interval = 1000 // Update every second
            };
            _updateTimer.Tick += UpdateTimer_Tick;
            _updateTimer.Start();
        }

        private void InitializeProgressBars()
        {
            UpdateSystemMetrics();
        }

        private async void UpdateTimer_Tick(object sender, EventArgs e)
        {
            await UpdateSystemMetrics();
        }

        private async Task UpdateSystemMetrics()
        {
            try
            {
                // Update CPU Usage
                float cpuUsage = _cpuCounter.NextValue();
                UpdateProgressBar("CPU", (int)cpuUsage);

                // Update RAM Usage
                float ramUsage = _ramCounter.NextValue();
                UpdateProgressBar("RAM", (int)ramUsage);

                // Update Disk Usage
                float diskUsage = await GetDiskUsage();
                UpdateProgressBar("Disk", (int)diskUsage);

                // Update Network Usage
                float networkUsage = await GetNetworkUsage();
                UpdateProgressBar("Network", (int)networkUsage);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("DashboardForm.UpdateSystemMetrics", ex.Message, ex.StackTrace);
            }
        }

        private void UpdateProgressBar(string name, int value)
        {
            if (_progressBars.ContainsKey(name))
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => UpdateProgressBar(name, value)));
                    return;
                }

                _progressBars[name].Value = Math.Min(value, 100);
                _progressBars[name].Text = $"{Math.Min(value, 100)}%";

                // Update change tracking
                if (_changeTracking.ContainsKey(name))
                {
                    var (before, _) = _changeTracking[name];
                    _changeTracking[name] = (before, value);
                }

                // Update progress bar color based on change
                var change = value - _changeTracking[name].Before;
                Color progressColor;
                if (change > 5)
                    progressColor = Color.FromArgb(255, 100, 100); // Red for significant increase
                else if (change < -5)
                    progressColor = Color.FromArgb(100, 255, 100); // Green for significant decrease
                else
                    progressColor = Color.FromArgb(0, 120, 212);   // Default blue for minimal change

                _progressBars[name].ForeColor = progressColor;
            }
        }

        private void InitializeLoadingPanel()
        {
            _loadingPanel = new Panel
            {
                Location = new Point(50, 200),
                Size = new Size(700, 40),
                BackColor = Color.FromArgb(30, 30, 30),
                BorderStyle = BorderStyle.FixedSingle
            };

            _loadingLabel = new Label
            {
                Text = "Loading System Metrics...",
                Location = new Point(10, 10),
                Size = new Size(200, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9f)
            };

            _loadingBar = new ProgressBar
            {
                Location = new Point(220, 10),
                Size = new Size(470, 20),
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30
            };

            _loadingPanel.Controls.Add(_loadingLabel);
            _loadingPanel.Controls.Add(_loadingBar);
            this.Controls.Add(_loadingPanel);
        }

        private void InitializeButtons()
        {
            _resetButton = new Button
            {
                Text = "Reset Baseline",
                Location = new Point(50, 560),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(0, 120, 212),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _resetButton.Click += ResetButton_Click;

            _exportButton = new Button
            {
                Text = "Export Report",
                Location = new Point(180, 560),
                Size = new Size(120, 30),
                BackColor = Color.FromArgb(0, 120, 212),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            _exportButton.Click += ExportButton_Click;

            this.Controls.Add(_resetButton);
            this.Controls.Add(_exportButton);
        }

        private void CaptureInitialMetrics()
        {
            _changeTracking["CPU"] = (0, 0);
            _changeTracking["RAM"] = (0, 0);
            _changeTracking["Disk"] = (0, 0);
            _changeTracking["Network"] = (0, 0);
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var metric in _progressBars.Keys)
                {
                    var currentValue = _progressBars[metric].Value;
                    _changeTracking[metric] = (currentValue, currentValue);
                }
                MessageBox.Show("Baseline metrics have been reset.", "Reset Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("DashboardForm.ResetButton_Click", ex.Message, ex.StackTrace);
            }
        }

        private async void ExportButton_Click(object sender, EventArgs e)
        {
            try
            {
                var report = GenerateReport();
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), 
                                     $"SystemMetrics_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
                await File.WriteAllTextAsync(path, report);
                MessageBox.Show($"Report exported to:\n{path}", "Export Complete", 
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("DashboardForm.ExportButton_Click", ex.Message, ex.StackTrace);
                MessageBox.Show("Error exporting report: " + ex.Message);
            }
        }

        private string GenerateReport()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("System Metrics Report");
            sb.AppendLine("===================");
            sb.AppendLine($"Generated: {DateTime.Now}");
            sb.AppendLine();

            foreach (var metric in _changeTracking.Keys)
            {
                var (before, after) = _changeTracking[metric];
                var change = after - before;
                var arrow = change > 0 ? "↑" : change < 0 ? "↓" : "→";
                sb.AppendLine($"{metric}:");
                sb.AppendLine($"  Baseline: {before:F1}%");
                sb.AppendLine($"  Current:  {after:F1}%");
                sb.AppendLine($"  Change:   {change:F1}% {arrow}");
                sb.AppendLine();
            }

            sb.AppendLine("System Information");
            sb.AppendLine("-----------------");
            sb.AppendLine(GetSystemInfo());

            return sb.ToString();
        }

        private async Task<float> GetDiskUsage()
        {
            try
            {
                var drives = System.IO.DriveInfo.GetDrives();
                float totalSpace = 0;
                float usedSpace = 0;

                foreach (var drive in drives)
                {
                    if (drive.IsReady)
                    {
                        totalSpace += drive.TotalSize;
                        usedSpace += drive.TotalSize - drive.AvailableFreeSpace;
                    }
                }

                return (usedSpace / totalSpace) * 100;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("DashboardForm.GetDiskUsage", ex.Message, ex.StackTrace);
                return 0;
            }
        }

        private async Task<float> GetNetworkUsage()
        {
            try
            {
                if (_networkSentCounter == null || _networkRecvCounter == null)
                    return 0;

                // Get bytes per second
                float bytesSent = _networkSentCounter.NextValue();
                float bytesReceived = _networkRecvCounter.NextValue();
                
                // Convert to Mbps
                float totalMbps = (bytesSent + bytesReceived) * 8 / 1_000_000;
                
                // Assume 1Gbps network card (adjust if needed)
                const float maxMbps = 1000;
                
                // Return as percentage of max speed
                return Math.Min((totalMbps / maxMbps) * 100, 100);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("DashboardForm.GetNetworkUsage", ex.Message, ex.StackTrace);
                return 0;
            }
        }

        private string GetSystemInfo()
        {
            try
            {
                var info = new System.Text.StringBuilder();
                info.AppendLine("System Information:");
                info.AppendLine("------------------");
                
                // OS Info
                info.AppendLine($"OS: {Environment.OSVersion}");
                info.AppendLine($"Machine Name: {Environment.MachineName}");
                info.AppendLine($"Processors: {Environment.ProcessorCount}");
                
                // Memory Info
                var computerInfo = new Microsoft.VisualBasic.Devices.ComputerInfo();
                var totalMemory = computerInfo.TotalPhysicalMemory / (1024 * 1024 * 1024.0);
                var availableMemory = computerInfo.AvailablePhysicalMemory / (1024 * 1024 * 1024.0);
                
                info.AppendLine($"Total Memory: {totalMemory:F2} GB");
                info.AppendLine($"Available Memory: {availableMemory:F2} GB");
                
                // Disk Info
                info.AppendLine("\nDisk Information:");
                foreach (var drive in System.IO.DriveInfo.GetDrives())
                {
                    if (drive.IsReady)
                    {
                        var totalSize = drive.TotalSize / (1024 * 1024 * 1024.0);
                        var freeSpace = drive.AvailableFreeSpace / (1024 * 1024 * 1024.0);
                        info.AppendLine($"Drive {drive.Name}: {freeSpace:F2} GB free of {totalSize:F2} GB");
                    }
                }

                return info.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("DashboardForm.GetSystemInfo", ex.Message, ex.StackTrace);
                return "Error retrieving system information";
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            
            _updateTimer?.Stop();
            _updateTimer?.Dispose();
            _cpuCounter?.Dispose();
            _ramCounter?.Dispose();
            _networkSentCounter?.Dispose();
            _networkRecvCounter?.Dispose();
        }
    }

    public class CircularProgressBar : Control
    {
        private int _value;
        private int _maximum = 100;
        private Color _foreColor = Color.FromArgb(0, 120, 212);
        private string _text = "0%";

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                _text = $"{value}%";
                Invalidate();
            }
        }

        public int Maximum
        {
            get => _maximum;
            set
            {
                _maximum = value;
                Invalidate();
            }
        }

        public override Color ForeColor
        {
            get => _foreColor;
            set
            {
                _foreColor = value;
                Invalidate();
            }
        }

        public override string Text
        {
            get => _text;
            set
            {
                _text = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                var rect = new Rectangle(0, 0, Width - 1, Height - 1);
                path.AddEllipse(rect);
                e.Graphics.FillPath(new SolidBrush(BackColor), path);
                e.Graphics.DrawPath(new Pen(Color.FromArgb(50, 50, 50), 1), path);

                var angle = (float)(_value * 360.0 / _maximum);
                if (angle > 0)
                {
                    using (var brush = new SolidBrush(_foreColor))
                    {
                        e.Graphics.FillPie(brush, rect, -90, angle);
                    }
                }

                using (var font = new Font("Segoe UI", 12f, FontStyle.Bold))
                using (var brush = new SolidBrush(Color.White))
                {
                    var textSize = e.Graphics.MeasureString(_text, font);
                    e.Graphics.DrawString(_text, font, brush,
                        Width / 2 - textSize.Width / 2,
                        Height / 2 - textSize.Height / 2);
                }
            }
        }
    }
}
