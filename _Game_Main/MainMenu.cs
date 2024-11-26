using _Game_Main;
using ConsoleGameEngine;
using SharpDX.DirectInput;

class MainMenu
{
    private readonly ConsoleEngine engine;
    public static FigletFont font;
    public static FigletFont font1;
    private readonly Program program;

    private Point[] selector = {new Point(0, 1), new Point(0, 2), new Point(0, 3), new Point(0, 4),
                                new Point(56, 1), new Point(56, 2), new Point(56, 3), new Point(56, 4)};
    public Point selectorPosition = new Point(165, 25);

    private int borderColor = 1;

    public string player1Name = "";

    public string currentPage = "MainMenu";

    private readonly int screenWidth;
    private readonly int screenHeight;

    private readonly bool isPlaying;

    private int moveCooldown = 10;
    private int moveTime = 0;

    private int typeCooldown = 8;
    private int typeTime = 0;

    private int enterCooldown = 8; 
    private int enterTime = 0;      

    private int delCooldown = 8;
    private int delTime = 0;

    private bool inputName1 = false;

    private DirectInput directInput;
    private Keyboard keyboard;
    private KeyboardState keyboardState;

    public MainMenu(ConsoleEngine engine, int screenWidth, int screenHeight,bool isPlaying, Program program)
    {
        this.engine = engine;
        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;
        this.program = program;

        directInput = new DirectInput();
        keyboard = new Keyboard(directInput);
        keyboard.Acquire();
    }

    public void Render()
    {
        LoadFonts();

        engine.ClearBuffer();

        RenderBorder();
        GameTitle();

        switch (currentPage)
        {
            case "MainMenu":
                RenderMainMenu();
                RenderSelector();
                break;
            case "1Player":
                Render1PlayerMenu();
                if (!inputName1) RenderSelector();
                break;
            case "Scores":
                // Additional rendering for other pages can go here
                break;
            case "Survival":
                program.isPlaying = true;
                break;
        }

        RenderDebugInfo();
        engine.DisplayBuffer();
    }

    // Methods for Rendering Stuff
    private void LoadFonts()
    {
        font = FigletFont.Load("C:\\Users\\Styx\\Desktop\\ITEC102FinalMain\\_Game_Main\\3d.flf");
        font1 = FigletFont.Load("C:\\Users\\Styx\\Desktop\\ITEC102FinalMain\\_Game_Main\\smslant.flf");
    }
    public void RenderBorder ()
    {
        for (int x = 0; x < screenWidth; x++)
        {
            engine.SetPixel(new Point(x, 0), borderColor, ConsoleCharacter.Full);
            engine.SetPixel(new Point(x, screenHeight - 1), borderColor, ConsoleCharacter.Full);
        }

        // Render left and right borders
        for (int y = 0; y < screenHeight; y++)
        {
            engine.SetPixel(new Point(0, y), borderColor, ConsoleCharacter.Full);
            engine.SetPixel(new Point(screenWidth - 1, y), borderColor, ConsoleCharacter.Full);
        }

        for (int z = 0; z < screenHeight; z++)
        {
            engine.SetPixel(new Point(screenWidth-60, screenHeight-z), borderColor, ConsoleCharacter.Full);
        }
    }
    private void RenderSelector()
    {
        foreach (var item in selector)
        {
            engine.SetPixel(new Point(item.X + selectorPosition.X, item.Y + selectorPosition.Y), 2, ConsoleCharacter.Full);
        }
    }
    // Render Game Title
    private void GameTitle()
    {
        string title = "VOID  INVADER";

        // Estimate the width of the title (assuming each character takes 5 pixels, adjust as needed)
        int estimatedWidth = title.Length * 8;

        // Calculate the starting X position for centering the title
        int startX = (screenWidth - 60 - estimatedWidth) / 2;

        // Render the title at the calculated position
        engine.WriteFiglet(new Point(startX, 10), title, font, 2);
    }

    private void RenderMainMenu()
    {
        string[] menuOptions = { "1-Player", "Scores", "Exit" };

        // Iterate through menu options and calculate centered positions
        for (int i = 0; i < menuOptions.Length; i++)
        {
            // Estimate the width of the text (assuming each character takes up 5 pixels)
            int estimatedWidth = menuOptions[i].Length * 5;

            // Calculate the starting X position for centering the text
            int startX = (screenWidth - 60 - estimatedWidth) / 2;

            // Write the Figlet text at the calculated position
            engine.WriteFiglet(new Point(startX, 25 + (i * 5)), menuOptions[i], font1, 2);
        }
    }
    // Render 1-Player Menu
    private void Render1PlayerMenu()
    {
        int num = 6;
        // Center the "Enter your name:" text
        int enterTextWidth = "Enter your name:".Length * num; // Adjust multiplier as necessary
        int startXEnter = (screenWidth - 60 - enterTextWidth) / 2;
        engine.WriteFiglet(new Point(startXEnter, 20), "Enter your name:", font1, 2);

        // Center the player1Name text
        int playerNameWidth = player1Name.Length * num; // Adjust multiplier as necessary
        int startXName = (screenWidth - 60 - playerNameWidth) / 2;
        engine.WriteFiglet(new Point(startXName, 25), player1Name, font1, 2);

        // Center the "Survival" text
        int survivalTextWidth = "Survival".Length * num; // Adjust multiplier as necessary
        int startXSurvival = (screenWidth - 60 - survivalTextWidth) / 2;
        engine.WriteFiglet(new Point(startXSurvival, 30), "Survival", font1, 2);

        // Center the "Back" text
        int backTextWidth = "Back".Length * num; // Adjust multiplier as necessary
        int startXBack = (screenWidth - 60 - backTextWidth) / 2;
        engine.WriteFiglet(new Point(startXBack, 35), "Back", font1, 2);

        if (engine.GetKey(ConsoleKey.Enter) && player1Name != "" && enterTime == 0)
        {
            enterTime = enterCooldown;
            inputName1 = false;
        }
    }

