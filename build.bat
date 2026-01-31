@echo off
echo ========================================
echo   Compiling ScrcpyGUI v3.0
echo ========================================
echo.

set CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe
set REFS=/r:System.dll /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Core.dll /r:System.IO.Compression.dll /r:System.IO.Compression.FileSystem.dll
set SRCS=Program.cs MainForm.cs AdbHelper.cs ScrcpyLauncher.cs DeviceManager.cs ScrcpyDownloader.cs Language.cs SettingsForm.cs
set OUT=/out:ScrcpyGUI.exe

%CSC% /target:winexe %OUT% %REFS% %SRCS%

if %errorlevel%==0 (
    echo.
    echo ========================================
    echo   BUILD SUCCESSFUL!
    echo ========================================
    echo   Output: ScrcpyGUI.exe
    echo.
    copy ScrcpyGUI.exe .. /Y >nul 2>&1
    echo   Copied to parent folder.
    echo.
) else (
    echo.
    echo ========================================
    echo   BUILD FAILED!
    echo ========================================
)
pause
