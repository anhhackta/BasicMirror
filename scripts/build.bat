@echo off
echo ========================================
echo   Building BasicMirror v1.0
echo ========================================
echo.

cd /d "%~dp0..\src"

set CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe
set REFS=/r:System.dll /r:System.Windows.Forms.dll /r:System.Drawing.dll /r:System.Core.dll /r:System.IO.Compression.dll /r:System.IO.Compression.FileSystem.dll
set SRCS=Program.cs MainForm.cs AdbHelper.cs ScrcpyLauncher.cs DeviceManager.cs ScrcpyDownloader.cs Language.cs SettingsForm.cs
set OUT=/out:..\BasicMirror.exe

if exist "..\assets\icon.ico" (
    set ICON=/win32icon:..\assets\icon.ico
) else (
    set ICON=
)

%CSC% /target:winexe %OUT% %REFS% %ICON% %SRCS%

if %errorlevel%==0 (
    echo.
    echo ========================================
    echo   BUILD SUCCESSFUL!
    echo ========================================
    echo   Output: BasicMirror.exe
    echo.
) else (
    echo.
    echo ========================================
    echo   BUILD FAILED!
    echo ========================================
)
pause
