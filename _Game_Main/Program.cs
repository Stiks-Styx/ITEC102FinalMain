using ConsoleGameEngine;
using System;

namespace _Game_Main
{
    class Program : ConsoleGame
    {
        private Player player;
        private Timer timer;

        private static void Main(string[] args)
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            new Program().Construct(width, height, 1, 1, FramerateMode.Unlimited);
        }

        public override void Create()
        {
            Engine.SetPalette(Palettes.Pico8);
            Engine.Borderless();
            Console.Title = "GAMER!!";
            TargetFramerate = 60;

            player = new Player(Engine, new Point(10,10));
            timer = new Timer(UpdateScreen, null, 0, 1000 / TargetFramerate);
        }

        private void UpdateScreen(object state)
        {
            // Update both players
            player.Update();
            //player2.Update();

            // Render both players
            player.Render();
            //player2.Render();

        }

        public override void Render()
        {

        }

        public override void Update()
        { }
    }
}
