using ConsoleGameEngine;

class Enemy
{
    private ConsoleEngine engine;
    public Point position;
    public int color = 14; // Color for the enemy

    // Enemy shape (simple representation)
    public Point[] shape = {
        new Point(0, 0),
        new Point(1, 0),
        new Point(0, 1),
        new Point(1, 1)
    };

    public Enemy(ConsoleEngine engine, Point spawnPosition)
    {
        this.engine = engine;
        this.position = spawnPosition;
    }

    public void Update()
    {
        // Move left by 1 unit each frame
        position.X--;

        // Remove the enemy if it goes off-screen (to the left)
        if (position.X < 0)
        {
            position = new Point(-1, -1); // Set to an invalid position (off-screen)
        }
    }

    public void Render()
    {
        engine.ClearBuffer();
        // Render each part of the enemy's shape
        foreach (var point in shape)
        {
            Point enemyPosition = new Point(point.X + position.X, point.Y + position.Y);
            engine.SetPixel(enemyPosition, color, ConsoleCharacter.Full);
        }
    }
}
