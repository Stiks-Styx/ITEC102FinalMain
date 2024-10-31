using ConsoleGameEngine;
using System.Runtime.InteropServices;
using static test1.ConsoleHelper;

namespace test1
{
    class Program : ConsoleGame
    {
        static void Main(string[] args)
        {


            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            new Program().Construct(width + 300, height + 300, 1, 1, FramerateMode.Unlimited);
            Console.SetWindowSize(width * 2, height * 2);
            Console.WriteLine("HEllo");

        }

        public override void Create()
        {
            Console.SetBufferSize(Console.WindowWidth + 300, Console.WindowHeight + 300);
            Engine.SetPalette(Palettes.Pico8);
            Engine.Borderless();
            Console.Title = "Console Game";
        }

        public override void Render()
        {

        }

        public override void Update()
        {

        }
    }
}
