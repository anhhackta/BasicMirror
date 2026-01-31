using System;
using System.Collections.Generic;
using System.IO;

namespace ScrcpyGUI
{
    public class SavedDevice
    {
        public string Name { get; set; }
        public string IP { get; set; }
    }

    public static class DeviceManager
    {
        private static string FilePath
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "devices.json"); }
        }

        public static List<SavedDevice> Load()
        {
            var devices = new List<SavedDevice>();
            try
            {
                if (File.Exists(FilePath))
                {
                    var lines = File.ReadAllLines(FilePath);
                    foreach (var line in lines)
                    {
                        var parts = line.Split('|');
                        if (parts.Length == 2)
                        {
                            devices.Add(new SavedDevice { Name = parts[0], IP = parts[1] });
                        }
                    }
                }
            }
            catch { }
            return devices;
        }

        public static void Save(List<SavedDevice> devices)
        {
            try
            {
                var lines = new List<string>();
                foreach (var d in devices)
                {
                    lines.Add(d.Name + "|" + d.IP);
                }
                File.WriteAllLines(FilePath, lines);
            }
            catch { }
        }

        public static void AddDevice(string name, string ip, List<SavedDevice> devices)
        {
            devices.Add(new SavedDevice { Name = name, IP = ip });
            Save(devices);
        }

        public static void RemoveDevice(int index, List<SavedDevice> devices)
        {
            if (index >= 0 && index < devices.Count)
            {
                devices.RemoveAt(index);
                Save(devices);
            }
        }
    }
}
