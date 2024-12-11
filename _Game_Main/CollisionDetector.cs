using ConsoleGameEngine;

public class CollisionDetector
{
    private readonly ConsoleEngine engine;

    public CollisionDetector(ConsoleEngine engine)
    {
        this.engine = engine;
    }

    public bool OnCollision(Point playerPosition, Point[] player, List<Point> enemyPoints)
    {
        foreach (var part in player)
        {
            var playerAbsolutePosition = new Point(part.X + playerPosition.X, part.Y + playerPosition.Y);

            // Check for collision between the player and the enemy
            if (enemyPoints.Any(enemyPart => playerAbsolutePosition.Equals(enemyPart)))
            {
                engine.WriteText(new Point(10, 12), "Hit", 4);  // Display "Hit" when collision occurs
                return true; // Return immediately on collision
            }
        }
        return false; // No collision detected
    }
}
