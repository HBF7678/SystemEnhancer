﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Optimizer.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Optimizer.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Windows Registry Editor Version 5.00
        ///
        ///[HKEY_CLASSES_ROOT\Directory\shell\OpenWithCMD]
        ///@=&quot;Open Command Prompt here&quot;
        ///&quot;Icon&quot;=&quot;cmd.exe&quot;
        ///&quot;NoWorkingDirectory&quot;=&quot;&quot;
        ///
        ///[HKEY_CLASSES_ROOT\Directory\shell\OpenWithCMD\command]
        ///@=&quot;cmd.exe /s /k pushd \&quot;%V\&quot;&quot;
        ///
        ///[HKEY_CLASSES_ROOT\Directory\Background\shell\OpenWithCMD]
        ///@=&quot;Open Command Prompt here&quot;
        ///&quot;Icon&quot;=&quot;cmd.exe&quot;
        ///&quot;NoWorkingDirectory&quot;=&quot;&quot;
        ///
        ///[HKEY_CLASSES_ROOT\Directory\Background\shell\OpenWithCMD\command]
        ///@=&quot;cmd.exe /s /k pushd \&quot;%V\&quot;&quot;
        ///
        ///[HKEY_CLASSES_ROOT\Drive\shell\OpenWit [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AddOpenWithCMD {
            get {
                return ResourceManager.GetString("AddOpenWithCMD", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap banner {
            get {
                object obj = ResourceManager.GetObject("banner", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap brazil {
            get {
                object obj = ResourceManager.GetObject("brazil", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap china {
            get {
                object obj = ResourceManager.GetObject("china", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap czech {
            get {
                object obj = ResourceManager.GetObject("czech", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Windows Registry Editor Version 5.00
        ///
        ///[HKEY_CLASSES_ROOT\DesktopBackground\Shell\DesktopShortcuts]
        ///&quot;MUIVerb&quot;=&quot;Desktop Shortcuts&quot;
        ///&quot;SubCommands&quot;=&quot;theme;wallpaper;scrnsavr;desktopicons;sound;cursor;DPI;color&quot;
        ///&quot;icon&quot;=&quot;desk.cpl&quot;
        ///&quot;Position&quot;=-
        ///
        ///[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\theme]
        ///@=&quot;Change Theme&quot;
        ///&quot;icon&quot;=&quot;imageres.dll,145&quot;
        ///
        ///[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\theme\command]
        ///@=&quot;control desk [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string DesktopShortcuts {
            get {
                return ResourceManager.GetString("DesktopShortcuts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to rem USE AT OWN RISK AS IS WITHOUT WARRANTY OF ANY KIND !!!!!
        ///
        ///rem https://docs.microsoft.com/en-us/windows-hardware/customize/desktop/unattend/security-malware-windows-defender-disableantispyware
        ///rem &quot;DisableAntiSpyware&quot; is discontinued and will be ignored on client devices, as of the August 2020 (version 4.18.2007.8) update to Microsoft Defender Antivirus.
        ///
        ///rem Disable Tamper Protection First !!!!!
        ///rem https://www.tenforums.com/tutorials/123792-turn-off-tamper-protection-windows-defender-antivirus.html
        ///reg [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string DisableDefenderSafeMode1903Plus {
            get {
                return ResourceManager.GetString("DisableDefenderSafeMode1903Plus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Windows Registry Editor Version 5.00
        ///
        ///[HKEY_CURRENT_USER\Software\Policies\microsoft\office\16.0\osm\preventedapplications]
        ///&quot;accesssolution&quot;=dword:00000001
        ///&quot;olksolution&quot;=dword:00000001
        ///&quot;onenotesolution&quot;=dword:00000001
        ///&quot;pptsolution&quot;=dword:00000001
        ///&quot;projectsolution&quot;=dword:00000001
        ///&quot;publishersolution&quot;=dword:00000001
        ///&quot;visiosolution&quot;=dword:00000001
        ///&quot;wdsolution&quot;=dword:00000001
        ///&quot;xlsolution&quot;=dword:00000001
        /// 
        ///[HKEY_CURRENT_USER\Software\Policies\microsoft\office\16.0\osm\preventedsolutiontypes]
        ///&quot;agave&quot;=dword:0000000 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string DisableOfficeTelemetry {
            get {
                return ResourceManager.GetString("DisableOfficeTelemetry", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to schtasks /end /tn &quot;\Microsoft\Office\OfficeTelemetryAgentFallBack2016&quot;
        ///schtasks /change /tn &quot;\Microsoft\Office\OfficeTelemetryAgentFallBack2016&quot; /disable
        ///schtasks /end /tn &quot;\Microsoft\Office\OfficeTelemetryAgentLogOn2016&quot;
        ///schtasks /change /tn &quot;\Microsoft\Office\OfficeTelemetryAgentLogOn2016&quot; /disable
        ///
        ///schtasks /end /tn &quot;\Microsoft\Office\OfficeTelemetryAgentFallBack&quot;
        ///schtasks /change /tn &quot;\Microsoft\Office\OfficeTelemetryAgentFallBack&quot; /disable
        ///schtasks /end /tn &quot;\Microsoft\Office\OfficeTelemetryAgentLogOn&quot; [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string DisableOfficeTelemetryTasks {
            get {
                return ResourceManager.GetString("DisableOfficeTelemetryTasks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to schtasks /end /tn &quot;\Microsoft\Windows\Customer Experience Improvement Program\Consolidator&quot;
        ///schtasks /change /tn &quot;\Microsoft\Windows\Customer Experience Improvement Program\Consolidator&quot; /disable
        ///schtasks /end /tn &quot;\Microsoft\Windows\Customer Experience Improvement Program\BthSQM&quot;
        ///schtasks /change /tn &quot;\Microsoft\Windows\Customer Experience Improvement Program\BthSQM&quot; /disable
        ///schtasks /end /tn &quot;\Microsoft\Windows\Customer Experience Improvement Program\KernelCeipTask&quot;
        ///schtasks /change /tn &quot;\Microsoft\Windo [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string DisableTelemetryTasks {
            get {
                return ResourceManager.GetString("DisableTelemetryTasks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to schtasks /end /tn &quot;\Microsoft\XblGameSave\XblGameSaveTask&quot;
        ///schtasks /change /tn &quot;\Microsoft\XblGameSave\XblGameSaveTask&quot; /disable
        ///schtasks /end /tn &quot;\Microsoft\XblGameSave\XblGameSaveTaskLogon&quot;
        ///schtasks /change /tn &quot;\Microsoft\XblGameSave\XblGameSaveTaskLogon&quot; /disable
        ///.
        /// </summary>
        internal static string DisableXboxTasks {
            get {
                return ResourceManager.GetString("DisableXboxTasks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap dutch {
            get {
                object obj = ResourceManager.GetObject("dutch", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap egypt {
            get {
                object obj = ResourceManager.GetObject("egypt", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///	&quot;subSystem&quot;: &quot;System&quot;,
        ///	&quot;subPrivacy&quot;: &quot;Privacy&quot;,
        ///	&quot;subGaming&quot;: &quot;Gaming&quot;,
        ///	&quot;subTouch&quot;: &quot;Touch&quot;,
        ///	&quot;subTaskbar&quot;: &quot;Taskbar&quot;,
        ///	&quot;subExtras&quot;: &quot;Extras&quot;,
        ///	&quot;btnAbout&quot;: &quot;OK&quot;,
        ///	&quot;restartButton&quot;: &quot;Restart now&quot;,
        ///	&quot;restartButton8&quot;: &quot;Restart now&quot;,
        ///	&quot;restartButton10&quot;: &quot;Restart now&quot;,
        ///	&quot;btnFind&quot;: &quot;Find&quot;,
        ///	&quot;btnKill&quot;: &quot;Kill&quot;,
        ///	&quot;trayUnlocker&quot;: &quot;File Handles&quot;,
        ///	&quot;restartAndApply&quot;: &quot;Restart to apply changes&quot;,
        ///	&quot;txtVersion&quot;: &quot;Version: {VN}&quot;,
        ///	&quot;txtBitness&quot;: &quot;You are working with {BITS}&quot;,
        ///	&quot;linkUpdate&quot;: &quot;Update available&quot;,
        ///	&quot;lblLab&quot; [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string EN {
            get {
                return ResourceManager.GetString("EN", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to reg add &quot;HKLM\Software\Microsoft\Windows Defender\Features&quot; /v &quot;TamperProtection&quot; /t REG_DWORD /d &quot;1&quot; /f
        ///
        ///reg add &quot;HKLM\System\CurrentControlSet\Services\SgrmBroker&quot; /v &quot;Start&quot; /t REG_DWORD /d &quot;2&quot; /f
        ///
        ///reg add &quot;HKLM\System\CurrentControlSet\Services\SecurityHealthService&quot; /v &quot;Start&quot; /t REG_DWORD /d &quot;2&quot; /f
        ///
        ///rem 1 - Disable Real-time protection
        ///reg delete &quot;HKLM\Software\Policies\Microsoft\Windows Defender&quot; /f
        ///reg add &quot;HKLM\Software\Policies\Microsoft\Windows Defender&quot; /v &quot;DisableAntiSpyware&quot; /t REG_DWORD /d &quot;0 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string EnableDefenderSafeMode1903Plus {
            get {
                return ResourceManager.GetString("EnableDefenderSafeMode1903Plus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Windows Registry Editor Version 5.00
        ///
        ///[HKEY_CURRENT_USER\Software\Policies\microsoft\office\16.0\osm\preventedapplications]
        ///&quot;accesssolution&quot;=-
        ///&quot;olksolution&quot;=-
        ///&quot;onenotesolution&quot;=-
        ///&quot;pptsolution&quot;=-
        ///&quot;projectsolution&quot;=-
        ///&quot;publishersolution&quot;=-
        ///&quot;visiosolution&quot;=-
        ///&quot;wdsolution&quot;=-
        ///&quot;xlsolution&quot;=-
        /// 
        ///[HKEY_CURRENT_USER\Software\Policies\microsoft\office\16.0\osm\preventedsolutiontypes]
        ///&quot;agave&quot;=-
        ///&quot;appaddins&quot;=-
        ///&quot;comaddins&quot;=-
        ///&quot;documentfiles&quot;=-
        ///&quot;templatefiles&quot;=-
        ///.
        /// </summary>
        internal static string EnableOfficeTelemetry {
            get {
                return ResourceManager.GetString("EnableOfficeTelemetry", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to schtasks /change /tn &quot;\Microsoft\Office\OfficeTelemetryAgentFallBack2016&quot; /enable
        ///schtasks /change /tn &quot;\Microsoft\Office\OfficeTelemetryAgentLogOn2016&quot; /enable
        ///
        ///schtasks /change /tn &quot;\Microsoft\Office\OfficeTelemetryAgentFallBack&quot; /enable
        ///schtasks /change /tn &quot;\Microsoft\Office\OfficeTelemetryAgentLogOn&quot; /enable
        ///
        ///reg add &quot;HKCU\SOFTWARE\Microsoft\Office\15.0\Outlook\Options\Mail&quot; /v &quot;EnableLogging&quot; /t REG_DWORD /d 1 /f
        ///reg add &quot;HKCU\SOFTWARE\Microsoft\Office\16.0\Outlook\Options\Mail&quot; /v &quot;EnableLogging&quot; /t  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string EnableOfficeTelemetryTasks {
            get {
                return ResourceManager.GetString("EnableOfficeTelemetryTasks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to schtasks /change /tn &quot;\Microsoft\Windows\Customer Experience Improvement Program\Consolidator&quot; /enable
        ///schtasks /change /tn &quot;\Microsoft\Windows\Customer Experience Improvement Program\BthSQM&quot; /enable
        ///schtasks /change /tn &quot;\Microsoft\Windows\Customer Experience Improvement Program\KernelCeipTask&quot; /enable
        ///schtasks /change /tn &quot;\Microsoft\Windows\Customer Experience Improvement Program\UsbCeip&quot; /enable
        ///schtasks /change /tn &quot;\Microsoft\Windows\Customer Experience Improvement Program\Uploader&quot; /enable
        ///schtasks / [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string EnableTelemetryTasks {
            get {
                return ResourceManager.GetString("EnableTelemetryTasks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to schtasks /change /tn &quot;\Microsoft\XblGameSave\XblGameSaveTask&quot; /enable
        ///schtasks /change /tn &quot;\Microsoft\XblGameSave\XblGameSaveTaskLogon&quot; /enable
        ///.
        /// </summary>
        internal static string EnableXboxTasks {
            get {
                return ResourceManager.GetString("EnableXboxTasks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap france {
            get {
                object obj = ResourceManager.GetObject("france", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap germany {
            get {
                object obj = ResourceManager.GetObject("germany", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @echo off 
        ///pushd &quot;%~dp0&quot; 
        ///dir /b %SystemRoot%\servicing\Packages\Microsoft-Windows-GroupPolicy-ClientExtensions-Package~3*.mum &gt;List.txt 
        ///dir /b %SystemRoot%\servicing\Packages\Microsoft-Windows-GroupPolicy-ClientTools-Package~3*.mum &gt;&gt;List.txt 
        ///for /f %%i in (&apos;findstr /i . List.txt 2^&gt;nul&apos;) do dism /online /norestart /add-package:&quot;%SystemRoot%\servicing\Packages\%%i&quot; 
        ///.
        /// </summary>
        internal static string GPEditEnablerInHome {
            get {
                return ResourceManager.GetString("GPEditEnablerInHome", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap greece {
            get {
                object obj = ResourceManager.GetObject("greece", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] hosts {
            get {
                object obj = ResourceManager.GetObject("hosts", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap hungary {
            get {
                object obj = ResourceManager.GetObject("hungary", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Windows Registry Editor Version 5.00
        ///
        ///[HKEY_CLASSES_ROOT\*\shell\runas]
        ///@=&quot;Take Ownership&quot;
        ///&quot;NoWorkingDirectory&quot;=&quot;&quot;
        ///
        ///[HKEY_CLASSES_ROOT\*\shell\runas\command]
        ///@=&quot;cmd.exe /c takeown /f \&quot;%1\&quot; &amp;&amp; icacls \&quot;%1\&quot; /grant administrators:F&quot;
        ///&quot;IsolatedCommand&quot;=&quot;cmd.exe /c takeown /f \&quot;%1\&quot; &amp;&amp; icacls \&quot;%1\&quot; /grant administrators:F&quot;
        ///
        ///[HKEY_CLASSES_ROOT\Directory\shell\runas]
        ///@=&quot;Take Ownership&quot;
        ///&quot;NoWorkingDirectory&quot;=&quot;&quot;
        ///
        ///[HKEY_CLASSES_ROOT\Directory\shell\runas\command]
        ///@=&quot;cmd.exe /c takeown /f \&quot;%1\&quot; /r /d y &amp;&amp; icacls \&quot;% [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string InstallTakeOwnership {
            get {
                return ResourceManager.GetString("InstallTakeOwnership", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap italy {
            get {
                object obj = ResourceManager.GetObject("italy", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap korea {
            get {
                object obj = ResourceManager.GetObject("korea", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap kurdish {
            get {
                object obj = ResourceManager.GetObject("kurdish", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap logo {
            get {
                object obj = ResourceManager.GetObject("logo", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] OneDrive_Uninstaller {
            get {
                object obj = ResourceManager.GetObject("OneDrive_Uninstaller", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap poland {
            get {
                object obj = ResourceManager.GetObject("poland", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        internal static byte[] Poppins_Regular {
            get {
                object obj = ResourceManager.GetObject("Poppins_Regular", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Windows Registry Editor Version 5.00
        ///
        ///[HKEY_CLASSES_ROOT\DesktopBackground\Shell\Power Menu]
        ///&quot;MUIVerb&quot;=&quot;Power Menu&quot;
        ///&quot;SubCommands&quot;=&quot;lock;logoff;switch;sleep;hibernate;restart;safemode;shutdown;hybridshutdown&quot;
        ///&quot;Icon&quot;=&quot;shell32.dll,215&quot;
        ///&quot;Position&quot;=&quot;bottom&quot;
        ///
        ///[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\lock]
        ///@=&quot;Lock&quot;
        ///
        ///[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\lock\command]
        ///@=&quot;Rundll32 User32.dll,LockWorkStati [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PowerMenu {
            get {
                return ResourceManager.GetString("PowerMenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Windows Registry Editor Version 5.00
        ///
        ///
        ///[-HKEY_CLASSES_ROOT\*\shell\runas]
        ///
        ///[-HKEY_CLASSES_ROOT\Directory\shell\runas]
        ///.
        /// </summary>
        internal static string RemoveTakeOwnership {
            get {
                return ResourceManager.GetString("RemoveTakeOwnership", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap romania {
            get {
                object obj = ResourceManager.GetObject("romania", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap russia {
            get {
                object obj = ResourceManager.GetObject("russia", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap spain {
            get {
                object obj = ResourceManager.GetObject("spain", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Windows Registry Editor Version 5.00
        ///
        ///[HKEY_CLASSES_ROOT\DesktopBackground\Shell\SystemShortcuts]
        ///&quot;MUIVerb&quot;=&quot;System Shortcuts&quot;
        ///&quot;SubCommands&quot;=&quot;admintools;datetime;regional;folderoptions;gmode;internetoptions;network;power;appwiz;rbin;run;search;services;sysdm;user;user2;flip3d&quot;
        ///&quot;icon&quot;=&quot;sysdm.cpl&quot;
        ///&quot;Position&quot;=-
        ///
        ///[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\admintools]
        ///@=&quot;Administrative Tools&quot;
        ///&quot;icon&quot;=&quot;imageres.dll,109&quot;
        ///
        ///[HKEY_LOCAL_MACHINE\SOFTWARE\Micros [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SystemShortcuts {
            get {
                return ResourceManager.GetString("SystemShortcuts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Windows Registry Editor Version 5.00
        ///
        ///[HKEY_CLASSES_ROOT\DesktopBackground\Shell\SystemTools]
        ///&quot;MUIVerb&quot;=&quot;System Tools&quot;
        ///&quot;SubCommands&quot;=&quot;control;cleanmgr;devmgr;event;regedit;secctr;msconfig;taskmgr;taskschd;wu&quot;
        ///&quot;icon&quot;=&quot;imageres.dll,104&quot;
        ///&quot;Position&quot;=-
        ///
        ///[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\control]
        ///@=&quot;Control Panel&quot;
        ///&quot;icon&quot;=&quot;control.exe&quot;
        ///
        ///[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\control\command]
        ///@=&quot;c [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SystemTools {
            get {
                return ResourceManager.GetString("SystemTools", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap taiwan {
            get {
                object obj = ResourceManager.GetObject("taiwan", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap turkey {
            get {
                object obj = ResourceManager.GetObject("turkey", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap ukraine {
            get {
                object obj = ResourceManager.GetObject("ukraine", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap united_kingdom {
            get {
                object obj = ResourceManager.GetObject("united_kingdom", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Windows Registry Editor Version 5.00
        ///
        ///[HKEY_CLASSES_ROOT\DesktopBackground\Shell\WindowsApps]
        ///&quot;MUIVerb&quot;=&quot;Windows Apps&quot;
        ///&quot;SubCommands&quot;=&quot;calc;chmap;cmd;dfrg;ie;notepad;paint;psr;snip;srd;srt;tsch;wmp;wordpad&quot;
        ///&quot;icon&quot;=&quot;imageres.dll,152&quot;
        ///&quot;Position&quot;=-
        ///
        ///[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\calc]
        ///@=&quot;Calculator&quot;
        ///&quot;icon&quot;=&quot;calc.exe&quot;
        ///
        ///[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\CommandStore\shell\calc\command]
        ///@=&quot;calc.exe&quot;
        ///
        ///[HK [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string WindowsApps {
            get {
                return ResourceManager.GetString("WindowsApps", resourceCulture);
            }
        }
    }
}
