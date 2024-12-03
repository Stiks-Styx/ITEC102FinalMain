using _Game_Main;
using ConsoleGameEngine;
using SharpDX.DirectInput;

class Player : IDisposable
{
    private readonly ConsoleEngine engine;
    private readonly Program program;
    private readonly BorderRenderer borderRenderer;
    private DirectInput directInput;
    private Keyboard keyboard;
    private KeyboardState keyboardState;

    private readonly int screenWidth;
    private readonly int screenHeight;

    // // //
    public int playerOneColor = 4;
    // // //

    public Point playerOnePosition;
    public List<Point> playerOneBullets = new List<Point>();
    public int playerOneLife = 5;
    public bool isAlive;

    public bool isOnePlayer = true;

    public int score = 0;

    public Point[] playerOne = {
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

    public Player(ConsoleEngine engine, Point initialPosition, int screenWidth, int screenHeight, BorderRenderer borderRenderer,Program program)
    {
        this.engine = engine;
        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;
        this.program = program;
        this.borderRenderer = borderRenderer;

        directInput = new DirectInput();
        keyboard = new Keyboard(directInput);
        keyboard.Acquire();

        isOnePlayer = true;
        playerOnePosition = initialPosition;
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
        if (keyboardState.IsPressed(Key.D) && playerOnePosition.X < screenWidth - 65) playerOnePosition.X++; // Adjusted for player width

        // Player One Shooting
        if (keyboardState.IsPressed(Key.Space) && CanAttack(ref attackTimeOne, attackCooldownFramesOne, ref attackPressedOne))
        {
            Point newBullet = new Point(playerOnePosition.X, playerOnePosition.Y + 3);
            playerOneBullets.Add(newBullet);
            score++;
        }

        if (keyboardState.IsPressed(Key.B))
        {
            LoseLife();
        }

        UpdateBullets(playerOneBullets);

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

            // Check if the bullet is within the screen's horizontal bounds.
            if (newBullet.X < screenWidth - 60)
            {
                bullets[i] = newBullet; // Update the bullet's position.
            }
            else
            {
                bullets.RemoveAt(i); // Remove the bullet if it goes off-screen.
            }
        }
    }

    public void Render()
    {
        engine.ClearBuffer();

        borderRenderer.RenderBorder();

        RenderPlayer(playerOne, playerOnePosition, playerOneColor); // for Player One
        RenderBullets(playerOneBullets, playerOneColor); // for Player One Bullets

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

    public void Dispose()
    {
        keyboard.Unacquire();
        keyboard.Dispose();
        directInput.Dispose();
    }

    private void LoseLife()
    {
        playerOneLife--;
        if (playerOneLife == 0)
        {
            program.RecordScore();
        }
    }
}