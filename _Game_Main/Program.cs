using ConsoleGameEngine;
using System;

namespace _Game_Main
{
    class Program : ConsoleGame
    {
        private Player player;
<<<<<<< Updated upstream
        private Timer timer;

        private static void Main(string[] args)
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            new Program().Construct(width*2, height*3, 1, 1, FramerateMode.Unlimited);
=======
        private MainMenu menu;
        private Timer timer;

        public int width = 400;
        public int height = 100;
        public bool isPlaying = true;

        private static void Main(string[] args)
        {
            new Program().Construct(400, 100, 1, 1, FramerateMode.Unlimited);
>>>>>>> Stashed changes
        }

        public override void Create()
        {
            Engine.SetPalette(Palettes.Pico8);
            Engine.Borderless();
            Console.Title = "GAMER!!";
            TargetFramerate = 60;
<<<<<<< Updated upstream

            player = new Player(Engine, new Point(10,10));
            timer = new Timer(UpdateScreen, null, 0, 500 / TargetFramerate);
=======
            menu = new MainMenu(Engine);
            player = new Player(Engine, new Point(10, 10), menu.isSinglePlayer); // change the false to true if singleplayer

            // Set a timer for the game loop (running every frame)
            timer = new Timer(UpdateScreen, null, 0, 1000 / TargetFramerate);
>>>>>>> Stashed changes
        }

        private void UpdateScreen(object state)
        {
<<<<<<< Updated upstream
            // Update both players
            player.Update();
            //player2.Update();

            // Render both players
            player.Render();
            //player2.Render();

=======
            Engine.ClearBuffer();
            if (isPlaying == true)
            {
                // Update the player and enemies
                player.Update();

                // Render the player and enemies
                player.Render();
            }
            else
            {

                menu.Render();
                menu.Update();
            }
            Engine.DisplayBuffer();
>>>>>>> Stashed changes
        }

        public override void Render()
        {
            Engine.WriteText(new Point(0, 0), Convert.ToString(timer), 1);
        }

        public override void Update()
        { }
    }
}
