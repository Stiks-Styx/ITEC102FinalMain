
using _Game_Main;
using ConsoleGameEngine;

class BorderRenderer
{
    private readonly ConsoleEngine engine;
    private readonly Program program;
    private readonly int screenWidth, screenHeight;
    private int borderColor = 1;

    public BorderRenderer(ConsoleEngine engine, int screenWidth, int screenHeight, Program program)
    {
        this.engine = engine;
        this.program = program;
        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;
    }

    public void RenderBorder()
    {
        for (int x = 0; x < screenWidth; x++)
        {
            engine.SetPixel(new Point(x, 0), borderColor, ConsoleCharacter.Full);
            engine.SetPixel(new Point(x, screenHeight - 1), borderColor, ConsoleCharacter.Full);
        }

        // Render left and right borders
        for (int y = 0; y < screenHeight; y++)
        {
            engine.SetPixel(new Point(0, y), borderColor, ConsoleCharacter.Full);
            engine.SetPixel(new Point(screenWidth - 1, y), borderColor, ConsoleCharacter.Full);
        }

        for (int z = 0; z < screenHeight; z++)
        {
            engine.SetPixel(new Point(screenWidth - 60, screenHeight - z), borderColor, ConsoleCharacter.Full);
        }
    }
}
