using ConsoleGameEngine;

namespace _Game_Main
{
    class Program : ConsoleGame
    {
        private Player player;
        private EnemySpawner enemySpawner; // Add reference to enemy spawner
        private MainMenu menu;
        private Timer timer;

        public int width = 400;
        public int height = 100;
        public bool isPlaying = false;

        private static void Main(string[] args)
        {
            int width = 400;
            int height = 100;

            new Program().Construct(width, height, 1, 1, FramerateMode.Unlimited);
        }

        public override void Create()
        {
            Console.SetBufferSize(width, height);
            Engine.SetPalette(Palettes.Pico8);
            Engine.Borderless();
            Console.Title = "GAMER!!";
            TargetFramerate = 60;
            menu = new MainMenu(Engine);
            player = new Player(Engine, new Point(10, 10), menu.isSinglePlayer); // change the false to true if singleplayer
            enemySpawner = new EnemySpawner(Engine); // Instantiate the enemy spawner

            // Set the enemy speed (1 step per frame by default, lower values for slower enemies)
            enemySpawner.SetEnemySpeed(.25f); // Slow speed (1 step per frame)

            // Set a timer for the game loop (running every frame)
            timer = new Timer(UpdateScreen, null, 0, 1000 / TargetFramerate);
        }


        private void UpdateScreen(object state)
        {
            if (isPlaying == true)
            {
                // Update the player and enemies
                player.Update();

                enemySpawner.UpdateEnemies(player.playerOneBullets, player.playerTwoPosition);


                enemySpawner.UpdateEnemies(player.playerTwoBullets, player.playerOnePosition);


                // Ensure there are always 10 enemies on the screen
                enemySpawner.EnsureEnemiesOnScreen(10);

                // Render the player and enemies
                player.Render();
                enemySpawner.RenderEnemies();
            }

            menu.Render();
            menu.Update();
        }


        public override void Render()
        {

        }

        public override void Update()
        {
            // The update logic is handled in the UpdateScreen callback method
        }
    }
}
