using _Game_Main;
using ConsoleGameEngine;
using SharpDX.DirectInput;

class MainMenu
{
    private readonly ConsoleEngine engine;
    private readonly Program program;
    public static FigletFont? font;
    public static FigletFont? font1;

    private Point[] selector = { new Point(-2, 0), 
                                 new Point(-2, 1), new Point(-1, 1),
                                 new Point(-2, 2), new Point(-1, 2),new Point(0, 2),
                                 new Point(-2, 3), new Point(-1, 3),
                                 new Point(-2, 4) 
                               };
    public Point selectorPosition = new Point(4, 25);

    public string player1Name = "";

    public string currentPage = "MainMenu";

    private readonly int screenWidth;
    private readonly int screenHeight;

    private int moveCooldown = 7;
    private int moveTime = 0;

    private int typeCooldown = 5;
    private int typeTime = 0;

    private int enterCooldown = 5;
    private int enterTime = 0;

    private int delCooldown = 5;
    private int delTime = 0;

    private bool inputName1 = false;

    private int tutorialPage = 1;

    private DirectInput directInput;
    private Keyboard keyboard;
    private KeyboardState keyboardState;

    private bool isTransitioningToScores = false; // Flag to indicate transition

    public MainMenu(ConsoleEngine engine, int screenWidth, int screenHeight, bool isPlaying, Program program)
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
        if (currentPage != "About")
            GameTitle();

