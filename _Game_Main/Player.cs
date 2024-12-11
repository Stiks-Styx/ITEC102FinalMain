using _Game_Main;
using ConsoleGameEngine;
using SharpDX.DirectInput;

class Player : IDisposable
{
    private readonly ConsoleEngine engine;
    private readonly Program program;
    private readonly BorderRenderer borderRenderer;
    private readonly CollisionDetector collision;
    private DirectInput directInput;
    private Keyboard keyboard;
    private KeyboardState keyboardState;

    private readonly int screenWidth;
    private readonly int screenHeight;

    public int playerOneColor = 4;
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

    public Player(ConsoleEngine engine, Point initialPosition, int screenWidth, int screenHeight, BorderRenderer borderRenderer, Program program, CollisionDetector collision)
    {
        this.engine = engine;
        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;
        this.program = program;
        this.borderRenderer = borderRenderer;
        this.collision = collision;

        directInput = new DirectInput();
        keyboard = new Keyboard(directInput);
        keyboard.Acquire();

        isOnePlayer = true;
        playerOnePosition = initialPosition;
    }

    public void Update(List<Enemy> enemies)
    {
        keyboardState = keyboard.GetCurrentState();
        if (keyboardState == null)
            return;

        if (keyboardState.IsPressed(Key.W) && playerOnePosition.Y > 1) playerOnePosition.Y--;
        if (keyboardState.IsPressed(Key.S) && playerOnePosition.Y < screenHeight - 7) playerOnePosition.Y++;
        if (keyboardState.IsPressed(Key.A) && playerOnePosition.X > 3) playerOnePosition.X--;
        if (keyboardState.IsPressed(Key.D) && playerOnePosition.X < screenWidth - 4) playerOnePosition.X++;

        if (keyboardState.IsPressed(Key.Space) && CanAttack(ref attackTimeOne, attackCooldownFramesOne, ref attackPressedOne))
        {
            playerOneBullets.Add(new Point(playerOnePosition.X, playerOnePosition.Y + 3));
        }

        UpdateBullets(playerOneBullets, enemies);

        foreach (var enemy in enemies)
        {
            if (enemy.IsActive && collision.OnCollision(playerOnePosition, playerOne, enemy.GetEnemyPoints()))
            {
                LoseLife();
                enemy.IsActive = false;
            }
        }
    }

    public void Render(List<Enemy> enemies)
    {
        engine.ClearBuffer();
        RenderPlayer(playerOne, playerOnePosition, playerOneColor);
        RenderBullets(playerOneBullets, playerOneColor);

        foreach (var enemy in enemies)
        {
            if (enemy.IsActive)
            {
                enemy.Render();
            }
        }
        engine.DisplayBuffer();
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

    private void UpdateBullets(List<Point> bullets, List<Enemy> enemies)
    {
        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            Point newBullet = new Point(bullets[i].X + 1, bullets[i].Y);

            if (newBullet.X < screenWidth - 3)
            {
                bullets[i] = newBullet;
            }
            else
            {
                bullets.RemoveAt(i);
                continue;
            }

            foreach (var enemy in enemies)
            {
                if (enemy.IsActive && IsBulletCollidingWithEnemy(newBullet, enemy))
                {
                    enemy.OnHit(enemies);
                    bullets.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public void RenderPlayer(Point[] player, Point position, int color)
    {
        List<Point> playerHitbox = new List<Point>();

        foreach (var item in player)
        {
            Point playerHitBox = new Point(item.X + position.X, item.Y + position.Y);
            playerHitbox.Add(playerHitBox);
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
            }
        }
        catch (InvalidOperationException) { }
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

    private bool IsBulletCollidingWithEnemy(Point bullet, Enemy enemy)
    {
        int width = enemy.Type == 1 ? 6 : 12;
        int height = enemy.Type == 1 ? 4 : 8;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (bullet.Equals(new Point(enemy.Position.X + x, enemy.Position.Y + y)))
                {
                    return true;
                }
            }
        }
        return false;
    }
}