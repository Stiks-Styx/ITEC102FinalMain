using ConsoleGameEngine;
using System.Xml.Linq;
using WindowsInput;
using WindowsInput.Native;

namespace _Game_Main
{
    class Program : ConsoleGame
    {
        private Player player;
        private MainMenu menu;
        private Timer timer;
        private DebugHelper debugHelper; // Instance of DebugHelper

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

            // Initialize MainMenu and DebugHelper
            menu = new MainMenu(Engine, Width, Height, isPlaying, this, player);
            player = new Player(Engine, new Point(10, (Height / 2)), Width, Height, menu, this);

            // Set up DebugHelper with dependencies
            debugHelper = new DebugHelper(Engine, MainMenu.font1, Height, menu.player1Name);

            // Set a timer for the game loop (running every frame)
            ZoomOut();
            timer = new Timer(UpdateScreen, null, 0, 1000 / TargetFramerate);
        }

        private void UpdateScreen(object state)
        {
            if (isPlaying)
            {
                // Update the player and enemies
                player.Update();

                // Render the player and enemies
                player.Render();

                //debugHelper.GameDebugInfo(menu.player1Name, null);
            }
            else
            {
                // Render the menu
                menu.Render();
                menu.Update();

                // Render debug info
                //debugHelper.MenuDebugInfo(menu.currentPage, menu.selectorPosition);
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

            for (int i = 0; i < 8; i++) // Repeat 4 times
            {
                sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.OEM_MINUS);
                System.Threading.Thread.Sleep(100); // Optional: Add a small delay between key presses
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

            // Sort the scores in descending order by value
            var sortedScores = scoresElement.Elements("score")
                .OrderByDescending(score => (int)score.Attribute("value"))
                .ToList();

            // Clear the existing scores
            scoresElement.RemoveAll();

            // Add the sorted scores back to the "scores" element
            foreach (var score in sortedScores)
            {
                scoresElement.Add(score);
            }

            document.Save(filePath);
        }

        public void ReadScore(string filePath)
        {
            // Load the XML document
            XDocument xdoc = XDocument.Load(filePath);

            // Query the scores
            var scores = from score in xdoc.Descendants("score")
                         select new
                         {
                             Player = score.Attribute("player").Value,
                             Value = score.Attribute("value").Value
                         };

            int offset = 5;
            // Display the scores
            foreach (var score in scores)
            {
                Engine.WriteFiglet(new Point(10, 40+offset), $"Player: {score.Player}, Score: {score.Value}", MainMenu.font1, 2);
                offset += 5;
            }
        }
    }
}
