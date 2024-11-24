using ConsoleGameEngine;
using System.Runtime.InteropServices;
using NAudio.Wave;

<<<<<<< Updated upstream
class Player
{
=======
class Player : IDisposable
{ 
>>>>>>> Stashed changes
    private readonly ConsoleEngine engine;

    public Point playerOnePosition;
    public Point playerTwoPosition;

    public Point playerOneBullet;
    public int playerOneBulletAmmount = 0;
    public int playerOneBulletColor = 10;

    public Point playerTwoBullet;
    public int playerTwoBulletAmmount = 0;
    public int playerTwoBulletColor = 20;

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

    /* To Do
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


    // To Do Move this to Utilty.cs
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

    private int attackCooldownFramesOne = 100; 
    private int attackTimeOne = 0;
    private bool attackPressedOne = false; // Flag to check if attack button was pressed

    private int attackCooldownFramesTwo = 100; 
    private int attackTimeTwo = 0;
    private bool attackPressedTwo = false; // Flag to check if attack button was pressed

    private int attack1 = 0;
    private int attack2 = 0;

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
                if (attackTimeOne == 0 && !attackPressedOne && attack1 == 0)
                {
                    attackTimeOne = attackCooldownFramesOne; // Reset cooldown timer
                    attack1++; // Increment attack count to indicate an attack occurred
                    attackPressedOne = true; // Set flag indicating button was pressed

                    // Set bullet's initial position at the tip of the character
                    playerOneBullet = new Point(playerOnePosition.X + 3, playerOnePosition.Y + 5);
                    playerOneBulletAmmount++;
                    if (playerOneBulletAmmount == 1)
                    {
                        PlayOneSound();
                    }
                }
            }
            else
            {
                // Reset the attackPressedOne flag and attack count when button is released
                attackPressedOne = false;
            }
            // Update attack cooldown
            if (attackTimeOne > 0)
            {
                attackTimeOne--;
            }

            // Move the bullet straight down if it has been fired
            if (playerOneBulletAmmount == 1)
            {
                playerOneBullet.Y--; // Only move bullet's Y coordinate upward

                // Reset the bullet if it reaches the top of the console window
                if (playerOneBullet.Y <= 0)
                {
                    playerOneBulletAmmount = 0;
                    attack1 = 0;
                }
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

            // Attack!! - check if enough time has passed since the last attack
            // Attack logic
            if ((GetAsyncKeyState(VK_RETURN) & 0x8000) != 0)
            {
                // If attack is ready and button was not pressed last frame
                if (attackTimeTwo == 0 && !attackPressedTwo && attack2 == 0)
                {
                    attackTimeTwo = attackCooldownFramesTwo; // Reset cooldown timer
                    attack2++; // Increment attack count to indicate an attack occurred
                    attackPressedTwo = true; // Set flag indicating button was pressed

                    // Set bullet's initial position at the tip of the character
                    playerTwoBullet = new Point(playerTwoPosition.X + 3, playerTwoPosition.Y + 5);
                    playerTwoBulletAmmount = 1;
                    if (playerTwoBulletAmmount == 1)
                    {
                        PlayTwoSound();
                    }
                }
            }
            else
            {
                // Reset the attackPressedOne flag and attack count when button is released
                attackPressedTwo = false;
            }
            // Update attack cooldown
            if (attackTimeTwo > 0)
            {
                attackTimeTwo--;
            }

            // Move the bullet straight down if it has been fired
            if (playerTwoBulletAmmount == 1)
            {
                playerTwoBullet.Y--; // Only move bullet's Y coordinate upward

                // Reset the bullet if it reaches the top of the console window
                if (playerTwoBullet.Y <= 0)
                {
                    playerTwoBulletAmmount = 0;
                    attack2 = 0;
                }
            }

            // Add boundary checking if needed (to keep player within the console window)
            playerTwoPosition.X = Math.Max(0, Math.Min(playerTwoPosition.X, Console.WindowWidth - 1));
            playerTwoPosition.Y = Math.Max(0, Math.Min(playerTwoPosition.Y, Console.WindowHeight - 1));
        }

        Render();
    }


    public void Render()
    {
<<<<<<< Updated upstream
        engine.ClearBuffer();
        if (isOnePlayer == true)
        {
            engine.WriteText(new Point(40, 0), Convert.ToString(playerOnePosition), 1);

            foreach (var item in character)
            {
                Point playerHitBox = new Point(item.X+playerOnePosition.X, item.Y+playerOnePosition.Y);
                engine.SetPixel(item + playerOnePosition, 255, ConsoleCharacter.Full);
                foreach (var enemyItem in enemy)
                {
                    if (false) // To Do write a condition if the player got hit by the enemy or got hit by an obstacle
                    {
                        engine.WriteText(new Point(70, 0), "Collision", 1);
                    }
                }
            }

            if (playerOneBulletAmmount == 1)
            {
                engine.SetPixel(playerOneBullet, playerOneBulletColor, ConsoleCharacter.Full);
                engine.SetPixel(new Point(playerOneBullet.X - 1, playerOneBullet.Y), playerOneBulletColor, ConsoleCharacter.Full);
                engine.SetPixel(new Point(playerOneBullet.X + 1, playerOneBullet.Y), playerOneBulletColor, ConsoleCharacter.Full);
                engine.SetPixel(new Point(playerOneBullet.X, playerOneBullet.Y - 1), playerOneBulletColor, ConsoleCharacter.Full);
            }

        }


        if (isTwoPlayer == true)
        {
            engine.WriteText(new Point(40, 1), Convert.ToString(playerTwoPosition), 1);

            foreach (var item in enemy)
            {
                Point playerHitBox = new Point(item.X + playerTwoPosition.X, item.Y + playerTwoPosition.Y);
                engine.SetPixel(item + playerTwoPosition, 1, ConsoleCharacter.Full);
                foreach (var enemyItem in character)
                {
                    if (false) // To Do write a condition if the player got hit by the enemy or got hit by an obstacle
                    {
                        engine.WriteText(new Point(70, 0), "Collision", 1);
                    }
                }
            }


            if (playerTwoBulletAmmount == 1)
            {
                engine.SetPixel(playerTwoBullet, playerTwoBulletColor, ConsoleCharacter.Full);
                engine.SetPixel(new Point(playerTwoBullet.X - 1, playerTwoBullet.Y), playerTwoBulletColor, ConsoleCharacter.Full);
                engine.SetPixel(new Point(playerTwoBullet.X + 1, playerTwoBullet.Y), playerTwoBulletColor, ConsoleCharacter.Full);
                engine.SetPixel(new Point(playerTwoBullet.X, playerTwoBullet.Y - 1), playerTwoBulletColor, ConsoleCharacter.Full);
            }
        }

        string res = $"Player 1 Attack Cooldown : {attackCooldownFramesOne}";
        string res1 = $"Player 1 Attack Time : {attackTimeOne}";
        string res2 = $"Player 1 Attack Ammount : {attack1}";
        
        string res3 = $"Player 2 Attack Cooldown : {attackCooldownFramesTwo}";
        string res4 = $"Player 2 Attack Time : {attackTimeTwo}";
        string res5 = $"Player 2 Attack Ammount : {attack2}";

        string bullet1 = $"{playerOneBullet.Y} : {playerOneBulletAmmount}";
        string bullet2 = $"{playerTwoBullet.Y} : {playerTwoBulletAmmount}";

        engine.WriteText(new Point(40, 3), res, 1);
        engine.WriteText(new Point(40, 4), res1, 1);
        engine.WriteText(new Point(40, 5), res2, 1);


        engine.WriteText(new Point(40, 7), res3, 1);
        engine.WriteText(new Point(40, 8), res4, 1);
        engine.WriteText(new Point(40, 9), res5, 1);

        engine.WriteText(new Point(40, 11), bullet1, 1);
        engine.WriteText(new Point(40, 12), bullet2, 1);
        engine.DisplayBuffer();
=======
        RenderBorder();
        RenderPlayer(playerOne, playerOnePosition, playerOneColor); // for Player One
        RenderPlayer(playerTwo, playerTwoPosition, playerTwoColor); // for Player Two
        RenderBullets(playerOneBullets, playerOneColor); // for Player One Bullets
        RenderBullets(playerTwoBullets, playerTwoColor); // for Player Two Bullets
>>>>>>> Stashed changes
    }

    private Point RotatePoint(Point point, Point center, float angle)
    {
        float cosAngle = (float)Math.Cos(angle * Math.PI / 180);
        float sinAngle = (float)Math.Sin(angle * Math.PI / 180);

        int x = (int)((point.X - center.X) * cosAngle - (point.Y - center.Y) * sinAngle + center.X);
        int y = (int)((point.X - center.X) * sinAngle + (point.Y - center.Y) * cosAngle + center.Y);

        return new Point(x, y);
    }

    public void PlayOneSound()
    {
        try
        {
<<<<<<< Updated upstream
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

=======
            foreach (var bullet in bullets)
            {
                engine.SetPixel(bullet, color, ConsoleCharacter.Full);
            }
        }
        catch (System.InvalidOperationException) 
        {
            foreach (var bullet in bullets)
            {
                engine.SetPixel(bullet, color, ConsoleCharacter.Full);
            }
>>>>>>> Stashed changes
        }
    }
    public void PlayTwoSound()
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
