using ConsoleGameEngine;
using System;

namespace _Game_Main
{
    class Program : ConsoleGame
    {
        private Player player;
        private Timer timer;

        List<_Enemy> enemies;

        private static void Main(string[] args)
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            new Program().Construct(width*2, height*3, 1, 1, FramerateMode.Unlimited);
        }

        public override void Create()
        {
            Engine.SetPalette(Palettes.Pico8);
            Engine.Borderless();
            Console.Title = "GAMER!!";
            TargetFramerate = 60;

            player = new Player(Engine, new Point(10,10), false);// change the false to true if singleplayer
            timer = new Timer(UpdateScreen, null, 0, 1000 / TargetFramerate);

            // List to store all active enemies
            enemies = new List<_Enemy>();

            // Add a Small enemy to the game
            enemies.Add(new Small(Engine, new Point(10, 5))); // Starting position (10,5)
        }

        private void UpdateScreen(object state)
        {
            // Update players
            player.Update();

            // Render players
            player.Render();

            foreach (var enemy in enemies)
            {
                enemy.EnemyMovement();
                enemy.EnemyAttack();
                enemy.EnemyRender();
            }

        }

        public override void Render()
        {
        }

        public override void Update()
        { 
        }
    }
}
