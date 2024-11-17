using ConsoleGameEngine;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;

class Player : IDisposable
{
    private EnemySpawner enemySpawner;
    private readonly ConsoleEngine engine;
    private DirectInput directInput;
    private Keyboard keyboard;
    private KeyboardState keyboardState;

    private int borderColor = 1;
    private int screenWidth = 400;
    private int screenHeight = 100;

    // // //
    public int playerOneColor = 4;
    public int playerTwoColor = 3;
    // // //

    public Point playerOnePosition;
    public List<Point> playerOneBullets = new List<Point>();
    public int playerOneLife = 5; 

    public Point playerTwoPosition;
    public List<Point> playerTwoBullets = new List<Point>();
    public int playerTwoLife = 5; 

    public bool isOnePlayer;
    public bool isTwoPlayer;

    public Point[] playerOne = {
        new Point(0,0),
        new Point(0,1), new Point(1,1), new Point(2,1),
        new Point(0,2), new Point(1,2), new Point(2,2), new Point(3,2),
        new Point(0,3), new Point(1,3), new Point(2,3), new Point(3,3), new Point(4,3),
        new Point(0,4), new Point(1,4), new Point(2,4), new Point(3,4),
        new Point(0,5), new Point(1,5), new Point(2,5),
        new Point(0,6)
    };

    public Point[] playerTwo = {
        new Point(0,0),
        new Point(0,1), new Point(1,1), new Point(2,1),
        new Point(0,2), new Point(1,2), new Point(2,2), new Point(3,2),
        new Point(0,3), new Point(1,3), new Point(2,3), new Point(3,3), new Point(4,3),
        new Point(0,4), new Point(1,4), new Point(2,4), new Point(3,4),
        new Point(0,5), new Point(1,5), new Point(2,5),
        new Point(0,6)
    };


    private int attackCooldownFramesOne = 30;
    private int attackTimeOne = 0;
    private bool attackPressedOne = false;

    private int attackCooldownFramesTwo = 30;
    private int attackTimeTwo = 0;
    private bool attackPressedTwo = false;

    public Player(ConsoleEngine engine, Point initialPosition, bool isSinglePlayer)
    {
        this.engine = engine;

        directInput = new DirectInput();
        keyboard = new Keyboard(directInput);
        keyboard.Acquire();

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
            playerTwoPosition = new Point(initialPosition.X + 10, initialPosition.Y); // Offset for player two
        }
    }

    public void Update()
    {
        keyboardState = keyboard.GetCurrentState();
        if (keyboardState == null)
            return;

        // Player One Controls
        if (keyboardState.IsPressed(Key.W) && playerOnePosition.Y > 1) playerOnePosition.Y--;
        if (keyboardState.IsPressed(Key.S) && playerOnePosition.Y < screenHeight - 5) playerOnePosition.Y++; // Adjusted for player height
        if (keyboardState.IsPressed(Key.A) && playerOnePosition.X > 1) playerOnePosition.X--;
        if (keyboardState.IsPressed(Key.D) && playerOnePosition.X < screenWidth - 5) playerOnePosition.X++; // Adjusted for player width

        // Player One Shooting
        if (keyboardState.IsPressed(Key.Space) && CanAttack(ref attackTimeOne, attackCooldownFramesOne, ref attackPressedOne))
        {
            Point newBullet = new Point(playerOnePosition.X, playerOnePosition.Y + 3);
            playerOneBullets.Add(newBullet);
        }

        // Player Two Controls (if two players are enabled)
        if (isTwoPlayer)
        {
            if (keyboardState.IsPressed(Key.Up) && playerTwoPosition.Y > 1) playerTwoPosition.Y--;
            if (keyboardState.IsPressed(Key.Down) && playerTwoPosition.Y < screenHeight - 7) playerTwoPosition.Y++;
            if (keyboardState.IsPressed(Key.Left) && playerTwoPosition.X > 1) playerTwoPosition.X--;
            if (keyboardState.IsPressed(Key.Right) && playerTwoPosition.X < screenWidth - 7) playerTwoPosition.X++;

            // Player Two Shooting
            if (keyboardState.IsPressed(Key.RightControl) && CanAttack(ref attackTimeTwo, attackCooldownFramesTwo, ref attackPressedTwo))
            {
                Point newBullet = new Point(playerTwoPosition.X, playerTwoPosition.Y + 3);
                playerTwoBullets.Add(newBullet);
            }
        }

        UpdateBullets(playerOneBullets);
        UpdateBullets(playerTwoBullets);
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
            Point newBullet = new Point(bullets[i].X + 1, bullets[i].Y);

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

    public void Render()
    {
        engine.ClearBuffer();
        RenderBorder();
        RenderPlayer(playerOne, playerOnePosition, playerOneColor); // for Player One
        RenderPlayer(playerTwo, playerTwoPosition, playerTwoColor); // for Player Two
        RenderBullets(playerOneBullets, playerOneColor); // for Player One Bullets
        RenderBullets(playerTwoBullets, playerTwoColor); // for Player Two Bullets
        engine.DisplayBuffer();
    }

    public void RenderPlayer(Point[] player, Point position, int color)
    {
        List<Point> playerHitbox = new List<Point>();

        // Add the player's segments to the hitbox
        foreach (var item in player)
        {
            Point playerHitBox = new Point(item.X + position.X, item.Y + position.Y);
            playerHitbox.Add(playerHitBox); // Add each calculated segment to the hitbox list
            engine.SetPixel(playerHitBox, color, ConsoleCharacter.Full); // Render the segment on the screen
        }
    }



    private void RenderBullets(List<Point> bullets, int color)
    {
        foreach (var bullet in bullets)
        {
            engine.SetPixel(bullet, color, ConsoleCharacter.Full);
        }
    }

    private void RenderBorder()
    {

        // Render top and bottom borders
        for (int x = 0; x < screenWidth; x++)
        {
            engine.SetPixel(new Point(x, 0), borderColor, ConsoleCharacter.Full);           // Top border
            engine.SetPixel(new Point(x, screenHeight - 1), borderColor, ConsoleCharacter.Full);   // Bottom border
        }

        // Render left and right borders
        for (int y = 0; y < screenHeight; y++)
        {
            engine.SetPixel(new Point(0, y), borderColor, ConsoleCharacter.Full);            // Left border
            engine.SetPixel(new Point(screenWidth - 1, y), borderColor, ConsoleCharacter.Full);    // Right border
        }
    }

    public void Dispose()
    {
        keyboard.Unacquire();
        keyboard.Dispose();
        directInput.Dispose();
    }
}
