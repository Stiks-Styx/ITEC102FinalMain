using ConsoleGameEngine;
using System;

namespace _Game_Main
{
    class Program : ConsoleGame
    {
        private Player player;
        private EnemySpawner enemySpawner; // Add reference to enemy spawner
        private Timer timer;

        private static void Main(string[] args)
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;
            new Program().Construct(200*3, 100, 1, 1, FramerateMode.Unlimited);
        }

        public override void Create()
        {
            Engine.SetPalette(Palettes.Pico8);
            Engine.Borderless();
            Engine.Frame(new Point(1, 1), new Point(199, 99), 2);
            Console.Title = "GAMER!!";
            TargetFramerate = 60;

            player = new Player(Engine, new Point(10, 10), false); // change the false to true if singleplayer
            enemySpawner = new EnemySpawner(Engine); // Instantiate the enemy spawner

            // Set the enemy speed (1 step per frame by default, lower values for slower enemies)
            enemySpawner.SetEnemySpeed(.25f); // Slow speed (1 step per frame)

            // Set a timer for the game loop (running every frame)
            timer = new Timer(UpdateScreen, null, 0, 1000 / TargetFramerate);
        }


        private void UpdateScreen(object state)
        {
            // Update the player and enemies
            player.Update();

            // Pass the player bullets to the enemy spawner for collision detection
            enemySpawner.UpdateEnemies(player.playerOneBullets); // Use playerOneBullets or playerTwoBullets
            enemySpawner.UpdateEnemies(player.playerTwoBullets); // Use playerOneBullets or playerTwoBullets

            // Ensure there are always 10 enemies on the screen
            enemySpawner.EnsureEnemiesOnScreen(5);

            // Render the player and enemies
            player.Render();
            enemySpawner.RenderEnemies(); // Render all enemies and their positions
        }


        public override void Render()
        {
            // The rendering is handled within the UpdateScreen callback method
        }

        public override void Update()
        {
            // The update logic is handled in the UpdateScreen callback method
        }
    }
}
