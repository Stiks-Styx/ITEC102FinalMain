using ConsoleGameEngine;
using System.Xml.Linq;
using WindowsInput;
using WindowsInput.Native;

namespace _Game_Main
{
    class Program : ConsoleGame
    {
        private Player? player;
        public MainMenu? menu;
        private Timer? timer;
        private DebugHelper? debugHelper;
        private BorderRenderer? borderRenderer;
        private CollisionDetector? collisionDetector;
        private SoundPlayer? ambiencePlayer;
        public PauseRender? pauseRender;
        private GameDisplay? gameDisplay;
        private GameLoading? gameLoading;
        private Enemy? enemy;
        private bool isAmbiencePlaying = false;
        private List<Enemy> enemies = new List<Enemy>();
        private List<Enemy> lifeCubes = new List<Enemy>();

        public int Width { get; private set; } = 440;
        public int Height { get; private set; } = 115;
        public bool isPlaying { get; set; } = false;
        private bool isGameOver = false; // New flag to indicate game over

        private int pauseCooldown = 10; // Cooldown in frames
        private int pauseTime = 0;

        private static void Main(string[] args)
        {
            var program = new Program();
            program.Construct(program.Width, program.Height, 1, 1, FramerateMode.Unlimited);
        }

        public override void Create()
        {

            Engine.SetPalette(Palettes.Pico8);
            Console.Title = "Void Invader";
            TargetFramerate = 30;

            gameLoading = new GameLoading(Engine, Width, Height);
            borderRenderer = new BorderRenderer(Engine, Width, Height, this);
            menu = new MainMenu(Engine, Width, Height, isPlaying, this);
            collisionDetector = new CollisionDetector(Engine);
            ambiencePlayer = new SoundPlayer(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", "ambience.mp3"));
            player = new Player(Engine, new Point(10, (Height / 2)), Width, Height, borderRenderer, this, collisionDetector, menu);
            debugHelper = new DebugHelper(Engine, MainMenu.font1, Height, menu.player1Name);
            gameDisplay = new GameDisplay(Engine, player, menu);

            ZoomOut();
            timer = new Timer(UpdateScreen, null, 0, 1000 / TargetFramerate);
            pauseRender = new PauseRender(Engine);
        }

        private void UpdateScreen(object state)
        {
            if (isPlaying)
            {
                player?.Update(enemies, lifeCubes);

                if (pauseTime > 0) pauseTime--;

                if (Engine.GetKey(ConsoleKey.Escape) && pauseTime == 0)
                {
                    pauseRender?.TogglePause();
                    pauseTime = pauseCooldown; // Reset cooldown
                }

                if (pauseRender?.IsPaused == true)
                {
                    pauseRender.RenderPauseScreen();
                    return;
                }
                if (player?.playerOneLife <= 0)
                {
                    isPlaying = false;
                    isGameOver = true; // Set game over flag
                    pauseRender?.GameOver();
                    return;
                }
                Enemy.ManageEnemies(Engine, enemies, Width, Height, player.score);
                player.Render(enemies);

            }
            else if (isGameOver)
            {
                pauseRender?.GameOver();
                if (Engine.GetKey(ConsoleKey.Enter))
                {
                    ResetGame();
                }
            }
            else
            {
                if (gameLoading.doneLoading == false)
                {
                    gameLoading.DisplayLoadingScreen();
                }
                else
                {
                    // audio not working
                    if (!isAmbiencePlaying)
                    {
                        ambiencePlayer.Play();
                        isAmbiencePlaying = true;
                    }

                    menu.Render();
                    menu.Update();
                }
            }
        }

        private void ResetGame()
        {
            isGameOver = false;
            isPlaying = false;
            player = new Player(Engine, new Point(10, (Height / 2)), Width, Height, borderRenderer, this, collisionDetector, menu);
            enemies.Clear();
            lifeCubes.Clear();
            menu.ResetToMainMenu();
        }


        public override void Render()
        {
            // Rendering logic handled in UpdateScreen callback
        }

        public override void Update()
        {
            // The update logic is handled in the UpdateScreen callback method
        }

        private static void ZoomOut()
        {
            var sim = new InputSimulator();

            for (int i = 0; i < 8; i++)
            {
                sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.OEM_MINUS);
                Thread.Sleep(100);
            }
        }

        public void RecordScore()
        {
            try
            {
                string filePath = "Scores\\Scores.xml";

                // Ensure directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                XElement scoreElement = new XElement("score",
                    new XAttribute("player", menu?.player1Name ?? "Unknown Player"),
                    new XAttribute("value", player?.score ?? 0),
                    new XAttribute("escapedEnemies", Enemy.escEnemy)); // Use proper static property

                XDocument document;

                if (File.Exists(filePath))
                {
                    document = XDocument.Load(filePath);
                }
                else
                {
                    document = new XDocument(
                        new XDeclaration("1.0", "UTF-8", "yes"),
                        new XElement("scores"));
                }

                XElement scoresElement = document.Root;
                if (scoresElement == null)
                {
                    scoresElement = new XElement("scores");
                    document.Add(scoresElement);
                }

                scoresElement.Add(scoreElement);

                // Sort scores by value in descending order
                var sortedScores = scoresElement.Elements("score")
                    .OrderByDescending(score => (int?)score.Attribute("value") ?? 0)
                    .ToList();

                scoresElement.RemoveAll(); // Clear current scores
                foreach (var score in sortedScores)
                {
                    scoresElement.Add(score);
                }

                document.Save(filePath);
            }
            catch (IOException ex)
            {
                Console.WriteLine("An error occurred while recording the score: " + ex.Message);
            }
        }


        public List<(string Player, string Value, string EscapedEnemies)> ReadScore(string filePath)
        {
            var scores = new List<(string Player, string Value, string EscapedEnemies)>();

            if (File.Exists(filePath))
            {
                var document = XDocument.Load(filePath);
                var scoreElements = document.Root?.Elements("score") ?? Enumerable.Empty<XElement>();

                foreach (var scoreElement in scoreElements)
                {
                    var player = scoreElement.Attribute("player")?.Value;
                    var value = scoreElement.Attribute("value")?.Value;
                    var escapedEnemies = scoreElement.Attribute("escapedEnemies")?.Value;

                    if (player != null && value != null && escapedEnemies != null)
                    {
                        scores.Add((player, value, escapedEnemies));
                    }
                }
            }

            return scores;
        }
    }
}
