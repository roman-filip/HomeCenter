using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HomeCenter.Services
{
    public class KeyboardListenerService : IDisposable
    {
        private IntPtr _hookID = IntPtr.Zero;
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        private LowLevelKeyboardProc _proc;
        private bool _disposed;
        private ConsoleKey _lastKeyPressed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposed = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~KeyboardService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            _proc = HookCallback;
            SetKeyboardHook();
        }

        public void Stop()
            => UnsetKeyboardHook();

        private void SetKeyboardHook()
        {
            int WH_KEYBOARD_LL = 13;

            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                _hookID = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private void UnsetKeyboardHook()
            => UnhookWindowsHookEx(_hookID);

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            int WM_KEYDOWN = 0x0100;

            //Debug.WriteLine($"{nCode}, {wParam}, {lParam}");

            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                //Debug.WriteLine($"{vkCode} == {(ConsoleKey)vkCode}");

                if (!KeyboardUtils.IsNumLockActive())
                {
                    switch ((ConsoleKey)vkCode)
                    {
                        case ConsoleKey.Add:
                            KeyboardUtils.PressVolumeUp();
                            break;
                        case ConsoleKey.Subtract:
                            KeyboardUtils.PressVolumeDown();
                            break;
                        case ConsoleKey.Multiply:
                            KeyboardUtils.PressPlayPause();
                            break;
                        //case ConsoleKey.Clear:
                        //    KeyboardUtils.PressF5();
                        //    break;
                        case ConsoleKey.Divide:
                            if (_lastKeyPressed == ConsoleKey.Divide)
                            {
                                PowerUtils.Hibernate();
                            }
                            break;
                    }

                    _lastKeyPressed = (ConsoleKey)vkCode;
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
    }
}
