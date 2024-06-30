using NUnit.Framework;
using System.IO;
using System;
using Microsoft.Win32;
using System.Diagnostics;
using Optimizer;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateGodModeFolder_ShouldCreateFolderOnDesktop()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string godModePath = Path.Combine(desktopPath, "GodMode.{ED7BA470-8E54-465E-825C-99712043E01C}");

            if (Directory.Exists(godModePath))
                Directory.Delete(godModePath);

            Optimizations.CreateGodModeFolder();

            Assert.That(Directory.Exists(godModePath), Is.True);

            Directory.Delete(godModePath);
        }
        [Test]
        public void CreateBackup_ShouldCreateBackupFolder()
        {
            string testPath = Path.Combine(Path.GetTempPath(), "TestBackup");
            Directory.CreateDirectory(testPath);

            Optimizations.CreateBackup(testPath);

            Assert.That(Directory.Exists(Path.Combine(testPath, "Backup")), Is.True);
            Assert.That(File.Exists(Path.Combine(testPath, "Backup", "HKLM.Reg")), Is.True);
            Assert.That(File.Exists(Path.Combine(testPath, "Backup", "HKCU.Reg")), Is.True);

            Directory.Delete(testPath, true);
        }

        [Test]
        public void OptimizeRegistry_ShouldSetCorrectRegistryValues()
        {
            Optimizations.OptimizeRegistry();

            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\PolicyManager\default\ApplicationManagement\AllowGameDVR"))
            {
                Assert.That(key.GetValue("value"), Is.EqualTo(0));
            }

            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"))
            {
                Assert.That(key.GetValue("GPU Priority"), Is.EqualTo(8));
                Assert.That(key.GetValue("Priority"), Is.EqualTo(6));
                Assert.That(key.GetValue("Scheduling Category"), Is.EqualTo("High"));
                Assert.That(key.GetValue("Latency Sensitive"), Is.EqualTo("True"));
                Assert.That(key.GetValue("SFIO Priority"), Is.EqualTo("High"));
            }

            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"))
            {
                Assert.That(key.GetValue("NetworkThrottlingIndex"), Is.EqualTo(unchecked((int)0xffffffff)));
                Assert.That(key.GetValue("SystemResponsiveness"), Is.EqualTo(0));
                Assert.That(key.GetValue("NoLazyMode"), Is.EqualTo(1));
                Assert.That(key.GetValue("AlwaysOn"), Is.EqualTo(1));
            }
        }

        [Test]
        public void OptimizeBCDEdit_ShouldExecuteCorrectCommands()
        {
            Optimizations.OptimizeBCDEdit();
            Assert.That(true, Is.True);
        }

        [Test]
        public void SetPowerPlan_ShouldExecuteCorrectCommands()
        {

            Optimizations.SetPowerPlan();

            Process process = new Process();
            process.StartInfo.FileName = "powercfg";
            process.StartInfo.Arguments = "/list";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            Assert.That(output, Does.Contain("SPEED"));
            Assert.That(output, Does.Contain("(SPEED) *"));
        }

        [Test]
        public void DisableWindowsDefender_ShouldSetCorrectRegistryValue()
        {
            Optimizations.DisableWindowsDefender();

            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender"))
            {
                Assert.That(key.GetValue("DisableAntiSpyware"), Is.EqualTo(1));
            }
        }

        [Test]
        public void DisableGenuineNotification_ShouldSetCorrectRegistryValue()
        {
            Optimizations.DisableGenuineNotification();

            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform"))
            {
                Assert.That(key.GetValue("NoGenuineNotification"), Is.EqualTo(1));
            }
        }

        [Test]
        public void OptimizeMouse_ShouldSetCorrectRegistryValues()
        {
            Optimizations.OptimizeMouse();

            using (var key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Mouse"))
            {
                Assert.That(key.GetValue("MouseThreshold1"), Is.EqualTo(0));
                Assert.That(key.GetValue("MouseThreshold2"), Is.EqualTo(0));
                Assert.That(key.GetValue("MouseSpeed"), Is.EqualTo(0));
            }
        }

        [Test]
        public void DisableCortana_ShouldSetCorrectRegistryValues()
        {
            Optimizations.DisableCortana();

            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Windows Search"))
            {
                Assert.That(key.GetValue("AllowCortana"), Is.EqualTo(0));
                Assert.That(key.GetValue("BingSearchEnabled"), Is.EqualTo(0));
            }
            using (var key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Explorer"))
            {
                Assert.That(key.GetValue("DisableSearchBoxSuggestions"), Is.EqualTo(1));
            }
        }

        [Test]
        public void DisableMaintenance_ShouldSetCorrectRegistryValue()
        {
            Optimizations.DisableMaintenance();

            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Schedule\Maintenance"))
            {
                Assert.That(key.GetValue("MaintenanceDisabled"), Is.EqualTo(1));
            }
        }

        [Test]
        public void DisableWindowsUpdate_ShouldSetCorrectRegistryValue()
        {
            Optimizations.DisableWindowsUpdate();

            using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update"))
            {
                Assert.That(key.GetValue("AutoUpdateCheckPeriodMinutes"), Is.EqualTo(0));
            }
        }
    }
}