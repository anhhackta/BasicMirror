using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ScrcpyGUI
{
    public static class AdbHelper
    {
        public static string AdbPath
        {
            get { return ScrcpyDownloader.GetAdbPath(); }
        }

        public static List<string> GetUsbDevices()
        {
            var devices = new List<string>();
            try
            {
                var output = RunAdb("devices");
                var lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines.Skip(1))
                {
                    var match = Regex.Match(line, @"^([^\s:]+)\s+device$");
                    if (match.Success) devices.Add(match.Groups[1].Value);
                }
            }
            catch { }
            return devices;
        }

        public static string GetDeviceIP(string serial)
        {
            try
            {
                var output = RunAdb(string.Format("-s {0} shell ip route", serial));
                var match = Regex.Match(output, @"src\s+(\d+\.\d+\.\d+\.\d+)");
                if (match.Success) return match.Groups[1].Value;
            }
            catch { }
            return null;
        }

        public static string GetDeviceName(string serial)
        {
            try
            {
                var output = RunAdb(string.Format("-s {0} shell getprop ro.product.model", serial));
                return output.Trim();
            }
            catch { }
            return serial;
        }

        public static string GetAndroidVersion(string serial)
        {
            try
            {
                var output = RunAdb(string.Format("-s {0} shell getprop ro.build.version.release", serial));
                return "Android " + output.Trim();
            }
            catch { }
            return "";
        }

        public static void EnableTcpIP(string serial)
        {
            RunAdb(string.Format("-s {0} tcpip 5555", serial));
            System.Threading.Thread.Sleep(2000);
        }

        public static bool ConnectWiFi(string ip)
        {
            var output = RunAdb(string.Format("connect {0}:5555", ip));
            return output.Contains("connected");
        }

        public static void DisconnectWiFi(string ip)
        {
            RunAdb(string.Format("disconnect {0}:5555", ip));
        }

        public static void StartServer()
        {
            RunAdb("start-server", false);
        }

        public static List<string> GetDisplays(string serial)
        {
            var displays = new List<string>();
            try
            {
                var output = RunAdb(string.Format("-s {0} shell dumpsys display | grep \"Display Id\"", serial));
                var matches = Regex.Matches(output, @"Display Id=(\d+)");
                foreach (Match m in matches)
                {
                    displays.Add(m.Groups[1].Value);
                }
            }
            catch { }
            if (displays.Count == 0) displays.Add("0");
            return displays;
        }

        private static string RunAdb(string args, bool wait = true)
        {
            if (string.IsNullOrEmpty(AdbPath)) return "";
            var psi = new ProcessStartInfo
            {
                FileName = AdbPath,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(AdbPath)
            };
            using (var proc = Process.Start(psi))
            {
                if (wait)
                {
                    proc.WaitForExit(10000);
                    return proc.StandardOutput.ReadToEnd();
                }
            }
            return "";
        }
    }
}
