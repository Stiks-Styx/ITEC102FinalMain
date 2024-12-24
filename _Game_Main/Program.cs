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
        private bool isAmbiencePlaying = false;
        private List<Enemy> enemies = new List<Enemy>();
        private List<Enemy> lifeCubes = new List<Enemy>();

        public int Width { get; private set; } = 440;
        public int Height { get; private set; } = 115;
        public bool isPlaying { get; set; } = false;

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
            Engine.Borderless();
            Console.Title = "GAMER!!";
            TargetFramerate = 60;

            borderRenderer = new BorderRenderer(Engine, Width, Height, this);
            menu = new MainMenu(Engine, Width, Height, isPlaying, this);
            collisionDetector = new CollisionDetector(Engine);
            ambiencePlayer = new SoundPlayer(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sounds", "ambience.mp3"));
            player = new Player(Engine, new Point(10, (Height / 2)), Width, Height, borderRenderer, this, collisionDetector, menu);

            debugHelper = new DebugHelper(Engine, MainMenu.font1, Height, menu.player1Name);

            ZoomOut();
            timer = new Timer(UpdateScreen, null, 0, 1000 / TargetFramerate);
            pauseRender = new PauseRender(Engine);
        }

        private void UpdateScreen(object state)
        {
            // Handle pause with cooldown only when the game is playing
            if (isPlaying)
            {
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

                player.Update(enemies, lifeCubes);
                Enemy.ManageEnemies(Engine, enemies, Width, Height, player.score);
                player.Render(enemies);
            }
            else
            {
                if (!isAmbiencePlaying)
                {
                    ambiencePlayer.Play();
                    isAmbiencePlaying = true;
                }

                menu.Render();
                menu.Update();
            }
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
                XDocument document;
                string filePath = "Scores\\Scores.xml";

                XElement scoreElement = new XElement("score",
                    new XAttribute("player", menu.player1Name),
                    new XAttribute("value", player.score));

                if (!File.Exists(filePath))
                {
                    document = new XDocument(
                        new XDeclaration("1.0", "UTF-8", "yes"),
                        new XElement("scores"));
                    document.Save(filePath);
                }

                document = XDocument.Load(filePath);

                XElement scoresElement = document.Root.Element("scores");

                if (scoresElement == null)
                {
                    scoresElement = new XElement("scores");
                    document.Root.Add(scoresElement);
                }

                scoresElement.Add(scoreElement);

                var sortedScores = scoresElement.Elements("score")
                    .OrderByDescending(score => (int)score.Attribute("value"))
                    .ToList();

                scoresElement.RemoveAll();

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

        public List<(string Player, string Value)> ReadScore(string filePath)
        {
            var scoreList = new List<(string Player, string Value)>();

            try
            {
                XDocument xdoc = XDocument.Load(filePath);

                var scores = from score in xdoc.Descendants("score")
                             select new
                             {
                                 Player = score.Attribute("player").Value,
                                 Value = score.Attribute("value").Value
                             };

                foreach (var score in scores)
                {
                    scoreList.Add((score.Player, score.Value));
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("An error occurred while reading the scores: " + ex.Message);
            }

            return scoreList;
        }
    }
}
