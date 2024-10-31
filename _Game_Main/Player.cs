using ConsoleGameEngine;
using System.Runtime.InteropServices;
using NAudio.Wave;

class Player
{
    private readonly ConsoleEngine engine;
    public Point playerOnePosition;
    public Point playerTwoPosition;
    private Point current;
    private Point current1;
    private Point current2;
    public Point[] character = { new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0), new Point(4, 0), new Point(5, 0), new Point(6, 0),
                                 /*           */  new Point(1, 1), new Point(2, 1), new Point(3, 1), new Point(4, 1), new Point(5, 1),
                                 /*           */  new Point(1, 2), new Point(2, 2), new Point(3, 2), new Point(4, 2), new Point(5, 2),
                                 /*                             */ new Point(2, 3), new Point(3, 3), new Point(4, 3),
                                 /*                             */ new Point(2, 4), new Point(3, 4), new Point(4, 4),
                                 /*                                              */ new Point(3, 5),
                                };
    public Point[] enemy = { new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0), new Point(4, 0), new Point(5, 0), new Point(6, 0),
                                 /*           */  new Point(1, 1), new Point(2, 1), new Point(3, 1), new Point(4, 1), new Point(5, 1),
                                 /*           */  new Point(1, 2), new Point(2, 2), new Point(3, 2), new Point(4, 2), new Point(5, 2),
                                 /*                             */ new Point(2, 3), new Point(3, 3), new Point(4, 3),
                                 /*                             */ new Point(2, 4), new Point(3, 4), new Point(4, 4),
                                 /*                                              */ new Point(3, 5),
                                };

    /*
     * Automatically change this if the user chooses 1/2player
     * if 1 player
     *  isOnPlayer will be true and isTwoPlayer will be false
     * if 2 player
     *  both isOne/TwoPlayer will be true
     */
    bool isOnePlayer = true;
    bool isTwoPlayer = true;

    public Player(ConsoleEngine engine, Point initialPosition)
    {
        this.engine = engine;
        if (isOnePlayer==true)
            playerOnePosition = new Point(10, 10);
        if (isTwoPlayer==true)
            playerTwoPosition = new Point(20, 10); // Changed initial position to differentiate players
        current = new Point(0, 0);
        current1 = new Point(3, 5);
        current2 = new Point(6, 0);
    }

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    private const int VK_W = 0x57; // W key
    private const int VK_S = 0x53; // S key
    private const int VK_A = 0x41; // A key
    private const int VK_D = 0x44; // D key
    private const int VK_UP = 0x26; // Up Arrow
    private const int VK_DOWN = 0x28; // Down Arrow
    private const int VK_LEFT = 0x25; // Left Arrow
    private const int VK_RIGHT = 0x27; // Right Arrow
    private const int VK_RETURN = 0x0D; // Enter
    private const int VK_SPACE = 0x20; // Space

    private int attackCooldownFrames = 100; 
    private int attackTime = 0;
    private bool attackPressed = false; // Flag to check if attack button was pressed

    private int attack = 0;

    public void Update()
    {
        // Player 1
        if (isOnePlayer == true)
        {
            // Player movement logic using GetAsyncKeyState
            if ((GetAsyncKeyState(VK_W) & 0x8000) != 0) playerOnePosition.Y--;
            if ((GetAsyncKeyState(VK_S) & 0x8000) != 0) playerOnePosition.Y++;
            if ((GetAsyncKeyState(VK_A) & 0x8000) != 0) playerOnePosition.X--;
            if ((GetAsyncKeyState(VK_D) & 0x8000) != 0) playerOnePosition.X++;

            // Attack!! - check if enough time has passed since the last attack
            // Attack logic
            if ((GetAsyncKeyState(VK_SPACE) & 0x8000) != 0)
            {
                // If attack is ready and button was not pressed last frame
                if (attackTime == 0 && !attackPressed)
                {
                    PlaySound();
                    attackTime = attackCooldownFrames; // Reset cooldown timer
                    attack++; // Increment attack count to indicate an attack occurred
                    attackPressed = true; // Set flag indicating button was pressed
                }
            }
            else
            {
                // Reset the attackPressed flag and attack count when button is released
                attackPressed = false;
                attack = 0; // Reset attack count to allow a new attack on next press
            }

            // Update attack time countdown
            if (attackTime > 0)
            {
                attackTime--; // Decrease cooldown timer
            }

            // Add boundary checking if needed (to keep player within the console window)
            playerOnePosition.X = Math.Max(0, Math.Min(playerOnePosition.X, Console.WindowWidth - 1));
            playerOnePosition.Y = Math.Max(0, Math.Min(playerOnePosition.Y, Console.WindowHeight - 1));
            
        }

        // Player 2
        if (isTwoPlayer == true)
        {
            // Player movement logic using GetAsyncKeyState
            if ((GetAsyncKeyState(VK_UP) & 0x8000) != 0) playerTwoPosition.Y--;
            if ((GetAsyncKeyState(VK_DOWN) & 0x8000) != 0) playerTwoPosition.Y++;
            if ((GetAsyncKeyState(VK_LEFT) & 0x8000) != 0) playerTwoPosition.X--;
            if ((GetAsyncKeyState(VK_RIGHT) & 0x8000) != 0) playerTwoPosition.X++;

            // Attack!!
            if ((GetAsyncKeyState(VK_RETURN) & 0x8000) != 0)
            {
                PlaySound();
            }

            // Add boundary checking if needed (to keep player within the console window)
            playerTwoPosition.X = Math.Max(0, Math.Min(playerTwoPosition.X, Console.WindowWidth - 1));
            playerTwoPosition.Y = Math.Max(0, Math.Min(playerTwoPosition.Y, Console.WindowHeight - 1));
        }

        Render();
    }


    public void Render()
    {
        engine.ClearBuffer();
        if (isOnePlayer == true)
        {
            engine.WriteText(new Point(40, 0), Convert.ToString(playerOnePosition), 1);
            //engine.Triangle(current + playerOnePosition, current2 + playerOnePosition, current1 + playerOnePosition, 255, ConsoleCharacter.Full);

            foreach (var item in character)
            {
                Point playerHitBox = new Point(item.X+playerOnePosition.X, item.Y+playerOnePosition.Y);
                engine.SetPixel(item + playerOnePosition, 255, ConsoleCharacter.Full);
                foreach (var enemyItem in enemy)
                {
                    if (playerHitBox.X == enemyItem.X+playerTwoPosition.X && playerHitBox.Y == enemyItem.Y+playerTwoPosition.Y)
                    {
                        engine.WriteText(new Point(70, 0), "Collision", 1);
                    }
                }
            }
        }


        if (isTwoPlayer == true)
        {
            engine.WriteText(new Point(40, 1), Convert.ToString(playerTwoPosition), 1);
            engine.Triangle(current + playerTwoPosition, current2 + playerTwoPosition, current1 + playerTwoPosition, 200, ConsoleCharacter.Full);

            foreach (var item in enemy)
            {
                Point playerHitBox = new Point(item.X + playerTwoPosition.X, item.Y + playerTwoPosition.Y);
                engine.SetPixel(item + playerTwoPosition, 1, ConsoleCharacter.Full);
                foreach (var enemyItem in character)
                {
                    if (playerHitBox.X == enemyItem.X + playerOnePosition.X && playerHitBox.Y == enemyItem.Y + playerOnePosition.Y)
                    {
                        engine.WriteText(new Point(70, 0), "Collision", 1);
                    }
                }
            }
        }

        string res = $"Attack Cooldown : {attackCooldownFrames}";
        string res1 = $"Attack Time : {attackTime}";
        string res2 = $"Attack Ammount : {attack}";

        engine.WriteText(new Point(40, 3), res, 1);
        engine.WriteText(new Point(40, 4), res1, 1);
        engine.WriteText(new Point(40, 5), res2, 1);

        engine.DisplayBuffer();
    }

    private Point RotatePoint(Point point, Point center, float angle)
    {
        float cosAngle = (float)Math.Cos(angle * Math.PI / 180);
        float sinAngle = (float)Math.Sin(angle * Math.PI / 180);

        int x = (int)((point.X - center.X) * cosAngle - (point.Y - center.Y) * sinAngle + center.X);
        int y = (int)((point.X - center.X) * sinAngle + (point.Y - center.Y) * cosAngle + center.Y);

        return new Point(x, y);
    }

    public void PlaySound()
    {
        try
        {
            string mp3FilePath = "C:\\Users\\Styx\\Desktop\\ITEC102FinalMain\\_Game_Main\\pew-pew-two-102442.mp3";
            using (var audioFile = new AudioFileReader(mp3FilePath))
            {
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                    Thread.Sleep(500);
                    outputDevice.Stop();
                }
            }
        }
        catch (System.AccessViolationException)
        {
            
        }
        catch (System.Runtime.InteropServices.COMException) 
        { 

        }
    }
}
