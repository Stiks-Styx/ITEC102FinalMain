using _Game_Main;
using ConsoleGameEngine;
using SharpDX.DirectInput;

class Player : IDisposable
{
    private readonly ConsoleEngine engine;
    private readonly Program program;
    private readonly BorderRenderer borderRenderer;
    private readonly CollisionDetector collision;
    private readonly Enemy enemy;
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
        new Point(0,0), new Point(1,0), new Point(2,0), new Point(3,0), new Point(4,0),
        new Point(5,0), new Point(6,0), new Point(7,0), new Point(8,0), new Point(9,0),
        new Point(10,0), new Point(11,0), new Point(12,0), new Point(13,0), new Point(14,0),
        new Point(15,0), new Point(16,0), new Point(17,0),
        new Point(1, -1), new Point(2, -1), new Point(3, -1), new Point(4, -1),
        new Point(5, -1), new Point(6, -1), new Point(7, -1), new Point(8, -1),
        new Point(9, -1), new Point(10, -1), new Point(11, -1), new Point(12, -1),
        new Point(13, -1), new Point(14, -1), new Point(15, -1),
        new Point(1, 1), new Point(2, 1), new Point(3, 1), new Point(4, 1),
        new Point(5, 1), new Point(6, 1), new Point(7, 1), new Point(8, 1),
        new Point(9, 1), new Point(10, 1), new Point(11, 1), new Point(12, 1),
        new Point(13, 1), new Point(14, 1), new Point(15, 1),
        new Point(2, 2), new Point(3, 2), new Point(4, 2), new Point(5, 2),
        new Point(6, 2), new Point(7, 2), new Point(8, 2), new Point(9, 2),
        new Point(10, 2), new Point(11, 2), new Point(12, 2), new Point(13, 2),
        new Point(14, 2),
        new Point(2, -2), new Point(3, -2), new Point(4, -2), new Point(5, -2),
        new Point(6, -2), new Point(7, -2), new Point(8, -2), new Point(9, -2),
        new Point(10, -2), new Point(11, -2), new Point(12, -2), new Point(13, -2),
        new Point(14, -2),
        new Point(3, 3), new Point(4, 3), new Point(5, 3), new Point(6, 3),
        new Point(7, 3), new Point(8, 3), new Point(9, 3), new Point(10, 3),
        new Point(3, -3), new Point(4, -3), new Point(5, -3), new Point(6, -3),
        new Point(7, -3), new Point(8, -3), new Point(9, -3), new Point(10, -3),
    };

    private int attackCooldownFramesOne = 30;
    private int attackTimeOne = 0;
    private bool attackPressedOne = false;

    private readonly GameDisplay gameDisplay;

    private bool scoreRecorded = false;

    public Player(ConsoleEngine engine, Point initialPosition, int screenWidth, int screenHeight, BorderRenderer borderRenderer, Program program, CollisionDetector collision, MainMenu mainMenu)
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

        gameDisplay = new GameDisplay(engine, this, mainMenu);
    }

    public void Update(List<Enemy> enemies, List<Enemy> lifeCubes)
    {
        keyboardState = keyboard.GetCurrentState();
        if (keyboardState == null)
            return;

        if (keyboardState.IsPressed(Key.W) && playerOnePosition.Y > 21) playerOnePosition.Y -= 2;
        if (keyboardState.IsPressed(Key.S) && playerOnePosition.Y < screenHeight - 6) playerOnePosition.Y += 2;
        if (keyboardState.IsPressed(Key.A) && playerOnePosition.X > 3) playerOnePosition.X -= 2;
        if (keyboardState.IsPressed(Key.D) && playerOnePosition.X < screenWidth - 18) playerOnePosition.X += 2;

        if (keyboardState.IsPressed(Key.Space) && CanAttack(ref attackTimeOne, attackCooldownFramesOne, ref attackPressedOne))
        {
            playerOneBullets.Add(new Point(playerOnePosition.X+ 5, playerOnePosition.Y -2));
            playerOneBullets.Add(new Point(playerOnePosition.X+ 5, playerOnePosition.Y +2));
        }

        UpdateBullets(playerOneBullets, enemies);
        try
        {
            foreach (var enemy in enemies)
            {
                if (enemy.IsActive && collision.OnCollision(playerOnePosition, playerOne, enemy.GetEnemyPoints()))
                {
                    if (enemy.Type == 0)
                    {
                        playerOneLife += enemy.OnCollisionWithPlayer();
                        enemy.IsActive = false;
                    }
                    else
                    {
                        LoseLife();
                        enemy.IsActive = false;
                    }
                }
            }
        }
        catch (Exception) { }

        // Update enemies' PlayerY property to match the player's Y position
        foreach (var enemy in enemies)
        {
            enemy.PlayerY = playerOnePosition.Y; // Set the PlayerY for each enemy
        }

        // Call ManageEnemies with the current score
        Enemy.ManageEnemies(engine, enemies, screenWidth, screenHeight, score);

        CheckLifeCubeCollision(lifeCubes);
    }

    public void Render(List<Enemy> enemies)
    {
        engine.ClearBuffer();

        // Render the border
        borderRenderer.RenderBorder();

        RenderPlayer(playerOne, playerOnePosition, playerOneColor);
        RenderBullets(playerOneBullets, playerOneColor);
        try
        {
            foreach (var enemy in enemies)
            {
                if (enemy.IsActive)
                {
                    enemy.Render();
                }
            }
        }
        catch (System.InvalidOperationException) { }

        gameDisplay.Render();

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
        try
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
                try
                {
                    foreach (var enemy in enemies)
                    {
                        if (enemy.IsActive && IsBulletCollidingWithEnemy(newBullet, enemy))
                        {
                            score += enemy.OnHit(enemies);
                            bullets.RemoveAt(i);
                            break;
                        }
                    }
                }
                catch (System.InvalidOperationException) { }
            }
        }
        catch (System.ArgumentOutOfRangeException) { }
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
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("An error occurred while rendering bullets: " + ex.Message);
        }
    }

    public void Dispose()
    {
        keyboard.Unacquire();
        keyboard.Dispose();
        directInput.Dispose();
    }

    private async Task LoseLife()
    {
        playerOneLife--; // Deduct life when this method is called

        if (playerOneLife <= 0)
        {
            if (!scoreRecorded)
            {
                scoreRecorded = true;
                program.RecordScore();
            }

            // program.pauseRender?.GameOver();

            // Wait for Enter key to return to main menu
            while (!engine.GetKey(ConsoleKey.Enter))
            {
                await Task.Delay(100); // Prevents busy-waiting
            }
        }
    }

    private bool IsBulletCollidingWithEnemy(Point bullet, Enemy enemy)
    {
        int width = enemy.Type == 1 ? 6 : (enemy.Type == 0 ? 3 : 12);
        int height = enemy.Type == 1 ? 4 : (enemy.Type == 0 ? 2 : 8);

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

    private void CheckLifeCubeCollision(List<Enemy> lifeCubes)
    {
        for (int i = lifeCubes.Count - 1; i >= 0; i--)
        {
            if (collision.OnCollision(playerOnePosition, playerOne, lifeCubes[i].GetEnemyPoints()))
            {
                playerOneLife++; // Increase player life
                lifeCubes.RemoveAt(i); // Remove the life cube after collision
            }
        }
    }
}