using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimizer
{
    public class Optimizations
    {
        public static void CreateBackup(string selectedPath)
        {
            Directory.SetCurrentDirectory(selectedPath);
            Directory.CreateDirectory(@"Backup");

            Process process = new Process();
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.FileName = "reg";

            process.StartInfo.Arguments = $"export HKLM \"{selectedPath}\\Backup\\HKLM.Reg\" /y";
            process.Start();
            process.WaitForExit();

            process.StartInfo.Arguments = $"export HKCU \"{selectedPath}\\Backup\\HKCU.Reg\" /y";
            process.Start();
            process.WaitForExit();
        }

        public static void OptimizeRegistry()
        {
            using (var key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\PolicyManager\default\ApplicationManagement\AllowGameDVR"))
            {
                key.SetValue("value", 0, Microsoft.Win32.RegistryValueKind.DWord);
            }

            using (var key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"))
            {
                key.SetValue("GPU Priority", 8, Microsoft.Win32.RegistryValueKind.DWord);
                key.SetValue("Priority", 6, Microsoft.Win32.RegistryValueKind.DWord);
                key.SetValue("Scheduling Category", "High", Microsoft.Win32.RegistryValueKind.String);
                key.SetValue("Latency Sensitive", "True", Microsoft.Win32.RegistryValueKind.String);
                key.SetValue("SFIO Priority", "High", Microsoft.Win32.RegistryValueKind.String);
            }

            using (var key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"))
            {
                key.SetValue("NetworkThrottlingIndex", Convert.ToInt32("ffffffff", 16), Microsoft.Win32.RegistryValueKind.DWord);
                key.SetValue("SystemResponsiveness", 0, Microsoft.Win32.RegistryValueKind.DWord);
                key.SetValue("NoLazyMode", 1, Microsoft.Win32.RegistryValueKind.DWord);
                key.SetValue("AlwaysOn", 1, Microsoft.Win32.RegistryValueKind.DWord);
            }
        }

        public static void OptimizeBCDEdit()
        {
            Process process = new Process();
            process.StartInfo.FileName = @"C:\Windows\Sysnative\bcdedit";
            process.StartInfo.Verb = "runas";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;

            string[] commands = {
            "/set useplatformclock No",
            "/set useplatformtick No",
            "/set disabledynamictick Yes",
            "/set tscsyncpolicy Enhanced"
        };

            foreach (var command in commands)
            {
                process.StartInfo.Arguments = command;
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
                process.WaitForExit();
            }
        }

        public static void SetPowerPlan()
        {
            Process process = new Process();
            process.StartInfo.FileName = "powercfg";

            string[] commands = {
            "-duplicatescheme e9a42b02-d5df-448d-aa00-03f14749eb61 95533644-e700-4a79-a56c-a89e8cb109d9",
            "-changename 95533644-e700-4a79-a56c-a89e8cb109d9 SPEED",
            "-setactive 95533644-e700-4a79-a56c-a89e8cb109d9"
            };

            foreach (var command in commands)
            {
                process.StartInfo.Arguments = command;
                process.Start();
                process.WaitForExit();
            }
        }

        public static void DisableWindowsDefender()
        {
            using (var key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender"))
            {
                key.SetValue("DisableAntiSpyware", 1, Microsoft.Win32.RegistryValueKind.DWord);
            }
        }

        public static void DisableGenuineNotification()
        {
            using (var key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform"))
            {
                key.SetValue("NoGenuineNotification", 1, Microsoft.Win32.RegistryValueKind.DWord);
            }
        }

        public static void OptimizeMouse()
        {
            using (var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Control Panel\Mouse"))
            {
                key.SetValue("MouseThreshold1", 0, Microsoft.Win32.RegistryValueKind.DWord);
                key.SetValue("MouseThreshold2", 0, Microsoft.Win32.RegistryValueKind.DWord);
                key.SetValue("MouseSpeed", 0, Microsoft.Win32.RegistryValueKind.DWord);
            }
        }

        public static void DisableCortana()
        {
            using (var key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Windows Search"))
            {
                key.SetValue("AllowCortana", 0, Microsoft.Win32.RegistryValueKind.DWord);
                key.SetValue("BingSearchEnabled", 0, Microsoft.Win32.RegistryValueKind.DWord);
            }
            using (var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Explorer"))
            {
                key.SetValue("DisableSearchBoxSuggestions", 1, Microsoft.Win32.RegistryValueKind.DWord);
            }
        }

        public static void DisableMaintenance()
        {
            using (var key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"))
            {
                key.SetValue("MaintenanceDisabled", 1, Microsoft.Win32.RegistryValueKind.DWord);
            }
        }

        public static void CreateGodModeFolder()
        {
            string godmode = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), @"GodMode.{ED7BA470-8E54-465E-825C-99712043E01C}");
            Directory.CreateDirectory(godmode);
        }

        public static void DisableWindowsUpdate()
        {
            using (var key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update"))
            {
                key.SetValue("AutoUpdateCheckPeriodMinutes", 0, Microsoft.Win32.RegistryValueKind.DWord);
            }
        }
    }
}
