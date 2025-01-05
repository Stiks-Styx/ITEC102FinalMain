using ConsoleGameEngine;

class PauseRender
{
    private readonly ConsoleEngine engine;
    private bool isPaused = false;

    public PauseRender(ConsoleEngine engine)
    {
        this.engine = engine;
    }

    public bool IsPaused => isPaused;

    public void TogglePause()
    {
        isPaused = !isPaused;
    }

    public void RenderPauseScreen()
    {
        if (isPaused)
        {
            engine.ClearBuffer();
            engine.WriteFiglet(new Point(20, 10), "Game Paused", MainMenu.font1, 7);
            engine.DisplayBuffer();
        }
    }

    public void GameOver()
    {
        engine.ClearBuffer();
        engine.WriteFiglet(new Point(20, 10), "Game Over", MainMenu.font1, 7);
        engine.WriteFiglet(new Point(20, 15), "Press Enter to return to Main Menu", MainMenu.font1, 7);
        engine.DisplayBuffer();
    }
}

