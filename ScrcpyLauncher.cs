using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ScrcpyGUI
{
    public class ScrcpySettings
    {
        public string Serial { get; set; }
        public string IP { get; set; }
        public bool UseTcpIP { get; set; }
        public string MaxSize { get; set; }
        public string Bitrate { get; set; }
        public string Fps { get; set; }
        public bool Audio { get; set; }
        public bool NoControl { get; set; }
        public bool StayAwake { get; set; }
        public bool AlwaysOnTop { get; set; }
        public bool Borderless { get; set; }
        public bool Fullscreen { get; set; }
        public bool ShowTouches { get; set; }
        public bool TurnScreenOff { get; set; }
        public bool Record { get; set; }
        public string RecordPath { get; set; }
        public string Title { get; set; }
        public string VideoCodec { get; set; }
        public string Display { get; set; }

        public ScrcpySettings()
        {
            MaxSize = "";
            Bitrate = "";
            Fps = "";
            Audio = true;
            AlwaysOnTop = true;
            RecordPath = "record.mp4";
            VideoCodec = "";
            Display = "";
        }
    }

    public class ScrcpyInstance
    {
        public Process Process { get; set; }
        public ScrcpySettings Settings { get; set; }
        public bool IsRecording { get; set; }
        public DateTime StartTime { get; set; }
    }

    public static class ScrcpyLauncher
    {
        private static List<ScrcpyInstance> runningInstances = new List<ScrcpyInstance>();

        public static List<ScrcpyInstance> RunningInstances
        {
            get { return runningInstances; }
        }

        public static ScrcpyInstance Launch(ScrcpySettings settings)
        {
            string scrcpyPath = ScrcpyDownloader.GetScrcpyPath();
            if (scrcpyPath == null)
            {
                throw new FileNotFoundException("scrcpy.exe not found!");
            }

            var args = new List<string>();

            // Device
            if (settings.UseTcpIP && !string.IsNullOrEmpty(settings.IP))
            {
                args.Add("--tcpip=" + settings.IP + ":5555");
            }
            else if (!string.IsNullOrEmpty(settings.Serial))
            {
                args.Add("-s");
                args.Add(settings.Serial);
            }

            // Video
            if (!string.IsNullOrEmpty(settings.MaxSize) && settings.MaxSize != "Auto")
                args.Add("--max-size=" + settings.MaxSize);
            if (!string.IsNullOrEmpty(settings.Bitrate) && settings.Bitrate != "Auto")
                args.Add("--video-bit-rate=" + settings.Bitrate + "M");
            if (!string.IsNullOrEmpty(settings.Fps) && settings.Fps != "Auto")
                args.Add("--max-fps=" + settings.Fps);
            if (!string.IsNullOrEmpty(settings.VideoCodec))
                args.Add("--video-codec=" + settings.VideoCodec);
            if (!string.IsNullOrEmpty(settings.Display))
                args.Add("--display-id=" + settings.Display);

            // Audio
            if (settings.Audio)
                args.Add("--audio-codec=opus");
            else
                args.Add("--no-audio");

            // Control
            if (settings.NoControl) args.Add("--no-control");
            if (settings.StayAwake) args.Add("--stay-awake");
            if (settings.ShowTouches) args.Add("--show-touches");
            if (settings.TurnScreenOff) args.Add("--turn-screen-off");

            // Window
            if (settings.AlwaysOnTop) args.Add("--always-on-top");
            if (settings.Borderless) args.Add("--window-borderless");
            if (settings.Fullscreen) args.Add("--fullscreen");
            if (!string.IsNullOrEmpty(settings.Title))
                args.Add("--window-title=\"" + settings.Title + "\"");

            // Record
            if (settings.Record && !string.IsNullOrEmpty(settings.RecordPath))
                args.Add("--record=\"" + settings.RecordPath + "\"");

            var argString = string.Join(" ", args);

            var psi = new ProcessStartInfo
            {
                FileName = scrcpyPath,
                Arguments = argString,
                UseShellExecute = false,
                CreateNoWindow = false,
                WorkingDirectory = Path.GetDirectoryName(scrcpyPath)
            };

            var process = Process.Start(psi);
            var instance = new ScrcpyInstance
            {
                Process = process,
                Settings = settings,
                IsRecording = settings.Record,
                StartTime = DateTime.Now
            };

            runningInstances.Add(instance);
            
            // Cleanup when process exits
            process.EnableRaisingEvents = true;
            process.Exited += (s, e) => { runningInstances.Remove(instance); };

            return instance;
        }

        public static void StopInstance(ScrcpyInstance instance)
        {
            if (instance != null && instance.Process != null && !instance.Process.HasExited)
            {
                try
                {
                    instance.Process.CloseMainWindow();
                    if (!instance.Process.WaitForExit(2000))
                        instance.Process.Kill();
                }
                catch { }
            }
            runningInstances.Remove(instance);
        }

        public static void StopAll()
        {
            var instances = new List<ScrcpyInstance>(runningInstances);
            foreach (var instance in instances)
            {
                StopInstance(instance);
            }
        }

        public static int GetRunningCount()
        {
            // Cleanup dead processes
            runningInstances.RemoveAll(i => i.Process == null || i.Process.HasExited);
            return runningInstances.Count;
        }
    }
}
