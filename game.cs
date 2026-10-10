using System.Text;
public static class GlobalEvent
{
    public static ulong distance = 0;
    public static bool playerDies = false;

}

public class Game
{
    readonly Player player = new();
    readonly PlayField field = new();

    public const ushort windowHeight = 30;
    public const ushort windowWidth = 50;
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

        do {/* The order of operations is very important
             * First The playfield is updated so the lines in memory look as they will be rendered
             * Then other gameobjects will update
             * Then the field has to do a late update... in order to account for these objects in the scene
             */

            field.Update();
            
            player.Update();

            field.LateUpdate(ref player.position);

            field.Render();

            Thread.Sleep(150 - player.velocity * 10);
            

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