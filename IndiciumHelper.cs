using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Management;

namespace Optimizer
{
    /// <summary>
    /// Provides system hardware and software information retrieval functionality
    /// </summary>
    public static class SystemInformationProvider
    {
        #region WMI Query Strings
        private const string QUERY_PROCESSOR = "SELECT * FROM Win32_Processor";
        private const string QUERY_OPERATING_SYSTEM = "SELECT * FROM Win32_OperatingSystem";
        private const string QUERY_PHYSICAL_MEMORY = "SELECT * FROM Win32_PhysicalMemory";
        private const string QUERY_MOTHERBOARD = "SELECT * FROM Win32_BaseBoard";
        private const string QUERY_STORAGE_CONTROLLER = "SELECT * FROM Win32_IDEController";
        private const string QUERY_SYSTEM = "SELECT * FROM Win32_ComputerSystem";
        private const string QUERY_FIRMWARE = "SELECT * FROM Win32_BIOS";
        #endregion

        #region Registry Keys
        private const string REGISTRY_PROCESSOR_KEY = @"HARDWARE\DESCRIPTION\System\CentralProcessor\0";
        private const string REGISTRY_PROCESSOR_NAME = "ProcessorNameString";
        #endregion

        #region Device Collections
        public static readonly List<StorageVolume> SystemVolumes = new List<StorageVolume>();
        public static readonly List<StorageVolume> OpticalDrives = new List<StorageVolume>();
        public static readonly List<StorageVolume> RemovableDevices = new List<StorageVolume>();

        public static readonly List<NetworkAdapter> PhysicalNetworkAdapters = new List<NetworkAdapter>();
        public static readonly List<NetworkAdapter> VirtualNetworkAdapters = new List<NetworkAdapter>();

        public static readonly List<InputDevice> KeyboardDevices = new List<InputDevice>();
        public static readonly List<InputDevice> PointerDevices = new List<InputDevice>();
        #endregion

