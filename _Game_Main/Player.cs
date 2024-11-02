using ConsoleGameEngine;
using NAudio.Wave;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

class Player
{
    private readonly ConsoleEngine engine;
    public Point playerOnePosition;
    public Point playerTwoPosition;

    public ConcurrentBag<Point> playerOneBullets = new ConcurrentBag<Point>();
    public int playerOneBulletColor = 10;

    public ConcurrentBag<Point> playerTwoBullets = new ConcurrentBag<Point>();
    public int playerTwoBulletColor = 20;

    private Point current;
    private Point current1;
    private Point current2;

    public Point[] playerOne = { new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0), new Point(4, 0), new Point(5, 0), new Point(6, 0),
                                 /*           */  new Point(1, 1), new Point(2, 1), new Point(3, 1), new Point(4, 1), new Point(5, 1),
                                 /*           */  new Point(1, 2), new Point(2, 2), new Point(3, 2), new Point(4, 2), new Point(5, 2),
                                 /*                             */ new Point(2, 3), new Point(3, 3), new Point(4, 3),
                                 /*                             */ new Point(2, 4), new Point(3, 4), new Point(4, 4),
                                 /*                                              */ new Point(3, 5),
                                };
    public Point[] playerTwo = { new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0), new Point(4, 0), new Point(5, 0), new Point(6, 0),
                                 /*           */  new Point(1, 1), new Point(2, 1), new Point(3, 1), new Point(4, 1), new Point(5, 1),
                                 /*           */  new Point(1, 2), new Point(2, 2), new Point(3, 2), new Point(4, 2), new Point(5, 2),
                                 /*                             */ new Point(2, 3), new Point(3, 3), new Point(4, 3),
                                 /*                             */ new Point(2, 4), new Point(3, 4), new Point(4, 4),
                                 /*                                              */ new Point(3, 5),
                                };

    bool isOnePlayer;
    bool isTwoPlayer;

    public Player(ConsoleEngine engine, Point initialPosition, bool isSinglePlayer)
    {
        this.engine = engine;
        if (isSinglePlayer)
        {
            isOnePlayer = true;
            playerOnePosition = initialPosition;
        }
        else
        {
            isOnePlayer = true;
            isTwoPlayer = true;
            playerOnePosition = initialPosition;
            playerTwoPosition = new Point(initialPosition.X + 10, initialPosition.Y); // offset for player two
        }
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

    private int attackCooldownFramesOne = 30;
    private int attackTimeOne = 0;
    private bool attackPressedOne = false;

    private int attackCooldownFramesTwo = 30;
    private int attackTimeTwo = 0;
    private bool attackPressedTwo = false;

    public void Update()
    {
        if (isOnePlayer)
        {
            HandlePlayerMovement(ref playerOnePosition, VK_W, VK_S, VK_A, VK_D);

            if ((GetAsyncKeyState(VK_SPACE) & 0x8000) != 0 && CanAttack(ref attackTimeOne, attackCooldownFramesOne, ref attackPressedOne))
            {
                Point newBullet = new Point(playerOnePosition.X + 3, playerOnePosition.Y);
                playerOneBullets.Add(newBullet);
                PlaySound("C:\\Users\\Styx\\Desktop\\ITEC102FinalMain\\_Game_Main\\pew-pew-two-102442.mp3");
            }

            UpdateBullets(ref playerOneBullets);
            playerOnePosition = ConstrainPosition(playerOnePosition);
        }

        if (isTwoPlayer)
        {
            HandlePlayerMovement(ref playerTwoPosition, VK_UP, VK_DOWN, VK_LEFT, VK_RIGHT);
            if ((GetAsyncKeyState(VK_RETURN) & 0x8000) != 0 && CanAttack(ref attackTimeTwo, attackCooldownFramesTwo, ref attackPressedTwo))
            {
                Point newBullet = new Point(playerTwoPosition.X + 3, playerTwoPosition.Y);
                playerTwoBullets.Add(newBullet);
                PlaySound("C:\\Users\\Styx\\Desktop\\ITEC102FinalMain\\_Game_Main\\pew-pew-two-102442.mp3");
            }

            UpdateBullets(ref playerTwoBullets);
            playerTwoPosition = ConstrainPosition(playerTwoPosition);
        }

        Render();
    }

    private void HandlePlayerMovement(ref Point position, int upKey, int downKey, int leftKey, int rightKey)
    {
        if ((GetAsyncKeyState(upKey) & 0x8000) != 0) position.Y--;
        if ((GetAsyncKeyState(downKey) & 0x8000) != 0) position.Y++;
        if ((GetAsyncKeyState(leftKey) & 0x8000) != 0) position.X--;
        if ((GetAsyncKeyState(rightKey) & 0x8000) != 0) position.X++;
    }

    private bool CanAttack(ref int attackTime, int cooldownFrames, ref bool attackPressed)
    {
        if (attackTime == 0)
        {
            attackTime = cooldownFrames;
            attackPressed = true;
            return true;
        }
        attackPressed = false;
        if (attackTime > 0) attackTime--;
        return false;
    }

    private void UpdateBullets(ref ConcurrentBag<Point> bullets)
    {
        var updatedBullets = new ConcurrentBag<Point>();
        foreach (var bullet in bullets)
        {
            Point newBullet = new Point(bullet.X, bullet.Y - 1);
            if (newBullet.Y > 0) updatedBullets.Add(newBullet);
        }
        bullets = updatedBullets;
    }

    private Point ConstrainPosition(Point position)
    {
        position.X = Math.Max(0, Math.Min(position.X, Console.WindowWidth - 1));
        position.Y = Math.Max(0, Math.Min(position.Y, Console.WindowHeight - 1));
        return position;
    }

    public void Render()
    {
        engine.ClearBuffer();
        if (isOnePlayer)
        {
            try
            {
                engine.WriteText(new Point(40, 0), playerOnePosition.ToString(), 1);
                Parallel.ForEach(playerOne, item =>
                {
                    Point playerOneHitBox = new Point(item.X + playerOnePosition.X, item.Y + playerOnePosition.Y);
                    engine.SetPixel(playerOneHitBox, 255, ConsoleCharacter.Full);
                });

                Parallel.ForEach(playerOneBullets, bullet =>
                {
                    engine.SetPixel(bullet, playerOneBulletColor, ConsoleCharacter.Full);
                    engine.SetPixel(new Point(bullet.X - 1, bullet.Y), playerOneBulletColor, ConsoleCharacter.Full);
                    engine.SetPixel(new Point(bullet.X + 1, bullet.Y), playerOneBulletColor, ConsoleCharacter.Full);
                    engine.SetPixel(new Point(bullet.X, bullet.Y - 1), playerOneBulletColor, ConsoleCharacter.Full);
                });
            }
            catch (System.InvalidOperationException) { }
        }

        if (isTwoPlayer)
        {
            try
            {
                engine.WriteText(new Point(40, 1), playerTwoPosition.ToString(), 1);
                Parallel.ForEach(playerTwo, item =>
                {
                    Point playerTwoHitBox = new Point(item.X + playerTwoPosition.X, item.Y + playerTwoPosition.Y);
                    engine.SetPixel(playerTwoHitBox, 255, ConsoleCharacter.Full);
                });

                Parallel.ForEach(playerTwoBullets, bullet =>
                {
                    engine.SetPixel(bullet, playerTwoBulletColor, ConsoleCharacter.Full);
                    engine.SetPixel(new Point(bullet.X - 1, bullet.Y), playerTwoBulletColor, ConsoleCharacter.Full);
                    engine.SetPixel(new Point(bullet.X + 1, bullet.Y), playerTwoBulletColor, ConsoleCharacter.Full);
                    engine.SetPixel(new Point(bullet.X, bullet.Y - 1), playerTwoBulletColor, ConsoleCharacter.Full);
                });
            }
            catch (System.InvalidOperationException) { }
        }

        engine.DisplayBuffer();
    }

    public void PlaySound(string filePath)
    {
        try
        {
            using (var audioFile = new AudioFileReader(filePath))
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
        catch (AccessViolationException) { }
        catch (COMException) { }
        catch (System.IO.FileNotFoundException) { }
    }

    private Point RotatePoint(Point point, Point center, float angle)
    {
        float cosAngle = (float)Math.Cos(angle * Math.PI / 180);
        float sinAngle = (float)Math.Sin(angle * Math.PI / 180);
        int x = (int)((point.X - center.X) * cosAngle - (point.Y - center.Y) * sinAngle + center.X);
        int y = (int)((point.X - center.X) * sinAngle + (point.Y - center.Y) * cosAngle + center.Y);
        return new Point(x, y);
    }
}
