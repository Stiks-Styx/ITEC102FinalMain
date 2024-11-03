using System.Runtime.InteropServices;
namespace test1
{
    class ConsoleHelper
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        // Constants for keys
        public const int VK_W = 0x57;     // W key
        public const int VK_S = 0x53;     // S key
        public const int VK_A = 0x41;     // A key
        public const int VK_D = 0x44;     // D key
        public const int VK_UP = 0x26;    // Up Arrow
        public const int VK_DOWN = 0x28;  // Down Arrow
        public const int VK_RETURN = 0x0D;// Enter
        public const int VK_LEFT = 0x25;  // Left Arrow
        public const int VK_RIGHT = 0x27; // Right Arrow
        public const int VK_SPACE = 0x20; // Space

        // Method to check if a key is pressed
        public static bool IsKeyPressed(int vKey)
        {
            return (GetAsyncKeyState(vKey) & 0x8000) != 0;
        }
    }
}