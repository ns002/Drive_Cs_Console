using System.Text;

public class Game
{
    public class PlayField()
    {
        private readonly static string Reset = "\e[0m";
        private readonly static string Zwart = "\e[30m";
        private readonly static string Rood = "\e[31m";
        private readonly static string Groen = "\e[32m";
        private readonly static string Geel = "\e[33m";
        private readonly static string Blauw = "\e[34m";
        private readonly static string Magenta = "\e[35m";
        private readonly static string Cyaan = "\e[36m";
        private readonly static string Wit = "\e[37m";
        private readonly static string FelZwart = "\e[90m";
        private readonly static string FelRood = "\e[91m";
        private readonly static string FelGroen = "\e[92m";
        private readonly static string FelGeel = "\e[93m";
        private readonly static string FelBlauw = "\e[94m";
        private readonly static string FelMagenta = "\e[95m";
        private readonly static string FelCyaan = "\e[96m";
        private readonly static string FelWit = "\e[97m";

        const ushort roadWidth = 12;
        public readonly string[] roadSideChar = [$"{Wit}▒{Reset}", $"{Rood}▓{Reset}"];
        private static Random randomNumberGenerator = new Random();
        private short GetRandomDirection() => (short)randomNumberGenerator.Next(-1, 2); //get's the random direction the road will move to
        private bool roadSidePicker = true;
        private ushort PickRoadSideIndex() => (roadSidePicker = !roadSidePicker) ? (ushort)0 : (ushort)1;
        public const char roadChar = ' ';
        public const char carChar = '▲';
        public const char grassChar = '░';
        public const char coinChar = '*';
        private const ushort minRoadStartPoint = 1;
        private const ushort maxRoadStartPoint = windowWidth - roadWidth - 3;

        public string CreateField()
        {
            short roadStartingPoint = (short)randomNumberGenerator.Next(minRoadStartPoint, maxRoadStartPoint);
            StringBuilder field = new();
            field.Append($"{Groen}").Append(grassChar, roadStartingPoint);
            field.Append(roadSideChar[PickRoadSideIndex()]);
            field.Append(roadChar, roadWidth);
            field.Append(roadSideChar[PickRoadSideIndex()]);
            field.Append($"{Groen}").Append(grassChar, windowWidth - roadStartingPoint + 14);
            field.Append($"{Reset}0").Append('\n');
            for (int i = 1; i < windowHeight; ++i)
            {
                roadSidePicker = !roadSidePicker;
                short direction = GetRandomDirection();
                if (direction < 0) if (roadStartingPoint > minRoadStartPoint) roadStartingPoint += direction;
                if (direction > 0) if (roadStartingPoint < maxRoadStartPoint) roadStartingPoint += direction;
                field.Append($"{Groen}").Append(grassChar, roadStartingPoint);
                field.Append(roadSideChar[PickRoadSideIndex()]);
                field.Append(roadChar, roadWidth);
                field.Append(roadSideChar[PickRoadSideIndex()]);
                field.Append($"{Groen}").Append(grassChar, windowWidth - roadStartingPoint + 14);
                field.Append($"{Reset}{i}").Append('\n');
            }
            return field.ToString();
        }
    }
    public class Player()
    {
        public short xPosition = 25;
        public const ushort maxVelocity = 10;
        public const ushort minVelocity = 1;
        public ushort velocity = minVelocity;

    }

    readonly Player player = new();
    readonly PlayField field = new();

    long distance = 0;
    const ushort windowHeight = 30;
    const ushort windowWidth = 50;
    volatile public static short steerDirection = 0;
    volatile public static short speedDirection = 0;
    volatile public static bool running = true;
    
    public static void Main(string[] args)
    {
        Game game = new Game();
        game.Run();
    }
    
    public void Run()
    {
        Console.CursorVisible = false;
        Console.OutputEncoding = Encoding.UTF8;
        Input.StartInputListener(); // Start the background input listener method

        do
        {
            //left
            if (player.xPosition > 1 && steerDirection == -1) --player.xPosition;
            //right
            else if (player.xPosition < windowWidth - 1 && steerDirection == 1) ++player.xPosition;
            //slower
            if (player.velocity > Player.minVelocity && speedDirection == -1) --player.velocity;
            //faster
            else if (player.velocity < Player.maxVelocity && speedDirection == 1) ++player.velocity;


            Console.Write(field.CreateField());

            Thread.Sleep(3000);
            //Thread.Sleep(150 - player.velocity * 10);
            ++distance;

            //worldgen
            /*StringBuilder road = new();
            road.Append(roadChar, carPosition - 1);
            road.Append(carChar);
            road.Append("  speed: ");
            road.Append((carVelocity * 10) + 30);
            Console.WriteLine(road.ToString());*/

        } while (running);
    }
}