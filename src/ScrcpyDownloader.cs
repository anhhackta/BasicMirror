using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ScrcpyGUI
{
    public static class ScrcpyDownloader
    {
        private const string GITHUB_API = "https://api.github.com/repos/Genymobile/scrcpy/releases/latest";
        private const string USER_AGENT = "ScrcpyGUI/3.0";
        private const string SCRCPY_FOLDER = "scrcpy";

        public static string ScrcpyDir
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SCRCPY_FOLDER); }
        }

        public static string ScrcpyExe
        {
            get { return Path.Combine(ScrcpyDir, "scrcpy.exe"); }
        }

        public static string AdbExe
        {
            get { return Path.Combine(ScrcpyDir, "adb.exe"); }
        }

        public static bool IsScrcpyInstalled()
        {
            // Check both in subfolder and current directory
            if (File.Exists(ScrcpyExe) && File.Exists(AdbExe))
                return true;
            
            // Fallback: check current directory
            string currentScrcpy = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scrcpy.exe");
            string currentAdb = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "adb.exe");
            return File.Exists(currentScrcpy) && File.Exists(currentAdb);
        }

        public static string GetScrcpyPath()
        {
            if (File.Exists(ScrcpyExe)) return ScrcpyExe;
            string current = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scrcpy.exe");
            if (File.Exists(current)) return current;
            return null;
        }

        public static string GetAdbPath()
        {
            if (File.Exists(AdbExe)) return AdbExe;
            string current = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "adb.exe");
            if (File.Exists(current)) return current;
            return null;
        }

        public static string GetScrcpyVersion()
        {
            string path = GetScrcpyPath();
            if (path == null) return null;
            try
            {
                var info = System.Diagnostics.FileVersionInfo.GetVersionInfo(path);
                return info.FileVersion;
            }
            catch { }
            return "Unknown";
        }

        public static string GetLatestDownloadUrl()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GITHUB_API);
                request.UserAgent = USER_AGENT;
                request.Accept = "application/vnd.github.v3+json";
                request.Timeout = 15000;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string json = reader.ReadToEnd();
                    Match match = Regex.Match(json, @"""browser_download_url"":\s*""([^""]+win64[^""]+\.zip)""");
                    if (match.Success)
                    {
                        return match.Groups[1].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lang.Get("msg_error") + ":\n" + ex.Message, Lang.Get("msg_error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        public static bool DownloadAndExtract(string url, Action<int> progressCallback, Action<string> statusCallback)
        {
            string tempZip = Path.Combine(Path.GetTempPath(), "scrcpy_temp.zip");
            string extractPath = ScrcpyDir;

            try
            {
                // Create scrcpy folder
                if (!Directory.Exists(extractPath))
                    Directory.CreateDirectory(extractPath);

                // Download
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("User-Agent", USER_AGENT);
                    client.DownloadProgressChanged += (s, e) =>
                    {
                        if (progressCallback != null)
                            progressCallback(e.ProgressPercentage);
                    };

                    var task = client.DownloadFileTaskAsync(new Uri(url), tempZip);
                    while (!task.IsCompleted)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(50);
                    }

                    if (task.IsFaulted)
                    {
                        throw task.Exception;
                    }
                }

                // Extract
                if (statusCallback != null) statusCallback(Lang.Get("status_extracting"));
                if (progressCallback != null) progressCallback(-1);

                using (ZipArchive archive = ZipFile.OpenRead(tempZip))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (string.IsNullOrEmpty(entry.Name)) continue;

                        // Remove the top-level folder from path
                        string relativePath = entry.FullName;
                        int slashIndex = relativePath.IndexOf('/');
                        if (slashIndex >= 0)
                            relativePath = relativePath.Substring(slashIndex + 1);

                        if (string.IsNullOrEmpty(relativePath)) continue;

                        string destPath = Path.Combine(extractPath, relativePath);
                        string destDir = Path.GetDirectoryName(destPath);

                        if (!Directory.Exists(destDir))
                            Directory.CreateDirectory(destDir);

                        entry.ExtractToFile(destPath, true);
                    }
                }

                // Cleanup
                File.Delete(tempZip);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lang.Get("msg_error") + ":\n" + ex.Message, Lang.Get("msg_error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (File.Exists(tempZip))
                    File.Delete(tempZip);
                return false;
            }
        }
    }
}
