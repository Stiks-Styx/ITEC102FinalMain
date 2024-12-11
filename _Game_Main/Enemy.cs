using ConsoleGameEngine;
using _Game_Main;

public class Enemy
{
    private readonly ConsoleEngine engine;
    public Point Position { get; private set; }
    public int Type { get; private set; } // 1 for normal, 2 for split
    public bool IsActive { get; set; } = true;

    private static Random random = new Random();
    private static int spawnCooldown = 100; // Cooldown in frames
    private static int currentCooldown = 0;

    public Enemy(ConsoleEngine engine, Point initialPosition, int type)
    {
        this.engine = engine;
        this.Position = initialPosition;
        this.Type = type;
    }

    public void Update()
    {
        if (!IsActive) return;

        // Move the enemy from right to left
        Position = new Point(Position.X - 1, Position.Y);

        // Deactivate if it goes off-screen
        if (Position.X < 0)
        {
            IsActive = false;
        }
    }

    public void Render()
    {
        if (!IsActive) return;

        int width = Type == 1 ? 6 : 12;
        int height = Type == 1 ? 4 : 8;
        int color = Type == 1 ? 2 : 3; // Different colors for different types

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                engine.SetPixel(new Point(Position.X + x, Position.Y + y), color, ConsoleCharacter.Full);
            }
        }
    }

    public void OnHit(List<Enemy> enemies)
    {
        if (Type == 2)
        {
            // Split into two smaller enemies
            enemies.Add(new Enemy(engine, new Point(Position.X, Position.Y - 2), 1));
            enemies.Add(new Enemy(engine, new Point(Position.X, Position.Y + 5), 1));
        }
        IsActive = false;
    }

    public List<Point> GetEnemyPoints()
    {
        List<Point> enemyPoints = new List<Point>();
        int width = Type == 1 ? 6 : 12;
        int height = Type == 1 ? 4 : 8;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                enemyPoints.Add(new Point(Position.X + x, Position.Y + y));
            }
        }

        return enemyPoints;
    }

    public static void ManageEnemies(ConsoleEngine engine, List<Enemy> enemies, int screenWidth, int screenHeight)
    {
        // Update existing enemies
        foreach (var enemy in enemies)
        {
            enemy.Update();
        }

        // Remove inactive enemies
        enemies.RemoveAll(e => !e.IsActive);

        // Handle enemy spawning
        if (currentCooldown <= 0)
        {
            int type = random.Next(1, 3); // Randomly choose between type 1 and type 2
            int yPosition = random.Next(1, screenHeight - (type == 1 ? 3 : 6)); // Ensure enemy spawns within vertical bounds
            enemies.Add(new Enemy(engine, new Point(screenWidth - 1, yPosition), type));
            currentCooldown = spawnCooldown;
        }
        else
        {
            currentCooldown--;
        }
    }
}

