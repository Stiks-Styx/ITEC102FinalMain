using ConsoleGameEngine;
using NAudio.Wave;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

class Player
{
    private readonly ConsoleEngine engine;
    public Point playerOnePosition;
    public Point playerTwoPosition;

    public List<Point> playerOneBullets = new List<Point>();
    public int playerOneBulletColor = 10;

    public List<Point> playerTwoBullets = new List<Point>();
    public int playerTwoBulletColor = 20;

    private Point current;
    private Point current1;
    private Point current2;

    public Point[] playerOne = { 
new Point(0,0),
new Point(0,1),new Point(1,1),new Point(2,1),
new Point(0,2),new Point(1,2),new Point(2,2),new Point(3,2),
new Point(0,3),new Point(1,3),new Point(2,3),new Point(3,3),new Point(4,3),
new Point(0,4),new Point(1,4),new Point(2,4),new Point(3,4),
new Point(0,5),new Point(1,5),new Point(2,5),
new Point(0,6)
                                };
    public Point[] playerTwo = { 
new Point(0,0),
new Point(0,1),new Point(1,1),
new Point(0,2),new Point(1,2),new Point(2,2),
new Point(0,3),new Point(1,3),new Point(2,3),new Point(3,3),
new Point(0,4),new Point(1,4),new Point(2,4),
new Point(0,5),new Point(1,5),
new Point(0,6)
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
        current1 = new Point(5, 3);
        current2 = new Point(0, 6);
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
                Point newBullet = new Point(playerOnePosition.X, playerOnePosition.Y + 3);
                playerOneBullets.Add(newBullet);
            }

            UpdateBullets(playerOneBullets);
            playerOnePosition = ConstrainPosition(playerOnePosition);
        }

        if (isTwoPlayer)
        {
            HandlePlayerMovement(ref playerTwoPosition, VK_UP, VK_DOWN, VK_LEFT, VK_RIGHT);
            if ((GetAsyncKeyState(VK_RETURN) & 0x8000) != 0 && CanAttack(ref attackTimeTwo, attackCooldownFramesTwo, ref attackPressedTwo))
            {
                Point newBullet = new Point(playerTwoPosition.X, playerTwoPosition.Y + 3);
                playerTwoBullets.Add(newBullet);
            }

            UpdateBullets(playerTwoBullets);
            playerTwoPosition = ConstrainPosition(playerTwoPosition);
        }

    }

    private void HandlePlayerMovement(ref Point position, int upKey, int downKey, int leftKey, int rightKey)
    {
        if ((GetAsyncKeyState(upKey) & 0x8000) != 0) position.Y--;
        if ((GetAsyncKeyState(downKey) & 0x8000) != 0) position.Y++;
        if ((GetAsyncKeyState(leftKey) & 0x8000) != 0) position.X--;
        if ((GetAsyncKeyState(rightKey) & 0x8000) != 0) position.X++;
        position = ConstrainPosition(position);
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

    private void UpdateBullets(List<Point> bullets)
    {
        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            // Move bullet upward by 1 step (or as per your logic)
            Point newBullet = new Point(bullets[i].X+1, bullets[i].Y);

            // Check if the bullet is still within the screen bounds
            if (newBullet.Y >= 0)
            {
                bullets[i] = newBullet;
            }
            else
            {
                bullets.RemoveAt(i); // Remove bullet if it goes off-screen
            }
        }
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
            engine.WriteText(new Point(40, 0), playerOnePosition.ToString(), 1);
            RenderPlayer(playerOne, playerOnePosition, 4);
            RenderBullets(playerOneBullets, playerOneBulletColor);
        }

        if (isTwoPlayer)
        {
            engine.WriteText(new Point(40, 1), playerTwoPosition.ToString(), 1);
            RenderPlayer(playerTwo, playerTwoPosition, 2);
            RenderBullets(playerTwoBullets, playerTwoBulletColor);
        }

        engine.DisplayBuffer();
    }

    private void RenderPlayer(Point[] player, Point position, int color)
    {
        foreach (var item in player)
        {
            Point playerHitBox = new Point(item.X + position.X, item.Y + position.Y);
            engine.SetPixel(playerHitBox, color, ConsoleCharacter.Full);
        }
    }

    private void RenderBullets(List<Point> bullets, int color)
    {
        try
        {
            foreach (var bullet in bullets)
            {
                engine.SetPixel(bullet, color, ConsoleCharacter.Full);
                // Add only the necessary pixels to limit render load
                engine.SetPixel(new Point(bullet.X+1, bullet.Y), color, ConsoleCharacter.Full);
            }
        }
        catch (System.InvalidOperationException) { }
    }
}
