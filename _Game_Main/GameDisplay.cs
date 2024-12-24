using ConsoleGameEngine;

class GameDisplay
{
    private readonly ConsoleEngine engine;
    private readonly Player player;
    private readonly MainMenu mainMenu;

    public GameDisplay(ConsoleEngine engine, Player player, MainMenu mainMenu)
    {
        this.engine = engine;
        this.player = player;
        this.mainMenu = mainMenu;
    }

    public void Render()
    {
        // Display player name using the font from MainMenu
        engine.WriteFiglet(new Point(4, 3), $"Player Name: {mainMenu.player1Name}", MainMenu.font1, 7);

        // Display player life using the font from MainMenu
        engine.WriteFiglet(new Point(4, 10), $"Player Life: {player.playerOneLife}", MainMenu.font1, 7);

        engine.WriteFiglet(new Point(Console.WindowWidth - 150, 3), $"Score: {player.score}", MainMenu.font1, 7);
    }
}
