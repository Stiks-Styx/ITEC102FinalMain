using ConsoleGameEngine;
using System;

namespace _Game_Main
{
    class Program : ConsoleGame
    {
        private Player player1;
        private Player player2;
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
            //Engine.SetBackground(5);
            Console.Title = "GAMER!!";
            TargetFramerate = 60;

            player1 = new Player(Engine);
            player2 = new Player(Engine);
            timer = new Timer(UpdateScreen, null, 0, 1000 / TargetFramerate);
        }

        private void UpdateScreen(object state)
        {
            player1.Update();
            player1.Render();
        }

        public override void Render()
        {
        }

        public override void Update()
        {
        }
    }
}
