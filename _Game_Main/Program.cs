using ConsoleGameEngine;
using System;
using System.Threading;

namespace _Game_Main
{
    class Program : ConsoleGame
    {
        private Point playerPosition;
        private Timer timer;
        Point current;
        Point current1;
        Point current2;
        Point current3;

        private static void Main(string[] args)
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            new Program().Construct(width, height, 16, 16, FramerateMode.Unlimited);
        }

        public override void Create()
        {
            Engine.SetPalette(Palettes.Pico8);
            Engine.Borderless();
            /*Engine.SetBackground(5);*/
            Console.Title = "GAMER!!";
            TargetFramerate = 60;

            // Initialize player position here
            current = new Point(1, 1);
            current1 = new Point(7, 1);
            current2 = new Point(4, 8);
            playerPosition = current2;

            timer = new Timer(UpdateScreen, null, 0, 1000 / TargetFramerate); // Set the timer interval to match the target frame rate
        }

        public override void Update()
        {
        }

        private void UpdateScreen(object state)
        {
            if (Engine.GetKeyDown(ConsoleKey.UpArrow) || Engine.GetKeyDown(ConsoleKey.W))
            {
                playerPosition.Y--;
            }
            if (Engine.GetKeyDown(ConsoleKey.DownArrow) || Engine.GetKeyDown(ConsoleKey.S))
            {
                playerPosition.Y++;
            }
            if (Engine.GetKeyDown(ConsoleKey.RightArrow) || Engine.GetKeyDown(ConsoleKey.D))
            {
                playerPosition.X++;
            }
            if (Engine.GetKeyDown(ConsoleKey.LeftArrow) || Engine.GetKeyDown(ConsoleKey.A))
            {
                playerPosition.X--;
            }

            Render();
        }

        public override void Render()
        {
            Engine.ClearBuffer();

            // Draw head
            //Engine.SetPixel(new Point(playerPosition.X, playerPosition.Y), 1, ConsoleCharacter.Full);//center head
            /*Engine.SetPixel(new Point(playerPosition.X+1, playerPosition.Y), 1, ConsoleCharacter.Full);
            Engine.SetPixel(new Point(playerPosition.X, playerPosition.Y), 1, ConsoleCharacter.Full);
            Engine.SetPixel(new Point(playerPosition.X-1, playerPosition.Y), 1, ConsoleCharacter.Full);*/

            Engine.WriteText(new Point(40, 0), Convert.ToString(playerPosition), 1);
            Engine.Triangle(current+playerPosition, current1+playerPosition, current2+playerPosition, 1, ConsoleCharacter.Light);


            Engine.DisplayBuffer();
        }

    }
}
/*
ff
████
████████
████████████
████████
████

  */