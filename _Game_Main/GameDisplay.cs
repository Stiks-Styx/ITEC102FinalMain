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
        engine.WriteFiglet(new Point(10, 5), $"Player Name: {mainMenu.player1Name}", MainMenu.font1, 7);

        // Display player life using the font from MainMenu
        engine.WriteFiglet(new Point(10, 10), $"Player Life: {player.playerOneLife}", MainMenu.font1, 7);
    }
}