        switch (currentPage)
        {
            case "MainMenu":
                RenderSelector();
                RenderMainMenu();
                break;
            case "Play":
                Render1PlayerMenu();
                if (!inputName1) RenderSelector();
                break;
            case "Scores":
                RenderScores();
                break;
            case "Tutorial":
                RenderTutorial();
                break;
            case "About":
                RenderAbout();
                break;
            case "Survival":
                program.isPlaying = true;
                break;
        }
        // displays debug info
        // RenderDebugInfo();
        engine.DisplayBuffer();
    }

    public static void LoadFonts()
    {
        font = FigletFont.Load("Fonts/3d.flf");
        font1 = FigletFont.Load("Fonts/smslant.flf");
    }

    private void RenderSelector()
    {
        foreach (var item in selector)
        {
            engine.SetPixel(new Point(item.X + selectorPosition.X, item.Y + selectorPosition.Y), 7, ConsoleCharacter.Full);
        }
    }

    private void GameTitle()
    {
        string title = "VOID  INVADER";
        int estimatedWidth = title.Length * 8;
        int startX = (screenWidth - 60 - estimatedWidth) / 2;
        engine.WriteFiglet(new Point(startX, 10), title, font, 7);
    }

    private void RenderMainMenu()
    {
        string[] menuOptions = { "Play", "Scores", "Tutorial", "About", "Exit" };
        for (int i = 0; i < menuOptions.Length; i++)
        {
            engine.WriteFiglet(new Point(10, 25 + (i * 5)), menuOptions[i], font1, 7);
        }
    }

    private void Render1PlayerMenu()
    {
        engine.WriteFiglet(new Point(10, 20), "Enter your name:", font1, 7);
        engine.WriteFiglet(new Point(10, 25), player1Name, font1, 7);
        engine.WriteFiglet(new Point(10, 30), "Survival", font1, 7);
        engine.WriteFiglet(new Point(10, 35), "Back", font1, 7);

        if (engine.GetKey(ConsoleKey.Enter) && player1Name != "" && enterTime == 0)
        {
            enterTime = enterCooldown;
            inputName1 = false;
        }
    }

    private void RenderScores()
    {
        if (isTransitioningToScores)
        {
            // Reset the transition flag after rendering scores
            isTransitioningToScores = false;
            return; // Prevent immediate return to main menu
        }

        var scores = program.ReadScore("Scores/Scores.xml");

        // Display message to return to main menu
        engine.WriteFiglet(new Point(10, screenHeight - 10), "Press Enter to return to Main Menu", font1, 7);

        int offset = 5;
        int columnOffset = 0;
        int maxScoresPerColumn = 10;
        
        engine.WriteFiglet(new Point(10, 35), "Name        Scores : EscEnemy", font1, 7);
        for (int i = 0; i < scores.Count; i++)
        {
            if (i == maxScoresPerColumn)
            {
                // Move to the second column
                columnOffset = 180; // Adjust this value based on your screen width
                offset = 5; // Reset offset for the new column
            }

            var score = scores[i];
            engine.WriteFiglet(new Point(10 + columnOffset, 40 + offset), $"{i + 1}: {score.Player}", font1, 7);
            engine.WriteFiglet(new Point(100 + columnOffset, 40 + offset), $"{score.Value} : {score.EscapedEnemies}", font1, 7); // Display escapedEnemies
            offset += 5;
        }

        // Check for Enter key press to return to main menu
        if (engine.GetKey(ConsoleKey.Enter) && enterTime == 0)
        {
            enterTime = enterCooldown;
            ResetToMainMenu();
        }
    }

    private void RenderTutorial()
    {
        LoadFonts();

        switch (tutorialPage)
        {
            case 1:
                RenderTutorialPage1();
                break;
            case 2:
                RenderTutorialPage2();
                break;
        }

        // Display navigation instructions
        engine.WriteFiglet(new Point(10, screenHeight - 10), "Press Enter to continue, Esc to return to Main Menu", font1, 7);
    }

    private void RenderTutorialPage1()
    {
        engine.WriteFiglet(new Point(10, 30), "Tutorial - Page 1", font1, 7);
        engine.WriteFiglet(new Point(10, 40), "Controls:", font1, 7);
        engine.WriteFiglet(new Point(10, 50), "W - Move Up", font1, 7);
        engine.WriteFiglet(new Point(10, 55), "A - Move Left", font1, 7);
        engine.WriteFiglet(new Point(10, 60), "S - Move Down", font1, 7);
        engine.WriteFiglet(new Point(10, 65), "D - Move Right", font1, 7);
        engine.WriteFiglet(new Point(10, 70), "Space - Fire", font1, 7);
        engine.WriteFiglet(new Point(10, 75), "Esc - Pause", font1, 7);
    }

    private void RenderTutorialPage2()
    {
        engine.WriteFiglet(new Point(10, 30), "Tutorial - Page 2", font1, 7);
        engine.WriteFiglet(new Point(10, 40), "Enemy Types:", font1, 7);
        engine.WriteFiglet(new Point(10, 50), "Green - Normal Enemy", font1, 2);
        engine.WriteFiglet(new Point(10, 55), "Type 1 : Blue - Can Split into Two Green", font1, 3);
        engine.WriteFiglet(new Point(10, 60), "Type 2 : Blue - Can Follow the Player", font1, 3);
        engine.WriteFiglet(new Point(10, 65), "Yellow - Can Split into Two Blue", font1, 6);
        engine.WriteFiglet(new Point(10, 75), "Life Cube Spawns when an enemy died with a chance of 5%", font1, 7);
    }

    private void RenderAbout()
    {
        string aboutText = "Void Invader is a simple console game developed in C# using the";
        string aboutText1 = "ConsoleGameEngine library as the backbone.";
        string developer1 = "John Alex B. Alcazar";
        string developer2 = "Emmanuel S. Delatorre";
        string developer3 = "Jhaztine Laitrell Datuin";

        string credits1 = "@javidx9 - yt";
        string credits2 = "ollelogdahl - GitHub";

        engine.WriteFiglet(new Point(10, 10), aboutText, font1, 7);
        engine.WriteFiglet(new Point(10, 15), aboutText1, font1, 7);


        engine.WriteFiglet(new Point(10, program.Height-40), "Developers:", font1, 7);
        engine.WriteFiglet(new Point(10, program.Height-30), developer1, font1, 7);
        engine.WriteFiglet(new Point(10, program.Height-25), developer2, font1, 7);
        engine.WriteFiglet(new Point(10, program.Height-20), developer3, font1, 7);

        engine.WriteFiglet(new Point(program.Width-130, program.Height-40), "Credits to:", font1, 7);
        engine.WriteFiglet(new Point(program.Width-130, program.Height-30), credits1, font1, 7);
        engine.WriteFiglet(new Point(program.Width-130, program.Height-25), credits2, font1, 7);


        engine.WriteFiglet(new Point(10, program.Height - 5), "Press Enter to Return to Main Menu", font1, 7);

        // Check for Enter key press to return to main menu
        if (engine.GetKey(ConsoleKey.Enter) && enterTime == 0)
        {
            enterTime = enterCooldown;
            ResetToMainMenu();
        }
    }

    private void RenderDebugInfo()
    {
        string debugInfo = $"Page: {currentPage}, Selector Position: {selectorPosition.X}, {selectorPosition.Y}";
        engine.WriteFiglet(new Point(10, screenHeight - 5), debugInfo, font1, 2);
    }

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
            case "Play":
                Handle1PlayerMenuInput();
                break;
            case "Tutorial":
                HandleTutorialInput();
                break;
        }

        HandleKeyboardInput();
    }

    private void HandleMainMenuInput()
    {
        // Move the selector up and down
        if (engine.GetKey(ConsoleKey.W) && selectorPosition.Y > 25 && CanType(ref moveTime, moveCooldown))
        {
            selectorPosition.Y -= 5;
        }
        else if (engine.GetKey(ConsoleKey.S) && selectorPosition.Y < 45 && CanType(ref moveTime, moveCooldown)) // Adjusted to 40 for the new option
        {
            selectorPosition.Y += 5;
        }

        // Check for Enter key press
        if (engine.GetKey(ConsoleKey.Enter) && enterTime == 0)
        {
            enterTime = enterCooldown; // Set cooldown for Enter key
            switch (selectorPosition.Y)
            {
                case 25:
                    currentPage = "Play";
                    inputName1 = true;
                    break;
                case 30:
                    isTransitioningToScores = true; // Set the transition flag
                    currentPage = "Scores";
                    break;
                case 35:
                    currentPage = "Tutorial";
                    break;
                case 40:
                    currentPage = "About";
                    break;
                case 45:
                    ExitGame();
                    break;
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
        else if (engine.GetKey(ConsoleKey.S) && selectorPosition.Y < 40 && CanType(ref moveTime, moveCooldown))
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

    private void HandleTutorialInput()
    {
        if (engine.GetKey(ConsoleKey.Enter) && enterTime == 0)
        {
            enterTime = enterCooldown;
            if (tutorialPage == 2)
            {
                tutorialPage--;
            }
            else if (tutorialPage == 1)
            {
                tutorialPage++;
            }
        }
        else if (engine.GetKey(ConsoleKey.Escape) && enterTime == 0)
        {
            enterTime = enterCooldown;
            ResetToMainMenu();
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

    public void ResetToMainMenu()
    {
        currentPage = "MainMenu";
        selectorPosition = new Point(4, 25);
        inputName1 = false;
        isTransitioningToScores = false;
        player1Name = "";
        // Reset other necessary states if needed
    }

    private void ExitGame()
    {
        keyboard.Unacquire();
        directInput.Dispose();
        Environment.Exit(0);
    }

    private void HandleKeyboardInput()
    {
        if (!inputName1) return;

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
