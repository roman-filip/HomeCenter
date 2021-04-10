using System;
using System.Runtime.InteropServices;

namespace HomeCenter.Services
{
    public static class KeyboardUtils
    {
        public static bool IsNumLockActive()
            => (((ushort)GetKeyState(0x90)) & 0xffff) != 0;

        public static bool IsCapsLockActive()
            => (((ushort)GetKeyState(0x14)) & 0xffff) != 0;

        public static bool IsScrollLockActive()
            => (((ushort)GetKeyState(0x91)) & 0xffff) != 0;

        public static void PressVolumeUp()
            => keybd_event((int)ConsoleKey.VolumeUp, 0, 0, 0);

        public static void PressVolumeDown()
            => keybd_event((int)ConsoleKey.VolumeDown, 0, 0, 0);

        public static void PressF5()
            => keybd_event((int)ConsoleKey.F5, 0, 0, 0);

        public static void PressPlayPause()
        {
            byte mediaPlayScanCode = 0x22;
            uint KEYEVENTF_EXTENDEDKEY = 1;
            uint KEYEVENTF_KEYUP = 2;

            keybd_event((int)ConsoleKey.MediaPlay, mediaPlayScanCode, KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event((int)ConsoleKey.MediaPlay, mediaPlayScanCode, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        private static extern short GetKeyState(int keyCode);
    }
}
