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
            engine.WriteText(new Point(20, 10), "Game Paused", 7);
            engine.DisplayBuffer();
        }
    }
}

