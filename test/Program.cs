using System;
using ConsoleGameEngine;

namespace test
{
    class Program : ConsoleGame
    {
        int option = 0;
        private static FigletFont font;
        bool isMainStart = true;


        private static void Main(string[] args)
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            new Program().Construct(width + 150, height, 1, 1, FramerateMode.Unlimited);
        }

        public override void Create()
        {
            Console.SetBufferSize(Console.WindowWidth + 150, Console.WindowHeight);
            Engine.SetPalette(Palettes.Pico8);
            Engine.Borderless();
            Console.Title = "Console Game";
            font = FigletFont.Load("C:\\Users\\Styx\\Desktop\\ITEC102FinalMain\\test\\3d.flf");
        }

        public override void Update()
        {
            if (Engine.GetKeyDown(ConsoleKey.Escape))
                Environment.Exit(0);
            if (Engine.GetKeyDown(ConsoleKey.Enter))
            {
                isMainStart = false;
            }

            if (Engine.GetKey(ConsoleKey.Enter) && option == 0)
            {
                // 1 player
                OnePlayer();
            }
            if (Engine.GetKey(ConsoleKey.Enter) && option == 2)
            {
                // 2 players
                TwoPlayer();
            }
            if (Engine.GetKey(ConsoleKey.Enter) && option == 4)
            {
                // View Scores
                Score();
            }
            if (Engine.GetKey(ConsoleKey.Enter) && option == 6)
            {
                Environment.Exit(0);
            }

            if (Engine.GetKeyDown(ConsoleKey.W) && option != 0)
                option -=2;
            if (Engine.GetKeyDown(ConsoleKey.S) && option != 6)
                option +=2;
        }

        public override void Render()
        {
            if (isMainStart == true)
                MenuStart();
            else { MainMenu(); }
            
        }

        public void MainMenu()
        {
            Engine.ClearBuffer();
            Engine.WriteText(new Point(0, 0), "[ ]1-Player", 1);
            Engine.WriteText(new Point(0, 2), "[ ]2-Players", 1);
            Engine.WriteText(new Point(0, 4), "[ ]Scores", 1);
            Engine.WriteText(new Point(0, 6), "[ ]Exit", 1);
            Engine.SetPixel(new Point(1, option), 255, ConsoleCharacter.Full);
            Engine.DisplayBuffer();
        }

        public void MenuStart()
        {
            Engine.ClearBuffer();
            try
            {
                Engine.WriteFiglet(new Point(0, 0), "Welcome", font, 1);
                Engine.WriteFiglet(new Point(0, 10), "To Our", font, 1);
                Engine.WriteFiglet(new Point(0, 20), "Game", font, 1);
                Engine.WriteText(new Point(0, 30), "Press Enter To Continue...", 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Engine.DisplayBuffer();
        }

        public void Score()
        {
            Engine.ClearBuffer();
            Engine.WriteText(new Point(0, 50), "Scores", 1);
            Engine.DisplayBuffer();
        }
        public void OnePlayer()
        {
            Engine.ClearBuffer();
            Engine.WriteText(new Point(0, 50), "1-Player", 1);
            Engine.DisplayBuffer();
        }
        public void TwoPlayer()
        {
            Engine.ClearBuffer();
            Engine.WriteText(new Point(0, 50), "2-Player", 1);
            Engine.DisplayBuffer();
        }

    }
}
