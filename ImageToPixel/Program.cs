using System;
using System.Drawing;
using ConsoleGameEngine;
using System.Collections.Generic;

class Program : ConsoleGame
{
    // Declare Bitmap as a field for accessibility in methods
    private Bitmap bitmap;
    // List to hold the coordinates of white pixels
    private List<System.Drawing.Point> whitePixelCoordinates = new List<System.Drawing.Point>();
    private List<System.Drawing.Point> blackPixelCoordinates = new List<System.Drawing.Point>();

    static void Main(string[] args)
    {
        int width = 200;
        int height = 100;
        new Program().Construct(width, height, 1, 1, FramerateMode.Unlimited);
    }

    public override void Create()
    {
        Console.SetBufferSize(Console.WindowWidth * 10, Console.WindowHeight * 10);
        Engine.SetPalette(Palettes.Pico8);
        Engine.Borderless();
        Console.Title = "Console Game";

        // Load the image
        bitmap = new Bitmap("bg.png"); // Ensure this path is valid
    }

    public override void Render()
    {
        Engine.ClearBuffer();
        whitePixelCoordinates.Clear(); // Clear previous coordinates
        blackPixelCoordinates.Clear();

        // Loop through each pixel
        for (int y = 0; y < bitmap.Height; y++)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                // Get the pixel color
                System.Drawing.Color pixelColor = bitmap.GetPixel(x, y);

                // Check if the pixel is white (255, 255, 255)
                if (pixelColor.R == 255 && pixelColor.G == 255 && pixelColor.B == 255)
                {
                    // Add the coordinate to the list
                    whitePixelCoordinates.Add(new System.Drawing.Point(x, y));
                    // Set pixel in console
                }
                else
                    blackPixelCoordinates.Add(new System.Drawing.Point(x, y));
            }
        }

        // Print the coordinates of white pixels
        foreach (System.Drawing.Point point in whitePixelCoordinates)
        {
            //Console.WriteLine($"White Pixel at: ({point.X}, {point.Y})");
            Engine.SetPixel(new ConsoleGameEngine.Point(point.X, point.Y), point.X, ConsoleCharacter.Full); // Set the pixel using the coordinates
        }
        /*foreach (System.Drawing.Point point1 in blackPixelCoordinates)
        {
            Engine.SetPixel(new ConsoleGameEngine.Point(point1.X, point1.Y), 3, ConsoleCharacter.Full); // Set the pixel using the coordinates
        }*/

        Engine.DisplayBuffer();
    }

    public override void Update()
    {
        // Game logic goes here
    }
}
