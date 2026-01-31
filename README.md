<div align="center">
  <img src="assets/logo.svg" alt="BasicMirror Logo" width="120"/>
  <h1>BasicMirror</h1>
  <p><strong>Simple & Lightweight Android Screen Mirror for Windows</strong></p>
  
  <p>
    <img src="https://img.shields.io/badge/version-1.0-blue?style=for-the-badge" alt="Version"/>
    <img src="https://img.shields.io/badge/platform-Windows-lightgrey?style=for-the-badge" alt="Platform"/>
    <img src="https://img.shields.io/badge/.NET-4.8-purple?style=for-the-badge" alt=".NET"/>
    <img src="https://img.shields.io/badge/license-MIT-green?style=for-the-badge" alt="License"/>
  </p>
  
  <p>
    <a href="#features">Features</a> •
    <a href="#installation">Installation</a> •
    <a href="#usage">Usage</a> •
    <a href="#building">Building</a> •
    <a href="#credits">Credits</a>
  </p>
</div>

---

## 🎯 What is BasicMirror?

**BasicMirror** is a lightweight GUI wrapper for [scrcpy](https://github.com/Genymobile/scrcpy), the popular open-source Android screen mirroring tool. It provides an intuitive interface to mirror your Android device to your PC without any configuration hassle.

### Why BasicMirror?

| Feature | BasicMirror | Command Line |
|---------|:-----------:|:------------:|
| One-click mirroring | ✅ | ❌ |
| Auto-download scrcpy | ✅ | ❌ |
| Multi-device support | ✅ | Manual |
| Save device profiles | ✅ | ❌ |
| Video recording | ✅ | Manual |
| Multi-language | ✅ | ❌ |
| Size | ~50KB | - |

---

## ✨ Features

<table>
<tr>
<td width="50%">

### 🌐 Multi-Language
- English (default)
- Tiếng Việt
- Easy to add more languages

</td>
<td width="50%">

### 📱 Multi-Device
- Mirror multiple phones simultaneously
- USB and WiFi connections
- Save device profiles

</td>
</tr>
<tr>
<td>

### 🎬 Screen Recording
- Start/Stop recording controls
- MP4 and MKV output
- Custom output path

</td>
<td>

### ⚙️ Advanced Settings
- Video quality (size, bitrate, FPS)
- Audio mirroring (Android 11+)
- Always on top, borderless, fullscreen

</td>
</tr>
</table>

### 📁 Auto-Download

BasicMirror automatically downloads scrcpy from the [official repository](https://github.com/Genymobile/scrcpy/releases) on first launch. No manual setup required!

---

## 📦 Installation

### Quick Start (Recommended)

1. **Download** `BasicMirror.exe` from [Releases](../../releases) (~50KB)
2. **Run** the executable
3. **Download scrcpy** when prompted (~15MB, from [Genymobile/scrcpy](https://github.com/Genymobile/scrcpy/releases))
4. **Connect** your Android device and start mirroring!

> **Note:** We don't host scrcpy binaries. Downloads come directly from the official scrcpy repository.

### System Requirements

- Windows 7/8/10/11
- .NET Framework 4.8 (pre-installed on Windows 10+)
- Android device with USB debugging enabled

---

## 🚀 Usage

### USB Connection

```
1. Enable USB debugging on your Android device
2. Connect phone to PC via USB cable
3. Click "Refresh" to detect device
4. Click "Mirror" to start
```

### WiFi Connection

```
1. Connect phone via USB first
2. Select device → Click "Open WiFi Port"
3. Note the IP address shown
4. Disconnect USB cable
5. Select the saved WiFi device → Click "Mirror"
```

### Recording

```
1. Check "Enable Recording"
2. Set output filename (default: record.mp4)
3. Mirror device → Recording starts automatically
4. Click "Stop" to save video
```

---

## 🛠️ Building

### Prerequisites

- Windows with .NET Framework 4.8
- No additional tools required (uses built-in C# compiler)

### Build from Source

```batch
git clone https://github.com/anhhackta/BasicMirror.git
cd BasicMirror/scripts
build.bat
```

### Project Structure

```
BasicMirror/
├── src/                    # Source code
│   ├── Program.cs          # Entry point
│   ├── MainForm.cs         # Main UI
│   ├── SettingsForm.cs     # Settings dialog
│   ├── Language.cs         # EN/VI localization
│   ├── AdbHelper.cs        # ADB commands
│   ├── ScrcpyLauncher.cs   # Scrcpy process manager
│   ├── ScrcpyDownloader.cs # Auto-download from GitHub
│   └── DeviceManager.cs    # Saved devices
├── scripts/                # Build & utility scripts
│   └── build.bat           # Build script
├── assets/                 # Images & icons
│   └── logo.svg            # App logo
├── docs/                   # Documentation & GitHub Pages
│   └── index.html          # Website
├── README.md
└── LICENSE
```

---

## 🙏 Credits

- **[scrcpy](https://github.com/Genymobile/scrcpy)** by Genymobile - The amazing screen mirroring tool
- **BasicMirror** by [anhhackta](https://github.com/anhhackta) - This GUI wrapper

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

<div align="center">
  <p>Made with ❤️ for the Android community</p>
  <p>
    <a href="https://github.com/anhhackta/BasicMirror/issues">Report Bug</a> •
    <a href="https://github.com/anhhackta/BasicMirror/issues">Request Feature</a>
  </p>
</div>
