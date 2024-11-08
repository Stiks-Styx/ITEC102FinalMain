using ConsoleGameEngine;

public class EnemySpawner
{
    private ConsoleEngine engine;
    private List<Point> enemies;
    private int spawnSpeed; // Control spawn rate
    private float enemySpeed; // Control enemy speed (can now be fractional)
    private Random random;

    // Variable to accumulate movement for fractional speeds
    private float enemyMovementAccumulator = 0;

    public EnemySpawner(ConsoleEngine engine)
    {
        this.engine = engine;
        enemies = new List<Point>();
        random = new Random();
        spawnSpeed = 5; // Adjust spawn rate if necessary
        enemySpeed = 1.0f; // Default movement speed of enemies (can be fractional now)
    }

    public void SpawnEnemy()
    {
        // Spawn an enemy at the right edge of the screen with a random Y position
        Point enemy = new Point(Console.WindowWidth - 1, random.Next(5, 85)); // Random Y-position
        enemies.Add(enemy);
    }

    public void UpdateEnemies(List<Point> playerBullets)
    {
        // Create a list to store the enemies and bullets that need to be removed
        List<int> enemiesToRemove = new List<int>();
        List<int> bulletsToRemove = new List<int>();

        // First, check for collisions and mark enemies for removal
        for (int i = 0; i < enemies.Count; i++)
        {
            Point enemy = enemies[i];

            // Check if the enemy is hit by any bullet
            for (int j = 0; j < playerBullets.Count; j++)
            {
                Point bullet = playerBullets[j];

                // Check for collision (bullet and enemy are at the same coordinates)
                if (bullet.X >= enemy.X && bullet.X < enemy.X + 5 && bullet.Y >= enemy.Y && bullet.Y < enemy.Y + 5)
                {
                    // Mark enemy for removal and bullet for removal
                    enemiesToRemove.Add(i);
                    bulletsToRemove.Add(j);
                    break; // Exit loop after enemy is marked for removal
                }
            }

            // Accumulate fractional movement for the enemy
            enemyMovementAccumulator += enemySpeed;

            // Move the enemy left by 1 unit once the accumulator reaches 1
            if (enemyMovementAccumulator >= 1.0f)
            {
                // Move the enemy left by 1 unit
                Point newEnemyPosition = new Point(enemy.X - 1, enemy.Y);

                // If the enemy moved off the left side of the screen, mark it for removal
                if (newEnemyPosition.X < 0)
                {
                    enemiesToRemove.Add(i);
                }
                else
                {
                    enemies[i] = newEnemyPosition; // Update position of the enemy
                }

                // Subtract the full unit from the accumulator
                enemyMovementAccumulator -= 1.0f;
            }
        }

        // Now, remove all marked enemies and bullets in one go after the iteration
        foreach (int index in enemiesToRemove.OrderByDescending(i => i))
        {
            try
            {
                enemies.RemoveAt(index); // Remove enemies in reverse order to avoid index shifting
            }
            catch (System.ArgumentOutOfRangeException) { }
        }

        foreach (int index in bulletsToRemove.OrderByDescending(i => i))
        {
            try
            {
                playerBullets.RemoveAt(index); // Remove bullets in reverse order to avoid index shifting
            }
            catch (System.ArgumentOutOfRangeException) { }
        }
    }

    public void RenderEnemies()
    {
        try
        {
            foreach (var enemy in enemies)
            {
                for (int dx = 0; dx < 5; dx++) // Draw 5x5 area for each enemy
                {
                    for (int dy = 0; dy < 5; dy++)
                    {
                        Point enemyPosition = new Point(enemy.X + dx, enemy.Y + dy);
                        engine.SetPixel(enemyPosition, 4, ConsoleCharacter.Medium); // Render enemy
                    }
                }

                // Optional: Display the enemy's current position as text
                engine.WriteText(new Point(40, 2), enemy.ToString(), 1); // Adjust position for display
            }
            engine.DisplayBuffer();
        }
        catch (System.InvalidOperationException) { }
    }

    // Ensure there are always exactly `targetCount` enemies on screen
    public void EnsureEnemiesOnScreen(int targetCount)
    {
        // Remove excess enemies if there are more than `targetCount`
        while (enemies.Count > targetCount)
        {
            enemies.RemoveAt(enemies.Count - 1); // Remove one enemy from the end of the list
        }

        // Add new enemies if there are fewer than `targetCount`
        while (enemies.Count < targetCount)
        {
            SpawnEnemy(); // Spawn a new enemy
        }
    }

    // Set the movement speed of the enemies
    public void SetEnemySpeed(float speed)
    {
        enemySpeed = speed;
    }

    // This method is used to get the current number of enemies
    public int GetCurrentEnemyCount()
    {
        return enemies.Count;
    }
}
