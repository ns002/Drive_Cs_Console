using System.Text;

public class Game
{
    public class PlayField()
    {
        readonly static string COLOR_RESET = "\e[0m";
        //readonly static string Zwart = "\e[30m";
        readonly static string COLOR_RED = "\e[31m";
        readonly static string COLOR_GREEN = "\e[32m";
        readonly static string COLOR_YELLOW = "\e[33m";
        //readonly static string Blauw = "\e[34m";
        readonly static string COLOR_MAGENTA = "\e[35m";
        //readonly static string Cyaan = "\e[36m";
        readonly static string COLOR_WHITE = "\e[37m";
        /*readonly static string FelZwart = "\e[90m";
        readonly static string FelRood = "\e[91m";
        readonly static string FelGroen = "\e[92m";
        readonly static string FelGeel = "\e[93m";
        readonly static string FelBlauw = "\e[94m";
        readonly static string FelMagenta = "\e[95m";
        readonly static string FelCyaan = "\e[96m";
        readonly static string FelWit = "\e[97m";*/

        short GetRandomDirection() => (short)randomNumberGenerator.Next(-1, 2); //get's the random direction the road will move to
        const ushort roadWidth = 12;
        const char roadsideChar = '▓';
        bool roadSideColorDecider = true;
        private string GetRoadsideColored()
        { 
            return roadSideColorDecider ?
            $"{COLOR_WHITE + roadsideChar}" :
            $"{COLOR_RED   + roadsideChar}";
        }
        public const char roadChar = ' ';
        public const char carChar = '▲';
        public const char grassChar = '░';
        public const char coinChar = '*';
        const ushort minRoadSP = 1;
        const ushort maxRoadSP = windowWidth - roadWidth - 3;

        static Random randomNumberGenerator = new Random();

        public string CreateField()
        {
            short roadStartingPoint = (short)randomNumberGenerator.Next(minRoadSP, maxRoadSP);
            StringBuilder field = new();
            field.Append($"{COLOR_GREEN}").Append(grassChar, roadStartingPoint);                    //grass left
            field.Append(GetRoadsideColored());                                                     //roadside left
            field.Append(roadChar, roadWidth);                                                      //road
            field.Append(GetRoadsideColored());                                                     //roadside right
            field.Append($"{COLOR_GREEN}").Append(grassChar, windowWidth - roadStartingPoint + 14); //grass right
            field.Append($"{COLOR_RESET}0\n");                                                      //debug & endline
            for (int i = 1; i < windowHeight; ++i)
            {
                roadSideColorDecider = !roadSideColorDecider;   //switch Red & White roadside
                short direction = GetRandomDirection();
                if (direction < 0) if (roadStartingPoint > minRoadSP) roadStartingPoint += direction;
                if (direction > 0) if (roadStartingPoint < maxRoadSP) roadStartingPoint += direction;
                field.Append($"{COLOR_GREEN}").Append(grassChar, roadStartingPoint);
                field.Append(GetRoadsideColored());
                field.Append(roadChar, roadWidth);
                field.Append(GetRoadsideColored());
                field.Append($"{COLOR_GREEN}").Append(grassChar, windowWidth - roadStartingPoint + 14);
                field.Append($"{COLOR_RESET}{i}").Append('\n');
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
    
    public static void Main()
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