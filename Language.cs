using System;
using System.Collections.Generic;

namespace ScrcpyGUI
{
    public static class Lang
    {
        private static string currentLang = "en";
        
        private static Dictionary<string, Dictionary<string, string>> strings = new Dictionary<string, Dictionary<string, string>>
        {
            { "en", new Dictionary<string, string>
                {
                    // App
                    { "app_title", "ScrcpyGUI" },
                    { "app_version", "v3.0" },
                    
                    // Sections
                    { "section_devices", "DEVICES" },
                    { "section_video", "VIDEO" },
                    { "section_audio_control", "AUDIO & CONTROL" },
                    { "section_window", "WINDOW" },
                    { "section_recording", "RECORDING" },
                    { "section_settings", "SETTINGS" },
                    
                    // Buttons
                    { "btn_refresh", "Refresh" },
                    { "btn_mirror", "Mirror" },
                    { "btn_mirror_all", "Mirror All" },
                    { "btn_open_wifi", "Open WiFi Port" },
                    { "btn_add_ip", "Add IP" },
                    { "btn_connect", "Connect" },
                    { "btn_disconnect", "Disconnect" },
                    { "btn_settings", "Settings" },
                    { "btn_save", "Save" },
                    { "btn_delete", "Delete" },
                    { "btn_browse", "Browse" },
                    { "btn_ok", "OK" },
                    { "btn_cancel", "Cancel" },
                    
                    // Recording
                    { "btn_rec_start", "Start" },
                    { "btn_rec_pause", "Pause" },
                    { "btn_rec_stop", "Stop" },
                    { "rec_output", "Output:" },
                    
                    // Labels
                    { "lbl_size", "Size:" },
                    { "lbl_bitrate", "Bitrate:" },
                    { "lbl_fps", "FPS:" },
                    { "lbl_ip", "IP Address:" },
                    { "lbl_device_name", "Device Name:" },
                    { "lbl_language", "Language:" },
                    
                    // Checkboxes
                    { "chk_audio", "Audio (Android 11+)" },
                    { "chk_no_control", "View Only (No Control)" },
                    { "chk_stay_awake", "Keep Screen On" },
                    { "chk_always_top", "Always on Top" },
                    { "chk_borderless", "Borderless" },
                    { "chk_fullscreen", "Fullscreen" },
                    { "chk_show_touches", "Show Touches" },
                    { "chk_turn_screen_off", "Turn Screen Off" },
                    
                    // Status
                    { "status_ready", "Ready" },
                    { "status_scanning", "Scanning devices..." },
                    { "status_found_devices", "Found {0} device(s)" },
                    { "status_no_devices", "No USB devices found" },
                    { "status_connecting", "Connecting..." },
                    { "status_connected", "Connected to {0}" },
                    { "status_mirroring", "Mirroring: {0}" },
                    { "status_recording", "Recording: {0}" },
                    { "status_downloading", "Downloading: {0}%" },
                    { "status_extracting", "Extracting..." },
                    { "status_download_complete", "Download complete!" },
                    { "status_opening_port", "Opening WiFi port..." },
                    { "status_port_opened", "Port opened. IP: {0}" },
                    { "status_saved", "Saved: {0}" },
                    { "status_deleted", "Deleted" },
                    
                    // Messages
                    { "msg_select_device", "Please select a device!" },
                    { "msg_enter_ip", "Please enter an IP address!" },
                    { "msg_scrcpy_not_found", "Scrcpy not installed." },
                    { "msg_download_prompt", "Scrcpy is not installed.\n\nDo you want to download it automatically from GitHub?\n(About 15MB)" },
                    { "msg_download_title", "Download Scrcpy" },
                    { "msg_save_device", "Enter device name:" },
                    { "msg_save_title", "Save Device" },
                    { "msg_error", "Error" },
                    { "msg_info", "Information" },
                    { "msg_confirm", "Confirm" },
                    { "msg_cannot_get_ip", "Cannot get IP. Check WiFi connection." },
                    
                    // Device types
                    { "device_usb", "USB" },
                    { "device_wifi", "WiFi" },
                    
                    // Settings
                    { "settings_title", "Settings" },
                    { "settings_general", "General" },
                    { "settings_video", "Video" },
                    { "settings_advanced", "Advanced" },
                    { "settings_codec", "Video Codec:" },
                    { "settings_display", "Display:" },
                    { "settings_rotation", "Rotation:" },
                    { "settings_crop", "Crop:" },
                    { "settings_shortcuts", "Keyboard Shortcuts" }
                }
            },
            { "vi", new Dictionary<string, string>
                {
                    // App
                    { "app_title", "ScrcpyGUI" },
                    { "app_version", "v3.0" },
                    
                    // Sections
                    { "section_devices", "THIET BI" },
                    { "section_video", "VIDEO" },
                    { "section_audio_control", "AM THANH & DIEU KHIEN" },
                    { "section_window", "CUA SO" },
                    { "section_recording", "QUAY VIDEO" },
                    { "section_settings", "CAI DAT" },
                    
                    // Buttons
                    { "btn_refresh", "Lam Moi" },
                    { "btn_mirror", "Mirror" },
                    { "btn_mirror_all", "Mirror Tat Ca" },
                    { "btn_open_wifi", "Mo Port WiFi" },
                    { "btn_add_ip", "Them IP" },
                    { "btn_connect", "Ket Noi" },
                    { "btn_disconnect", "Ngat Ket Noi" },
                    { "btn_settings", "Cai Dat" },
                    { "btn_save", "Luu" },
                    { "btn_delete", "Xoa" },
                    { "btn_browse", "Chon" },
                    { "btn_ok", "OK" },
                    { "btn_cancel", "Huy" },
                    
                    // Recording
                    { "btn_rec_start", "Bat Dau" },
                    { "btn_rec_pause", "Tam Dung" },
                    { "btn_rec_stop", "Dung" },
                    { "rec_output", "Luu tai:" },
                    
                    // Labels
                    { "lbl_size", "Kich thuoc:" },
                    { "lbl_bitrate", "Bitrate:" },
                    { "lbl_fps", "FPS:" },
                    { "lbl_ip", "Dia chi IP:" },
                    { "lbl_device_name", "Ten thiet bi:" },
                    { "lbl_language", "Ngon ngu:" },
                    
                    // Checkboxes
                    { "chk_audio", "Am thanh (Android 11+)" },
                    { "chk_no_control", "Chi xem (Khong dieu khien)" },
                    { "chk_stay_awake", "Giu man hinh sang" },
                    { "chk_always_top", "Luon tren cung" },
                    { "chk_borderless", "Khong vien" },
                    { "chk_fullscreen", "Toan man hinh" },
                    { "chk_show_touches", "Hien thi cham" },
                    { "chk_turn_screen_off", "Tat man hinh DT" },
                    
                    // Status
                    { "status_ready", "San sang" },
                    { "status_scanning", "Dang quet thiet bi..." },
                    { "status_found_devices", "Tim thay {0} thiet bi" },
                    { "status_no_devices", "Khong tim thay thiet bi USB" },
                    { "status_connecting", "Dang ket noi..." },
                    { "status_connected", "Da ket noi: {0}" },
                    { "status_mirroring", "Dang mirror: {0}" },
                    { "status_recording", "Dang quay: {0}" },
                    { "status_downloading", "Dang tai: {0}%" },
                    { "status_extracting", "Dang giai nen..." },
                    { "status_download_complete", "Tai thanh cong!" },
                    { "status_opening_port", "Dang mo port WiFi..." },
                    { "status_port_opened", "Da mo port. IP: {0}" },
                    { "status_saved", "Da luu: {0}" },
                    { "status_deleted", "Da xoa" },
                    
                    // Messages
                    { "msg_select_device", "Vui long chon thiet bi!" },
                    { "msg_enter_ip", "Vui long nhap dia chi IP!" },
                    { "msg_scrcpy_not_found", "Chua cai dat scrcpy." },
                    { "msg_download_prompt", "Scrcpy chua duoc cai dat.\n\nBan co muon tai tu dong tu GitHub khong?\n(Khoang 15MB)" },
                    { "msg_download_title", "Tai Scrcpy" },
                    { "msg_save_device", "Nhap ten thiet bi:" },
                    { "msg_save_title", "Luu Thiet Bi" },
                    { "msg_error", "Loi" },
                    { "msg_info", "Thong Bao" },
                    { "msg_confirm", "Xac Nhan" },
                    { "msg_cannot_get_ip", "Khong lay duoc IP. Kiem tra ket noi WiFi." },
                    
                    // Device types
                    { "device_usb", "USB" },
                    { "device_wifi", "WiFi" },
                    
                    // Settings
                    { "settings_title", "Cai Dat" },
                    { "settings_general", "Chung" },
                    { "settings_video", "Video" },
                    { "settings_advanced", "Nang Cao" },
                    { "settings_codec", "Codec Video:" },
                    { "settings_display", "Man hinh:" },
                    { "settings_rotation", "Xoay:" },
                    { "settings_crop", "Cat:" },
                    { "settings_shortcuts", "Phim Tat" }
                }
            }
        };

        public static string Current
        {
            get { return currentLang; }
            set { if (strings.ContainsKey(value)) currentLang = value; }
        }

        public static string Get(string key)
        {
            if (strings.ContainsKey(currentLang) && strings[currentLang].ContainsKey(key))
                return strings[currentLang][key];
            if (strings["en"].ContainsKey(key))
                return strings["en"][key];
            return key;
        }

        public static string Get(string key, params object[] args)
        {
            string template = Get(key);
            try { return string.Format(template, args); }
            catch { return template; }
        }

        public static string[] GetAvailableLanguages()
        {
            var langs = new List<string>();
            foreach (var key in strings.Keys) langs.Add(key);
            return langs.ToArray();
        }

        public static string GetLanguageName(string code)
        {
            if (code == "en") return "English";
            if (code == "vi") return "Tieng Viet";
            return code;
        }
    }
}
