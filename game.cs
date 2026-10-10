using System.Text;
public class Game
{
    public class Player()
    {
        //public short xPosition = 25;
        public const ushort maxVelocity = 10;
        public const ushort minVelocity = 1;
        public ushort velocity = minVelocity;

    }

    readonly Player player = new();
    readonly PlayField field = new();

    ulong distance = 0;
    public const ushort windowHeight = 30;
    public const ushort windowWidth = 50;
    volatile public static Input.Movement steerDirection = Input.Movement.NONE;
    volatile public static Input.Movement speedDirection = Input.Movement.NONE;
    volatile public static bool running = true;

    public static void Main()
    {
        Game game = new Game();
        game.field.Create();
        game.Run();
    }
    
    public void Run()
    {
        Console.CursorVisible = false;
        Console.OutputEncoding = Encoding.UTF8;
        Input.StartInputListener(); //start the background input listener

        do
        {
            //if (player.xPosition > 1 && steerDirection == Input.Movement.LEFT) --player.xPosition;
            //else if (player.xPosition < windowWidth - 1 && steerDirection == Input.Movement.RIGHT) ++player.xPosition;
            if (player.velocity > Player.minVelocity && speedDirection == Input.Movement.SLOWER) --player.velocity;
            else if (player.velocity < Player.maxVelocity && speedDirection == Input.Movement.FASTER) ++player.velocity;

            field.Update();
            field.Render();

            Thread.Sleep(150 - player.velocity * 10);
            ++distance;

            //tested the car
            /*StringBuilder road = new();
            road.Append(roadChar, carPosition - 1);
            road.Append(carChar);
            road.Append("  speed: ");
            road.Append((carVelocity * 10) + 30);
            Console.WriteLine(road.ToString());*/

        } while (running);
    }
}