using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace test1
{
    public class ConsoleHelper
    {
        public const int VK_F11 = 0x7A;
        public const int SW_MAXIMIZE = 3;

        public const uint WM_KEYDOWN = 0x100;
        public const uint WM_MOUSEWHEEL = 0x20A;

        public const uint WHEEL_DELTA = 120;
        public const uint MK_CONTROL = 0x00008 << 16;

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    }
}
