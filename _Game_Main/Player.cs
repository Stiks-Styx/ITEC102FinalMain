using System;
using ConsoleGameEngine;

class Player
{
    private readonly ConsoleEngine engine;
    public Point playerOnePosition;
    public Point playerTwoPosition;
    private Point current;
    private Point current1;
    private Point current2;

    public Player(ConsoleEngine engine)
    {
        this.engine = engine;
        playerOnePosition = new Point(10, 10);
        playerTwoPosition = new Point(20, 10); // Changed initial position to differentiate players
        current = new Point(0, 0);
        current1 = new Point(3, 5);
        current2 = new Point(6, 0);
    }

    public void Update()
    {
        // Player 1
        if (engine.GetKeyDown(ConsoleKey.W))
        {
            playerOnePosition.Y--;
        }
        if (engine.GetKeyDown(ConsoleKey.S))
        {
            playerOnePosition.Y++;
        }
        if (engine.GetKeyDown(ConsoleKey.D))
        {
            playerOnePosition.X++;
        }
        if (engine.GetKeyDown(ConsoleKey.A))
        {
            playerOnePosition.X--;
        }

        // Player 2
        if (engine.GetKeyDown(ConsoleKey.UpArrow))
        {
            playerTwoPosition.Y--;
        }
        if (engine.GetKeyDown(ConsoleKey.DownArrow))
        {
            playerTwoPosition.Y++;
        }
        if (engine.GetKeyDown(ConsoleKey.RightArrow))
        {
            playerTwoPosition.X++;
        }
        if (engine.GetKeyDown(ConsoleKey.LeftArrow))
        {
            playerTwoPosition.X--;
        }
        Render();
    }

    public void Render()
    {
        engine.ClearBuffer();
        engine.WriteText(new Point(40, 0), Convert.ToString(playerOnePosition), 1);
        engine.Triangle(current+playerOnePosition, current2+playerOnePosition, current1 + playerOnePosition, 255, ConsoleCharacter.Full);
        engine.WriteText(new Point(40, 1), Convert.ToString(playerTwoPosition), 1);
        engine.Triangle(current + playerTwoPosition, current2+ playerTwoPosition, current1 + playerTwoPosition, 200, ConsoleCharacter.Full);
        engine.DisplayBuffer();
    }

    private Point RotatePoint(Point point, Point center, float angle)
    {
        float cosAngle = (float)Math.Cos(angle * Math.PI / 180);
        float sinAngle = (float)Math.Sin(angle * Math.PI / 180);

        int x = (int)((point.X - center.X) * cosAngle - (point.Y - center.Y) * sinAngle + center.X);
        int y = (int)((point.X - center.X) * sinAngle + (point.Y - center.Y) * cosAngle + center.Y);

        return new Point(x, y);
    }
}