    private void RenderSurvival()
    {

    }

    // Render Scores
    private void RenderScores()
    {

    }

    private void RenderDebugInfo()
    {
        string debugInfo = $"Page: {currentPage}, Selector Position: {selectorPosition.X}, {selectorPosition.Y}";
        engine.WriteFiglet(new Point(10, screenHeight-5), debugInfo, font1,2);
    }
    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ //

    public void Update()
    {
        keyboardState = keyboard.GetCurrentState();

        // Reduce the cooldown timer for Enter key
        if (enterTime > 0) enterTime--;

        switch (currentPage)
        {
            case "MainMenu":
                HandleMainMenuInput();
                break;
            case "1Player":
                Handle1PlayerMenuInput();
                break;
        }

        HandleKeyboardInput();
    }
    // Methods for Update
    private void HandleMainMenuInput()
    {
        if (engine.GetKey(ConsoleKey.W) && selectorPosition.Y > 25 && CanType(ref moveTime, moveCooldown))
        {
            selectorPosition.Y -= 5;
        }
        else if (engine.GetKey(ConsoleKey.S) && selectorPosition.Y < 40 && CanType(ref moveTime, moveCooldown))
        {
            selectorPosition.Y += 5;
        }

        if (engine.GetKey(ConsoleKey.Enter) && enterTime == 0)
        {
            enterTime = enterCooldown;
            switch (selectorPosition.Y)
            {
                case 25: currentPage = "1Player"; inputName1 = true; break;
                case 35: currentPage = "Scores"; break;
                case 40: ExitGame(); break;
            }
        }
    }

    private void Handle1PlayerMenuInput()
    {
        if (inputName1) return;
        if (engine.GetKey(ConsoleKey.W) && selectorPosition.Y > 30 && CanType(ref moveTime, moveCooldown))
        {
            selectorPosition.Y -= 5;
        }
        else if (engine.GetKey(ConsoleKey.S) && selectorPosition.Y < 35 && CanType(ref moveTime, moveCooldown))
        {
            selectorPosition.Y += 5;
        }

        if (engine.GetKey(ConsoleKey.Enter) && enterTime == 0)
        {
            enterTime = enterCooldown;
            if (selectorPosition.Y == 30)
            {
                currentPage = "Survival";
            }
            else if (selectorPosition.Y == 35)
            {
                ResetToMainMenu();
            }
        }
    }

    private bool CanType(ref int moveTime, int moveCooldown)
    {
        if (moveTime == 0)
        {
            moveTime = moveCooldown;
            return true;
        }
        if (moveTime > 0) moveTime--;
        return false;
    }

    private void ResetToMainMenu()
    {
        currentPage = "MainMenu";
        selectorPosition = new Point(165, 25);
        player1Name = "";
        inputName1 = false;
    }

    private void ExitGame()
    {
        keyboard.Unacquire();
        directInput.Dispose();
        Environment.Exit(0);
    }

    // Handle keyboard input to type the player's name
    private void HandleKeyboardInput()
    {

        if (!inputName1 ) return;

        if (keyboardState.IsPressed(Key.Back) && CanType(ref delTime, delCooldown))
        {
            try
            {
                player1Name = player1Name.Substring(0, player1Name.Length - 1); // Remove last character
            }
            catch (System.ArgumentOutOfRangeException) { }
        }

        // Iterate over all keys to see if they are pressed
        for (int i = 0; i < 256; i++)
        {
            if (keyboardState.IsPressed((Key)i) && CanType(ref typeTime, typeCooldown))
            {
                char keyChar = GetCharacterFromKey((Key)i);
                if (keyChar != '\0' && player1Name.Length < 10)
                {
                    player1Name += keyChar;
                }
            }
        }
    }

    // Convert the key to a character (you can extend this for other key codes)
    private char GetCharacterFromKey(Key key)
    {
        switch (key)
        {
            case Key.A: return 'A';
            case Key.B: return 'B';
            case Key.C: return 'C';
            case Key.D: return 'D';
            case Key.E: return 'E';
            case Key.F: return 'F';
            case Key.G: return 'G';
            case Key.H: return 'H';
            case Key.I: return 'I';
            case Key.J: return 'J';
            case Key.K: return 'K';
            case Key.L: return 'L';
            case Key.M: return 'M';
            case Key.N: return 'N';
            case Key.O: return 'O';
            case Key.P: return 'P';
            case Key.Q: return 'Q';
            case Key.R: return 'R';
            case Key.S: return 'S';
            case Key.T: return 'T';
            case Key.U: return 'U';
            case Key.V: return 'V';
            case Key.W: return 'W';
            case Key.X: return 'X';
            case Key.Y: return 'Y';
            case Key.Z: return 'Z';
                
            default: return '\0'; // Return null character for unrecognized keys
        }
    }
}
