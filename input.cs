using System.Runtime.InteropServices;

namespace Game
{
    public partial class Game
    {
        [LibraryImport("user32.dll")]
        private static partial short GetAsyncKeyState(int vKey);

        const int VK_LEFT = 0x25; const int VK_RIGHT = 0x27; const int VK_A = 0x41; const int VK_D = 0x44;
        const int VK_UP = 0x26; const int VK_DOWN = 0x28; const int VK_W = 0x57; const int VK_S = 0x53;
        const int VK_ESCAPE = 0x1B;

        // An instance method that can freely read/write to the class instance fields
        private void StartInputListener()
        {
            _ = Task.Run(() =>
            {
                bool leftPressed, rightPressed, upPressed, downPressed;
                while (restart)
                {
                    //If the key is pressed GetAsyncKeyState(k) will return a negative number
                    leftPressed = GetAsyncKeyState(VK_LEFT) < 0 || GetAsyncKeyState(VK_A) < 0;
                    rightPressed = GetAsyncKeyState(VK_RIGHT) < 0 || GetAsyncKeyState(VK_D) < 0;
                    upPressed = GetAsyncKeyState(VK_UP) < 0 || GetAsyncKeyState(VK_W) < 0;
                    downPressed = GetAsyncKeyState(VK_DOWN) < 0 || GetAsyncKeyState(VK_S) < 0;

                    if (GetAsyncKeyState(VK_ESCAPE) < 0) Interlocked.Exchange(ref restart, false);

                    if (leftPressed && !rightPressed)
                         Interlocked.Exchange(ref steerDirection, -1);
                    else if (rightPressed && !leftPressed)
                         Interlocked.Exchange(ref steerDirection, 1);
                    else Interlocked.Exchange(ref steerDirection, 0);

                    if (upPressed && !downPressed)
                         Interlocked.Exchange(ref speedDirection, 1);
                    else if (downPressed && !upPressed)
                         Interlocked.Exchange(ref speedDirection, -1);
                    else 
                        Interlocked.Exchange(ref speedDirection, 0);

                    while (Console.KeyAvailable) Console.ReadKey(true);
                }
            });
        }
    }
}