        /// <summary>
        /// Retrieves detailed information about system processors
        /// </summary>
        public static List<ProcessorInfo> GetProcessorInformation()
        {
            List<ProcessorInfo> processors = new List<ProcessorInfo>();
            
            try
            {
                using (var searcher = new ManagementObjectSearcher(QUERY_PROCESSOR))
                using (var results = searcher.Get())
                {
                    foreach (ManagementObject processor in results)
                    {
                        processors.Add(new ProcessorInfo
                        {
                            Name = GetWmiPropertyValue<string>(processor, "Name"),
                            L2CacheSize = ByteSize.FromKiloBytes(GetWmiPropertyValue<double>(processor, "L2CacheSize")),
                            L3CacheSize = ByteSize.FromKiloBytes(GetWmiPropertyValue<double>(processor, "L3CacheSize")),
                            PhysicalCores = GetWmiPropertyValue<UInt32>(processor, "NumberOfCores"),
                            LogicalProcessors = GetWmiPropertyValue<UInt32>(processor, "NumberOfLogicalProcessors"),
                            Architecture = GetWmiPropertyValue<UInt16>(processor, "Architecture"),
                            MaxClockSpeed = GetWmiPropertyValue<UInt32>(processor, "MaxClockSpeed"),
                            Manufacturer = GetWmiPropertyValue<string>(processor, "Manufacturer"),
                            ProcessorId = GetWmiPropertyValue<string>(processor, "ProcessorId"),
                            SocketDesignation = GetWmiPropertyValue<string>(processor, "SocketDesignation"),
                            Description = GetWmiPropertyValue<string>(processor, "Description")
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("SystemInformationProvider.GetProcessorInformation", ex.Message, ex.StackTrace);
            }

            return processors;
        }

        /// <summary>
        /// Retrieves operating system information
        /// </summary>
        public static OperatingSystemInfo GetOperatingSystemInformation()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher(QUERY_OPERATING_SYSTEM))
                using (var results = searcher.Get())
                {
                    foreach (ManagementObject os in results)
                    {
                        return new OperatingSystemInfo
                        {
                            Name = GetWmiPropertyValue<string>(os, "Caption"),
                            Architecture = GetWmiPropertyValue<string>(os, "OSArchitecture"),
                            Version = GetWmiPropertyValue<string>(os, "Version"),
                            BuildNumber = GetWmiPropertyValue<string>(os, "BuildNumber"),
                            InstallDate = ManagementDateTimeConverter.ToDateTime(GetWmiPropertyValue<string>(os, "InstallDate")),
                            LastBootUpTime = ManagementDateTimeConverter.ToDateTime(GetWmiPropertyValue<string>(os, "LastBootUpTime")),
                            RegisteredUser = GetWmiPropertyValue<string>(os, "RegisteredUser"),
                            SerialNumber = GetWmiPropertyValue<string>(os, "SerialNumber"),
                            SystemDirectory = GetWmiPropertyValue<string>(os, "SystemDirectory")
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("SystemInformationProvider.GetOperatingSystemInformation", ex.Message, ex.StackTrace);
            }

            return null;
        }

        /// <summary>
        /// Retrieves system memory information
        /// </summary>
        public static List<MemoryModule> GetMemoryConfiguration()
        {
            List<MemoryModule> memoryModules = new List<MemoryModule>();

            try
            {
                using (var searcher = new ManagementObjectSearcher(QUERY_PHYSICAL_MEMORY))
                using (var results = searcher.Get())
                {
                    foreach (ManagementObject module in results)
                    {
                        memoryModules.Add(new MemoryModule
                        {
                            BankLabel = GetWmiPropertyValue<string>(module, "BankLabel"),
                            Capacity = ByteSize.FromBytes(GetWmiPropertyValue<UInt64>(module, "Capacity")),
                            FormFactor = GetWmiPropertyValue<UInt16>(module, "FormFactor"),
                            Manufacturer = GetWmiPropertyValue<string>(module, "Manufacturer"),
                            Speed = GetWmiPropertyValue<UInt32>(module, "Speed"),
                            MemoryType = GetWmiPropertyValue<UInt16>(module, "MemoryType")
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("SystemInformationProvider.GetMemoryConfiguration", ex.Message, ex.StackTrace);
            }

            return memoryModules;
        }

        /// <summary>
        /// Retrieves motherboard information
        /// </summary>
        public static MotherboardInfo GetMotherboardInformation()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher(QUERY_MOTHERBOARD))
                using (var results = searcher.Get())
                {
                    foreach (ManagementObject board in results)
                    {
                        return new MotherboardInfo
                        {
                            Manufacturer = GetWmiPropertyValue<string>(board, "Manufacturer"),
                            Product = GetWmiPropertyValue<string>(board, "Product"),
                            SerialNumber = GetWmiPropertyValue<string>(board, "SerialNumber"),
                            Version = GetWmiPropertyValue<string>(board, "Version")
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("SystemInformationProvider.GetMotherboardInformation", ex.Message, ex.StackTrace);
            }

            return null;
        }

        /// <summary>
        /// Safely retrieves a property value from a WMI object
        /// </summary>
        private static T GetWmiPropertyValue<T>(ManagementObject obj, string propertyName)
        {
            try
            {
                object value = obj[propertyName];
                if (value != null)
                {
                    return (T)value;
                }
            }
            catch { }

            return default(T);
        }

        /// <summary>
        /// Retrieves a registry value
        /// </summary>
        private static T GetRegistryValue<T>(string keyPath, string valueName, T defaultValue = default(T))
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath))
                {
                    if (key != null)
                    {
                        object value = key.GetValue(valueName);
                        if (value != null)
                        {
                            return (T)value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError("SystemInformationProvider.GetRegistryValue", ex.Message, ex.StackTrace);
            }

            return defaultValue;
        }
    }

    #region Information Classes
    public class ProcessorInfo
    {
        public string Name { get; set; }
        public ByteSize L2CacheSize { get; set; }
        public ByteSize L3CacheSize { get; set; }
        public UInt32 PhysicalCores { get; set; }
        public UInt32 LogicalProcessors { get; set; }
        public UInt16 Architecture { get; set; }
        public UInt32 MaxClockSpeed { get; set; }
        public string Manufacturer { get; set; }
        public string ProcessorId { get; set; }
        public string SocketDesignation { get; set; }
        public string Description { get; set; }
    }

    public class OperatingSystemInfo
    {
        public string Name { get; set; }
        public string Architecture { get; set; }
        public string Version { get; set; }
        public string BuildNumber { get; set; }
        public DateTime InstallDate { get; set; }
        public DateTime LastBootUpTime { get; set; }
        public string RegisteredUser { get; set; }
        public string SerialNumber { get; set; }
        public string SystemDirectory { get; set; }
    }

    public class MemoryModule
    {
        public string BankLabel { get; set; }
        public ByteSize Capacity { get; set; }
        public UInt16 FormFactor { get; set; }
        public string Manufacturer { get; set; }
        public UInt32 Speed { get; set; }
        public UInt16 MemoryType { get; set; }
    }

    public class MotherboardInfo
    {
        public string Manufacturer { get; set; }
        public string Product { get; set; }
        public string SerialNumber { get; set; }
        public string Version { get; set; }
    }

    public class StorageVolume
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ByteSize Capacity { get; set; }
        public ByteSize FreeSpace { get; set; }
        public string FileSystem { get; set; }
        public bool IsRemovable { get; set; }
        public string DriveType { get; set; }
    }

    public class NetworkAdapter
    {
        public string Name { get; set; }
        public string AdapterType { get; set; }
        public string MacAddress { get; set; }
        public string[] IpAddresses { get; set; }
        public bool IsEnabled { get; set; }
        public UInt32 Speed { get; set; }
    }

    public class InputDevice
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool IsWireless { get; set; }
    }
    #endregion
}
