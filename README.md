# BasicMirror

A lightweight, modern desktop GUI for [scrcpy](https://github.com/Genymobile/scrcpy) - the Android screen mirroring tool.

![Version](https://img.shields.io/badge/version-3.0-blue)
![Platform](https://img.shields.io/badge/platform-Windows-lightgrey)
![.NET](https://img.shields.io/badge/.NET_Framework-4.8-purple)
![License](https://img.shields.io/badge/license-MIT-green)

## ✨ Features

| Feature | Description |
|---------|-------------|
| 🌐 **Multi-language** | English + Tiếng Việt |
| 📱 **Multi-device** | Mirror multiple phones simultaneously |
| 🎬 **Recording** | Start/Stop video recording |
| ⚙️ **Settings** | Video codec, advanced options |
| 📁 **Auto-download** | Downloads scrcpy from [official repo](https://github.com/Genymobile/scrcpy/releases) |
| 🎨 **Modern UI** | Dark theme with gradient header |

## 📦 Installation

### Quick Start (Recommended)
1. Download **`BasicMirror.exe`** from [Releases](../../releases) (~50KB)
2. Run the EXE
3. Click "Yes" when prompted to download scrcpy (~15MB from [Genymobile/scrcpy](https://github.com/Genymobile/scrcpy/releases))
4. Done! Start mirroring your Android device

> **Note:** This app downloads scrcpy binaries directly from the official [Genymobile/scrcpy](https://github.com/Genymobile/scrcpy) repository. We don't host any scrcpy files.

### Build from Source
```batch
git clone https://github.com/anhhackta/BasicMirror.git
cd BasicMirror
build.bat
```

## 🚀 Usage

1. **USB Connection**
   - Connect phone via USB with debugging enabled
   - Click "Refresh" to detect device
   - Click "Mirror" to start

2. **WiFi Connection**
   - Connect phone via USB first
   - Select device → Click "Open WiFi Port"
   - Disconnect USB cable
   - Select the saved WiFi device → Click "Mirror"

3. **Recording**
   - Check "Enable Recording"
   - Set output filename
   - Mirror device → Recording starts automatically
   - Click "Stop" to save video

## Requirements

- Windows 7/8/10/11
- .NET Framework 4.8 (pre-installed on Windows 10+)
- Android device with USB debugging enabled

## Project Structure

```
ScrcpyGUI/
├── Program.cs          # Entry point
├── MainForm.cs         # Main UI
├── SettingsForm.cs     # Settings dialog
├── Language.cs         # EN/VI localization
├── AdbHelper.cs        # ADB commands
├── ScrcpyLauncher.cs   # Scrcpy process manager
├── ScrcpyDownloader.cs # Auto-download from GitHub
├── DeviceManager.cs    # Saved devices
├── logo.svg            # App icon
└── build.bat           # Build script
```

## License

MIT License - See [LICENSE](LICENSE)

## Credits

- [scrcpy](https://github.com/Genymobile/scrcpy) by Genymobile
- ScrcpyGUI by [Your Name]
