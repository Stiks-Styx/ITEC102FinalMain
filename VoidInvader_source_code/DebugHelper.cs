using ConsoleGameEngine;

class DebugHelper
{
    private readonly ConsoleEngine engine;
    private static FigletFont font1;
    private readonly int screenHeight;
    private readonly string playerName;

    public DebugHelper(ConsoleEngine engine, FigletFont font1, int screenHeight, string playerName)
    {
        this.engine = engine;
        this.screenHeight = screenHeight;
        this.playerName = playerName;

    }

    public void MenuDebugInfo(string? currentPage, Point selectorPosition)
    {
        try
        {
            font1 = FigletFont.Load("C:\\Users\\Styx\\Desktop\\ITEC102FinalMain\\_Game_Main\\smslant.flf");

            // Check if currentPage is null or empty
            if (string.IsNullOrEmpty(currentPage))
            {
                currentPage = "Unknown Page"; // Set a default value
            }

            // Check if selectorPosition is valid
            if (selectorPosition.X == null && selectorPosition.Y == null)
            {
                selectorPosition = new Point(0, 0); // Set a default value
            }

            string debugInfo = $"Page: {currentPage}, Selector Position: {selectorPosition.X}, {selectorPosition.Y}";
            engine.WriteFiglet(new Point(10, 110), debugInfo, font1, 2);
        }
        catch (Exception ex)
        {
            // Log the exception for debugging
            Console.WriteLine("Error in MenuDebugInfo: " + ex.Message);
        }
    }

    public void GameDebugInfo(string? playerName, string? playerScore)
    {
        // Null check for playerName before rendering
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Unknown Player"; // Default name if playerName is null or empty
        }

        string debugInfo = $"{playerName}";
        engine.WriteFiglet(new Point(350, 10), debugInfo, font1, 2);/*

        int yOffset = 0; // Initialize an offset for the Y-coordinate

        foreach (var part in playerOne)
        {

            var absolutePosition = new Point(part.X + playerOnePosition.X, part.Y + playerOnePosition.Y);

            // Write the text to the engine, adjusting the Y-position for each body part
            engine.WriteFiglet(new Point(0, 0 + yOffset), $"{absolutePosition.X},{absolutePosition.Y}", font1, 2);

            yOffset += 5; // Move down by 1 row for the next body part
        }*/
    }
}
