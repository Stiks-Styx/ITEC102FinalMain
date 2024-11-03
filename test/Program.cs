using System;
using ConsoleGameEngine;

namespace test
{
    class Program : ConsoleGame
    {
        int option = 0;
        private static FigletFont font;
        bool isMainStart = true;
        private int i = 0;

        private static void Main(string[] args)
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            new Program().Construct(width + 150, height*2, 1, 1, FramerateMode.Unlimited);
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
                if (isMainStart)
                {
                    isMainStart = false;
                }
                else
                {
                    switch (option)
                    {
                        case 0: OnePlayer(); break;   // 1 player
                        case 2: TwoPlayer(); break;   // 2 players
                        case 4: Score(); break;       // View Scores
                        case 6: Environment.Exit(0); return;  // Exit
                    }
                }
            }

            if (Engine.GetKeyDown(ConsoleKey.W) && option > 0)
                option -= 2;
            if (Engine.GetKeyDown(ConsoleKey.S) && option < 6)
                option += 2;
        }

        public override void Render()
        {
            if (isMainStart)
                MenuStart();
            else
                MainMenu();
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
            int colorValue = 1; // Starting color value

            while (true)
            {
                for (i = 0; i <= 3; i++)
                {
                    Engine.ClearBuffer(); // Clear the buffer to redraw

                    // Write the text with the current color value
                    Engine.WriteFiglet(new Point(0, i), "Looking", font, colorValue);
                    Engine.WriteFiglet(new Point(0, i + 10), "for one", font, colorValue+1);
                    Engine.WriteFiglet(new Point(0, i + 20), "Member", font, colorValue+2);
                    Engine.WriteFiglet(new Point(0, i + 30), "ITEC-102", font, colorValue+3);
                    Engine.DisplayBuffer();

                    System.Threading.Thread.Sleep(500); // Adjust the speed of the animation

                    // Increment colorValue and wrap around if it exceeds 10
                    colorValue++;
                    if (colorValue > 5)
                        colorValue = 2;
                }

                if (Engine.GetKeyDown(ConsoleKey.Enter))
                    break; // Exit the animation loop when Enter is pressed
            }
        }


        public void Score()
        {
            Engine.ClearBuffer();
            Engine.WriteText(new Point(0, 0), "Scores", 1);
            // Here you can add code to display actual scores
            Engine.WriteText(new Point(0, 2), "No scores to display yet.", 1);
            Engine.DisplayBuffer();
            System.Threading.Thread.Sleep(2000); // Pause to allow players to see the score screen
        }

        public void OnePlayer()
        {
            Engine.ClearBuffer();
            Engine.WriteText(new Point(0, 0), "1-Player mode selected.", 1);
            // You can add your game logic here
            Engine.DisplayBuffer();
            System.Threading.Thread.Sleep(2000); // Pause to allow players to see the message
        }

        public void TwoPlayer()
        {
            Engine.ClearBuffer();
            Engine.WriteText(new Point(0, 0), "2-Player mode selected.", 1);
            // You can add your game logic here
            Engine.DisplayBuffer();
            System.Threading.Thread.Sleep(2000); // Pause to allow players to see the message
        }
    }
}
