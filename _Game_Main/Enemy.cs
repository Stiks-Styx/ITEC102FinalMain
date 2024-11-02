using ConsoleGameEngine;

class _Enemy
{
    protected readonly ConsoleEngine engine;
    public int EnemyHealth { get; protected set; }
    public Point Position { get; protected set; }

    public _Enemy(ConsoleEngine engine, Point initialPosition, int initialHealth)
    {
        this.engine = engine;
        Position = initialPosition;
        EnemyHealth = initialHealth;
    }

    public virtual void EnemyMovement()
    {
        // Basic movement logic (e.g., moving downward each frame)
        Position = new Point(Position.X, Position.Y + 1); // Example: move down
    }

    public virtual void EnemyAttack()
    {
        // Basic attack logic (could spawn bullets, reduce player health, etc.)
        // Implement different attack styles in derived classes
    }

    public virtual void EnemyRender()
    {
        // Basic rendering, which can be overridden in derived classes
        engine.SetPixel(Position, 12, ConsoleCharacter.Full); // Example color for enemy
    }
}

// Small enemy class
class Small : _Enemy
{
    public Small(ConsoleEngine engine, Point initialPosition)
        : base(engine, initialPosition, 1) // Small enemy has 1 health
    {
    }

    public override void EnemyRender()
    {
        engine.SetPixel(Position, 10, ConsoleCharacter.Full); // Different color or style for Small enemy
    }
}

// Medium enemy class
class Medium : _Enemy
{
    public Medium(ConsoleEngine engine, Point initialPosition)
        : base(engine, initialPosition, 2) // Medium enemy has 2 health
    {
    }

    public override void EnemyRender()
    {
        engine.SetPixel(Position, 11, ConsoleCharacter.Full); // Different color or style for Medium enemy
    }
}

// Large enemy class
class Large : _Enemy
{
    public Large(ConsoleEngine engine, Point initialPosition)
        : base(engine, initialPosition, 3) // Large enemy has 3 health
    {
    }

    public override void EnemyRender()
    {
        engine.SetPixel(Position, 12, ConsoleCharacter.Full); // Different color or style for Large enemy
    }
}

// BossOne class
class BossOne : _Enemy
{
    public BossOne(ConsoleEngine engine, Point initialPosition)
        : base(engine, initialPosition, 10) // BossOne has 10 health
    {
    }

    public override void EnemyRender()
    {
        engine.SetPixel(Position, 13, ConsoleCharacter.Full); // Different color or style for BossOne
    }
}

// BossTwo class
class BossTwo : _Enemy
{
    public BossTwo(ConsoleEngine engine, Point initialPosition)
        : base(engine, initialPosition, 20) // BossTwo has 20 health
    {
    }

    public override void EnemyRender()
    {
        engine.SetPixel(Position, 14, ConsoleCharacter.Full); // Different color or style for BossTwo
    }
}

// BossThree class
class BossThree : _Enemy
{
    public BossThree(ConsoleEngine engine, Point initialPosition)
        : base(engine, initialPosition, 30) // BossThree has 30 health
    {
    }

    public override void EnemyRender()
    {
        engine.SetPixel(Position, 15, ConsoleCharacter.Full); // Different color or style for BossThree
    }
}
