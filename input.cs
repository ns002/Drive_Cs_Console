using System.Runtime.InteropServices;
public partial class Input     //Input class is partial because of partial GetAsyncKeyState() member
{
    [LibraryImport("user32.dll")]
    private static partial short GetAsyncKeyState(int vKey);    //External source from user32.dll

    private enum Key : int {
        ESCAPE = 0x1B,
        LEFT = 0x25, UP = 0x26, RIGHT = 0x27, DOWN = 0x28,
        A = 0x41, D = 0x44, W = 0x57, S = 0x53
    }

    private enum Movement : short { 
        FORWARD  =  1, RIGHT = FORWARD,
        BACKWARD = -1, LEFT  = BACKWARD,
        NONE = 0
    }

    private static bool listening = false;
    // An instance method that can freely read/write to the class instance fields
    public static void StartInputListener()
    {
        if (!listening) {
            listening = true; _ = Task.Run(() => {
                bool leftPressed, rightPressed, upPressed, downPressed;
                while (Game.running) {
                    //If the key is pressed GetAsyncKeyState(k) will return a negative number
                    leftPressed = GetAsyncKeyState((int)Key.LEFT) < 0 || GetAsyncKeyState((int)Key.A) < 0;
                    rightPressed = GetAsyncKeyState((int)Key.RIGHT) < 0 || GetAsyncKeyState((int)Key.D) < 0;
                    upPressed = GetAsyncKeyState((int)Key.UP) < 0 || GetAsyncKeyState((int)Key.W) < 0;
                    downPressed = GetAsyncKeyState((int)Key.DOWN) < 0 || GetAsyncKeyState((int)Key.S) < 0;

                    if (GetAsyncKeyState((int)Key.ESCAPE) < 0)
                    {
                        Interlocked.Exchange(ref Game.running, false);
                        listening = false; Console.WriteLine("Input Listener Destroyed");
                    }
                    if (leftPressed && !rightPressed)
                        Interlocked.Exchange(ref Game.steerDirection, (short)Movement.LEFT);
                    else if (rightPressed && !leftPressed)
                        Interlocked.Exchange(ref Game.steerDirection, (short)Movement.RIGHT);
                    else Interlocked.Exchange(ref Game.steerDirection, (short)Movement.NONE);

                    if (upPressed && !downPressed)
                        Interlocked.Exchange(ref Game.speedDirection, (short)Movement.FORWARD);
                    else if (downPressed && !upPressed)
                        Interlocked.Exchange(ref Game.speedDirection, (short)Movement.BACKWARD);
                    else Interlocked.Exchange(ref Game.speedDirection, (short)Movement.NONE);

                    while (Console.KeyAvailable) Console.ReadKey(true);
                }
            });
        }
    }
}
