using System;
using ConsoleGameEngine;

namespace test
{
    class Program : ConsoleGame
    {

        private static void Main(string[] args)
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            new Program().Construct(width + 150, height + 150, 1, 1, FramerateMode.Unlimited);
        }

        public override void Create()
        {
            Console.SetBufferSize(Console.WindowWidth * 2, Console.WindowHeight * 2);
            Engine.SetPalette(Palettes.Pico8);
            Engine.Borderless();
            Console.Title = "Console Game";
        }

        public override void Update()
        {
        }

        public override void Render()
        {
        }

    }
}
