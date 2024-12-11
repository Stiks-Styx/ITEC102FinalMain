using ConsoleGameEngine;
using System.Xml.Linq;
using WindowsInput;
using WindowsInput.Native;

namespace _Game_Main
{
    class Program : ConsoleGame
    {
        private Player? player;
        private MainMenu? menu;
        private Timer? timer;
        private DebugHelper? debugHelper;
        private BorderRenderer? borderRenderer;
        private CollisionDetector? collisionDetector;
        private SoundPlayer? ambiencePlayer;
        private bool isAmbiencePlaying = false;
        private PauseRender? pauseRender;
        private List<Enemy> enemies = new List<Enemy>();

        public int Width { get; private set; } = 440;
        public int Height { get; private set; } = 115;
        public bool isPlaying { get; set; } = false;

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
            ambiencePlayer = new SoundPlayer("C:\\Users\\Styx\\Desktop\\ITEC102FinalMain\\_Game_Main\\ambience.mp3");
            player = new Player(Engine, new Point(10, (Height / 2)), Width, Height, borderRenderer, this, collisionDetector);

            debugHelper = new DebugHelper(Engine, MainMenu.font1, Height, menu.player1Name);

            ZoomOut();
            timer = new Timer(UpdateScreen, null, 0, 1000 / TargetFramerate);
            pauseRender = new PauseRender(Engine);
        }

        private void UpdateScreen(object state)
        {
            if (Engine.GetKey(ConsoleKey.Escape))
            {
                pauseRender?.TogglePause();
            }

            if (pauseRender?.IsPaused == true)
            {
                pauseRender.RenderPauseScreen();
                return;
            }

            if (isPlaying)
            {
                player.Update(enemies);
                Enemy.ManageEnemies(Engine, enemies, Width, Height);
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
            XDocument document;
            string filePath = "C:\\Users\\Styx\\Desktop\\ITEC102FinalMain\\_Game_Main\\Scores.xml";

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

        public void ReadScore(string filePath)
        {
            XDocument xdoc = XDocument.Load(filePath);

            var scores = from score in xdoc.Descendants("score")
                         select new
                         {
                             Player = score.Attribute("player").Value,
                             Value = score.Attribute("value").Value
                         };

            int offset = 5;
            foreach (var score in scores)
            {
                Engine.WriteFiglet(new Point(10, 40 + offset), $"{score.Player}", MainMenu.font1, 7);
                offset += 5;
            }
        }
    }
}
