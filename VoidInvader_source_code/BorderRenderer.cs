using _Game_Main;
using ConsoleGameEngine;

class BorderRenderer
{
    private readonly ConsoleEngine engine;
    private readonly Program program;
    private readonly int screenWidth, screenHeight;
    private int borderColor = 1;
    private string[] borderDesign = { "[", "■", "]" };
    private string borderDesign1 = "[■]";

    public BorderRenderer(ConsoleEngine engine, int screenWidth, int screenHeight, Program program)
    {
        this.engine = engine;
        this.program = program;
        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;
    }

    public void RenderBorder()
    {
        // Render top and bottom borders using the borderDesign array
        for (int x = 0; x < screenWidth; x++)
        {
            string topBottomChar = borderDesign[x % borderDesign.Length]; // Cycle through borderDesign
            engine.WriteText(new Point(x, 0), topBottomChar, borderColor); // Top border
            engine.WriteText(new Point(x, screenHeight - 1), topBottomChar, borderColor); // Bottom border
            engine.WriteText(new Point(x, 16), topBottomChar, borderColor); // Additional bottom border
        }

        // Render left and right borders using the borderDesign array
        for (int y = 0; y < screenHeight; y++)
        {
            engine.WriteText(new Point(0, y), borderDesign1, borderColor); // Left border
            engine.WriteText(new Point(screenWidth - 3, y), borderDesign1, borderColor); // Right border
        }
    }
}
