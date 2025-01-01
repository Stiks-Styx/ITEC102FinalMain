using ConsoleGameEngine;
using System.Threading;

class GameLoading
{
    private readonly ConsoleEngine engine;
    private readonly int screenWidth;
    private readonly int screenHeight;
    private int loadingProgress = 0;
    public bool doneLoading { get; set; } = false;

    public GameLoading(ConsoleEngine engine, int screenWidth, int screenHeight)
    {
        this.engine = engine;
        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;
    }

    public void DisplayLoadingScreen()
    {
        engine.ClearBuffer();
        MainMenu.LoadFonts();
        string loadingText = "Loading...";
        int estimatedWidth = loadingText.Length * 8;
        int startX = (screenWidth - estimatedWidth) / 2;
        int startY = screenHeight / 2;

        engine.WriteFiglet(new Point(startX, startY), loadingText, MainMenu.font1, 7);
        engine.DisplayBuffer();

        // Simulate loading time
        for (loadingProgress = 0; loadingProgress <= 100; loadingProgress += 10)
        {
            engine.SetPixel(new Point(startX + loadingProgress / 2, startY + 10), 7, ConsoleCharacter.Full);
            engine.SetPixel(new Point(startX + (loadingProgress+1) / 2, startY + 10), 7, ConsoleCharacter.Full);
            engine.SetPixel(new Point(startX + (loadingProgress+2) / 2, startY + 10), 7, ConsoleCharacter.Full);
            engine.SetPixel(new Point(startX + (loadingProgress+3) / 2, startY + 10), 7, ConsoleCharacter.Full);
            engine.SetPixel(new Point(startX + (loadingProgress+4) / 2, startY + 10), 7, ConsoleCharacter.Full);
            engine.SetPixel(new Point(startX + loadingProgress / 2, startY + 11), 7, ConsoleCharacter.Full);
            engine.SetPixel(new Point(startX + (loadingProgress+1) / 2, startY + 11), 7, ConsoleCharacter.Full);
            engine.SetPixel(new Point(startX + (loadingProgress+2) / 2, startY + 11), 7, ConsoleCharacter.Full);
            engine.SetPixel(new Point(startX + (loadingProgress+3) / 2, startY + 11), 7, ConsoleCharacter.Full);
            engine.SetPixel(new Point(startX + (loadingProgress+4) / 2, startY + 11), 7, ConsoleCharacter.Full);
            engine.SetPixel(new Point(startX + loadingProgress / 2, startY + 12), 7, ConsoleCharacter.Full);
            engine.SetPixel(new Point(startX + (loadingProgress+1) / 2, startY + 12), 7, ConsoleCharacter.Full);
            engine.SetPixel(new Point(startX + (loadingProgress+2) / 2, startY + 12), 7, ConsoleCharacter.Full);
            engine.SetPixel(new Point(startX + (loadingProgress+3) / 2, startY + 12), 7, ConsoleCharacter.Full);
            engine.SetPixel(new Point(startX + (loadingProgress+4) / 2, startY + 12), 7, ConsoleCharacter.Full);
            engine.DisplayBuffer();
            Thread.Sleep(1000); // Adjust the sleep time as needed
        }

        doneLoading = true;
    }
}
