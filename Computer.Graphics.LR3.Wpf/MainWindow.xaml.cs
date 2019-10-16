using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using VisualWindow = OpenTK.GameWindow;

namespace Computer.Graphics.LR3.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Task1Btn_Click(object sender, RoutedEventArgs e)
        {
            double baseSpeed = 1, speedUp = 1.2, slowness = 0.95, maxSpeed = 40;
            double xOffset = 0, yOffset = 0, xSpeed = 0, ySpeed = 0;
            using (var visualWindow = new VisualWindow())
            {
                visualWindow.Load += (s, args) =>
                {
                    visualWindow.Width = visualWindow.Height = 400;
                    visualWindow.VSync = VSyncMode.On;
                };
                visualWindow.Resize += (s, args) =>
                {
                    GL.Viewport(0, 0, visualWindow.Width, visualWindow.Height);
                };
                visualWindow.UpdateFrame += (s, args) =>
                {
                    var keyboardState = OpenTK.Input.Keyboard.GetState();

                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Escape))
                        visualWindow.Exit();

                    GL.MatrixMode(MatrixMode.Modelview);

                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Up) || keyboardState.IsKeyDown(OpenTK.Input.Key.W))
                        ySpeed = baseSpeed + ySpeed * (ySpeed > 0 ? speedUp : 0);
                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Down) || keyboardState.IsKeyDown(OpenTK.Input.Key.S))
                        ySpeed = -baseSpeed + ySpeed * (ySpeed < 0 ? speedUp : 0);
                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Left) || keyboardState.IsKeyDown(OpenTK.Input.Key.A))
                        xSpeed = -baseSpeed + xSpeed * (xSpeed < 0 ? speedUp : 0);
                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Right) || keyboardState.IsKeyDown(OpenTK.Input.Key.D))
                        xSpeed = baseSpeed + xSpeed * (xSpeed > 0 ? speedUp : 0);

                    // Max speed controll
                    xSpeed = Math.Abs(xSpeed) < maxSpeed ? xSpeed : maxSpeed * (xSpeed < 0 ? -1 : 1);
                    ySpeed = Math.Abs(ySpeed) < maxSpeed ? ySpeed : maxSpeed * (ySpeed < 0 ? -1 : 1);

                    // Small speed aprox to 0
                    if (Math.Abs(xSpeed) < 1) xSpeed = 0;
                    if (Math.Abs(ySpeed) < 1) ySpeed = 0;

                    // Auto slow speed
                    xSpeed *= slowness;
                    ySpeed *= slowness;

                    xOffset += xSpeed;
                    yOffset += ySpeed;
                };
                visualWindow.RenderFrame += (s, args) =>
                {
                    float left = -1000,
                        right = 1000,
                        down = -1000,
                        up = 1000;

                    GL.ClearColor(255, 255, 255, 255);
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.Ortho(left, right, down, up, 0, 100);
                    

                    GL.Begin(BeginMode.Lines);
                    GL.Vertex2(left, 0);
                    GL.Vertex2(right, 0);
                    GL.Vertex2(0, up);
                    GL.Vertex2(0, down);
                    GL.End();

                    GL.PushMatrix();
                    GL.Translate(xOffset, yOffset, 0);

                    GL.Begin(BeginMode.Quads);

                    GL.Color3(1.0, 1.0, 1.0);
                    GL.Vertex2(250, 450);
                    GL.Color3(0.0, 0.0, 1.0);
                    GL.Vertex2(250, 150);
                    GL.Color3(0.0, 1.0, 0.0);
                    GL.Vertex2(550, 150);
                    GL.Color3(1.0, 0.0, 0.0);
                    GL.Vertex2(550, 450);

                    GL.End();

                    GL.PopMatrix();

                    visualWindow.SwapBuffers();
                };

                visualWindow.Run(60.0);
            }
        }

        private void Task2Btn_Click(object sender, RoutedEventArgs e)
        {
            using (var visualWindow = new VisualWindow())
            {
                double angle = 0, selfAngle = 0, xOffset = 0, yOffset = 0, scale = 1;
                visualWindow.Load += (s, args) =>
                {
                    visualWindow.Width = 600;
                    visualWindow.Height = 200;
                    visualWindow.VSync = VSyncMode.On;
                };
                visualWindow.Resize += (s, args) =>
                {
                    GL.Viewport(0, 0, visualWindow.Width, visualWindow.Height);
                };
                visualWindow.UpdateFrame += (s, args) =>
                {
                    var keyboardState = OpenTK.Input.Keyboard.GetState();

                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Escape))
                        visualWindow.Exit();

                    GL.MatrixMode(MatrixMode.Modelview);

                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Up))
                        yOffset += 10;
                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Down))
                        yOffset -= 10;
                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Left))
                        xOffset -= 10;
                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Right))
                        xOffset += 10;

                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Plus))
                        scale += 0.1;
                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Minus))
                        scale -= 0.1;

                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Home))
                        angle += 5;
                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.End))
                        angle -= 5;

                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Q))
                        selfAngle += 3;

                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.E))
                        selfAngle -= 3;
                };
                visualWindow.RenderFrame += (s, args) =>
                {
                    float left = 0,
                        right = 300,
                        down = 0,
                        up = 100;

                    GL.ClearColor(255, 255, 255, 255);
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.Ortho(left, right, down, up, 0, 100);

                    // Translations
                    GL.Translate(xOffset, yOffset, 0);
                    GL.Translate(150, 50, 0);
                    GL.Scale(scale, scale, 0);
                    GL.Rotate(angle, 0, 0, 1);
                    GL.Translate(-150, -50, 0);


                    GL.Color3(255.0, 0, 0);

                    // T
                    GL.PushMatrix();
                    GL.Translate(50, 50, 0);
                    GL.Rotate(selfAngle, 0, 0, 1);
                    GL.Translate(-50, -50, 0);

                    GL.Begin(BeginMode.LineLoop);
                    GL.Vertex2(10, 80);
                    GL.Vertex2(90, 80);
                    GL.Vertex2(90, 70);
                    GL.Vertex2(55, 70);
                    GL.Vertex2(55, 20);
                    GL.Vertex2(45, 20);
                    GL.Vertex2(45, 70);
                    GL.Vertex2(10, 70);
                    GL.End();

                    GL.PopMatrix();


                    // S
                    GL.PushMatrix();
                    GL.Translate(150, 50, 0);
                    GL.Rotate(-selfAngle, 0, 0, 1);
                    GL.Translate(-150, -50, 0);

                    GL.Begin(BeginMode.LineLoop);
                    GL.Vertex2(190, 80);
                    GL.Vertex2(110, 80);
                    GL.Vertex2(110, 45);
                    GL.Vertex2(180, 45);
                    GL.Vertex2(180, 30);
                    GL.Vertex2(110, 30);
                    GL.Vertex2(110, 20);
                    GL.Vertex2(190, 20);
                    GL.Vertex2(190, 55);
                    GL.Vertex2(120, 55);
                    GL.Vertex2(120, 70);
                    GL.Vertex2(190, 70);
                    GL.End();

                    GL.PopMatrix();

                    // I
                    GL.PushMatrix();
                    GL.Translate(250, 50, 0);
                    GL.Rotate(selfAngle, 0, 0, 1);
                    GL.Translate(-250, -50, 0);

                    GL.Begin(BeginMode.LineLoop);
                    GL.Vertex2(265, 80);
                    GL.Vertex2(265, 70);
                    GL.Vertex2(255, 70);
                    GL.Vertex2(255, 30);
                    GL.Vertex2(265, 30);
                    GL.Vertex2(265, 20);
                    GL.Vertex2(235, 20);
                    GL.Vertex2(235, 30);
                    GL.Vertex2(245, 30);
                    GL.Vertex2(245, 70);
                    GL.Vertex2(235, 70);
                    GL.Vertex2(235, 80);
                    GL.End();

                    GL.PopMatrix();

                    visualWindow.SwapBuffers();
                };

                visualWindow.Run(60.0);
            }
        }
    }
}
