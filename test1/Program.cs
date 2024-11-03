using ConsoleGameEngine;
using static test1.ConsoleHelper;

namespace test1
{
    class Program : ConsoleGame
    {
        private int xCenter = 30, yCenter = 30; // Center of the triangle
        private double rotationAmount = 0;
        private double theta;
        private const double sideLength = 20; // Length of each side of the triangle
        private const double moveSpeed = 1.0; // Adjust for movement speed

        public Program()
        {
            UpdateTheta();
        }

        public static void Main(string[] args)
        {
            int height = 100 * 3;
            int width = 200 * 3;

            new Program().Construct(width, height, 1, 1, FramerateMode.Unlimited);
            Console.SetWindowSize(width, height);
        }

        private void UpdateTheta()
        {
            theta = rotationAmount * (Math.PI / 180);
        }

        public (double xPrime, double yPrime) RotatePoint(double x, double y, double xc, double yc, double theta)
        {
            double xPrime = xc + (x - xc) * Math.Cos(theta) - (y - yc) * Math.Sin(theta);
            double yPrime = yc + (x - xc) * Math.Sin(theta) + (y - yc) * Math.Cos(theta);
            return (xPrime, yPrime);
        }

        private void GetEquilateralTriangleVertices(out double x1, out double y1, out double x2, out double y2, out double x3, out double y3)
        {
            // Calculate the vertices of the equilateral triangle
            x1 = xCenter; // Top vertex (tip)
            y1 = yCenter - (sideLength * Math.Sqrt(3) / 2); // Top vertex

            x2 = xCenter - (sideLength / 2); // Bottom left vertex
            y2 = yCenter + (sideLength * Math.Sqrt(3) / 2); // Bottom left vertex

            x3 = xCenter + (sideLength / 2); // Bottom right vertex
            y3 = yCenter + (sideLength * Math.Sqrt(3) / 2); // Bottom right vertex
        }

        private void DrawLine(int x1, int y1, int x2, int y2, int color)
        {
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = (x1 < x2) ? 1 : -1;
            int sy = (y1 < y2) ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                Engine.SetPixel(new Point(x1, y1), color, ConsoleCharacter.Full);
                if (x1 == x2 && y1 == y2) break;
                int err2 = err * 2;
                if (err2 > -dy)
                {
                    err -= dy;
                    x1 += sx;
                }
                if (err2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }
            }
        }

        public void DrawTriangle(int x1, int y1, int x2, int y2, int x3, int y3, int color)
        {
            DrawLine(x1, y1, x2, y2, color);
            DrawLine(x2, y2, x3, y3, color);
            DrawLine(x3, y3, x1, y1, color);
        }

        public override void Create()
        {
            int height = 100 * 3;
            int width = 200 * 3;

            Console.SetBufferSize(width, height);
            Engine.SetPalette(Palettes.Pico8);
            Engine.Borderless();
            Console.Title = "Console Game";
        }

        private const double rotationSpeed = 5.0; // Speed of rotation

        public override void Update()
        {
            if (IsKeyPressed(VK_W))
            {
                MoveTriangle(); // Move the triangle based on current direction
            }
            if (IsKeyPressed(VK_A))
            {
                rotationAmount += rotationSpeed; // Rotate counter-clockwise
                UpdateTheta();
            }
            if (IsKeyPressed(VK_D))
            {
                rotationAmount -= rotationSpeed; // Rotate clockwise
                UpdateTheta();
            }

            // Keep rotationAmount within the range of 0 to 359 degrees
            rotationAmount = (rotationAmount + 360) % 360;

            // Debugging output to console
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"Position: ({xCenter}, {yCenter})  Angle: {rotationAmount}°");
        }



        private void MoveTriangle()
        {
            // Calculate the direction based on the current rotation
            double directionX = Math.Sin(theta); // Y-axis direction
            double directionY = -Math.Cos(theta); // Inverse for upward movement

            // Normalize the direction vector
            double length = Math.Sqrt(directionX * directionX + directionY * directionY);
            if (length > 0) // To prevent division by zero
            {
                directionX /= length; // Normalize
                directionY /= length; // Normalize
            }

            // Move the triangle center in the direction it is facing
            xCenter += (int)Math.Round(directionX * moveSpeed);
            yCenter += (int)Math.Round(directionY * moveSpeed);

            // Keep triangle within console bounds (if needed)
            xCenter = Math.Clamp(xCenter, 0, Console.WindowWidth - 1);
            yCenter = Math.Clamp(yCenter, 0, Console.WindowHeight - 1);
        }

        public override void Render()
        {
            Engine.ClearBuffer(); // Clear the previous frame

            // Get the original vertices of the equilateral triangle
            GetEquilateralTriangleVertices(out double x1, out double y1, out double x2, out double y2, out double x3, out double y3);

            // Rotate the triangle vertices
            (double rotatedX1, double rotatedY1) = RotatePoint(x1, y1, xCenter, yCenter, theta);
            (double rotatedX2, double rotatedY2) = RotatePoint(x2, y2, xCenter, yCenter, theta);
            (double rotatedX3, double rotatedY3) = RotatePoint(x3, y3, xCenter, yCenter, theta);

            // Draw the rotated triangle
            DrawTriangle((int)rotatedX1, (int)rotatedY1, (int)rotatedX2, (int)rotatedY2, (int)rotatedX3, (int)rotatedY3, 9);

            // Calculate the midpoint of the base
            double midX = (rotatedX2 + rotatedX3) / 2.0;
            double midY = (rotatedY2 + rotatedY3) / 2.0;

            // Draw a line from the tip to the midpoint of the base
            DrawLine((int)rotatedX1, (int)rotatedY1, (int)midX, (int)midY, 6); // Color 6 for the line

            Engine.DisplayBuffer();
        }
    }
}
