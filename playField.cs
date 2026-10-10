using System.Text;
public class PlayField()
{
    readonly static string COLOR_RESET = "\e[0m";
    readonly static string COLOR_RED = "\e[31m";
    readonly static string COLOR_GREEN = "\e[32m";
    readonly static string COLOR_YELLOW = "\e[33m";
    readonly static string COLOR_MAGENTA = "\e[35m";
    readonly static string COLOR_WHITE = "\e[37m";

    const char roadsideChar = '▓';
    const char roadChar = ' ';
    const char playerChar = '▲';
    const char grassChar = '░';
    const char coinChar = '*';

    short GetRandomDirection() => (short)randomNumberGenerator.Next(-1, 2); //get's the random direction the road will move to
    
    bool roadSideColorDecider = true;
    string GetRoadsideColored()
    {
        return roadSideColorDecider ?
        $"{COLOR_WHITE + roadsideChar}" :
        $"{COLOR_RED + roadsideChar}";
    }

    public const ushort roadWidth = 12;
    const ushort minRoadSP = 1;
    const ushort maxRoadSP = Game.windowWidth - roadWidth - 3;
    short roadStartingPoint = (short)randomNumberGenerator.Next(minRoadSP, maxRoadSP);

    static Random randomNumberGenerator = new Random();
    StringBuilder[] lines = [];
    
    public void Update()
    {
        /* remembering the discarded line to move it back to the top
         * keep in mind that you need to clear the old string before adding the new one to it
         */ StringBuilder lastLine = lines[Game.windowHeight - 1].Clear();
        
        //shifting the lines
        for (int i = Game.windowHeight - 1; i > 0; --i)
        {
            int j = i - 1;
            lines[i] = lines[j];
        }

        lines[0] = lastLine;
        CreateLine(lines[0]);   //the newly generated line will always be at the top of the stack
    }

    static bool spawning = true;
    const short roadCenter = (roadWidth - 1) / 2;
    static ushort centeredSpawnLocationCounter = 0;
    static ushort visibleSpotCounter;
    public void LateUpdate(ref ushort playerPosition)
    {
        //place coins in line[0]

        //emplace player in line[29]
        visibleSpotCounter = 0;
        for (int i = 0; i < lines[29].Length; ++i)
        {
            switch(lines[29][i])   //inspecting each character in line 29
            {
                default: continue; //if criteria not hit it skips the character; goes to the next i

                case roadChar or roadsideChar or grassChar or coinChar:
                    goto CriteriaMatches;

                CriteriaMatches:   //label: when the string contains a visibile char
                    if (spawning && (lines[29][i] == roadChar || lines[29][i] == coinChar) && ++centeredSpawnLocationCounter == roadCenter)
                    {
                        playerPosition = ;
                        spawning = false;
                    }
                    if (++visibleSpotCounter == playerPosition)
                    {
                        if (lines[29][i] != roadChar || lines[29][i] != coinChar)
                        {
                            //GlobalEvent.playerDies = true;
                            //Interlocked.Exchange(ref Game.running, false);
                            //return;
                        }
                        else if (lines[29][i] == coinChar)
                        {
                            //+ 1 collected coins
                        }
                        lines[29][i] = playerChar;
                    }
                    break;
            }
        }

        ++GlobalEvent.distance;
    }

    public void Render()
    {
        string canvas = string.Empty;
        Console.SetCursorPosition(0, 0); //avoid Console.Clear() to prevent flashing and/or artefacts
        for (int i = 0; i < Game.windowHeight; ++i) canvas += (lines[i].ToString() + '\n');
        Console.Write(canvas);
    }

    public void Create()
    {
        lines = new StringBuilder[Game.windowHeight];
        for (int i = Game.windowHeight - 1; i >= 0; --i)
        {
            lines[i] = new();
            CreateLine(lines[i]);
        }
    }
    ulong lineNumber = 0;
    private void CreateLine(StringBuilder lineReference)
    {
        short direction = GetRandomDirection(); //either -1 (left), 0 (straight) or 1 (right)
        if ((direction < 0 && roadStartingPoint > minRoadSP) || //move road left?
            (direction > 0 && roadStartingPoint < maxRoadSP))   //move road right?
            roadStartingPoint += direction;
        
        roadSideColorDecider = !roadSideColorDecider; //switch behaviour Red & White roadside every time

        lineReference.Append($"{COLOR_GREEN}").Append(grassChar, roadStartingPoint);                    //grass left
        lineReference.Append(GetRoadsideColored());                                                     //roadside left
        lineReference.Append(roadChar, roadWidth);                                                      //road
        lineReference.Append(GetRoadsideColored());                                                     //roadside right
        lineReference.Append($"{COLOR_GREEN}").Append(grassChar, Game.windowWidth - roadStartingPoint + 14); //grass right
        lineReference.Append($"{COLOR_RESET + lineNumber++}"); //makes debug info at the end normal text color and add a lineNumber to identify it
    }
}
