using System.Windows;
using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

using VisualWindow = OpenTK.GameWindow;
using System.IO;
using System.Collections.Generic;

namespace Computer.Graphics.LR1.Wpf
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
            using (var visualWindow = new VisualWindow())
            {
                visualWindow.Load += (s, args) =>
                {
                    visualWindow.VSync = VSyncMode.On;
                };
                visualWindow.Resize += (s, args) =>
                {
                    GL.Viewport(0, 0, visualWindow.Width, visualWindow.Height);
                };
                visualWindow.UpdateFrame += (s, args) =>
                {
                };
                visualWindow.RenderFrame += (s, args) =>
                {
                    float left = -100,
                        right = 100,
                        down = -50,
                        up = 50;


                    GL.ClearColor(255, 255, 255, 255);
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.Ortho(left, right, down, up, 0, 100);

                    GL.Begin(PrimitiveType.Lines);

                    // Axis
                    GL.Color3(Color.Black);
                    GL.Vertex2(0, -100);
                    GL.Vertex2(0, 100);
                    GL.Vertex2(-100, 0);
                    GL.Vertex2(100, 0);

                    // Graphic
                    GL.Color3(Color.DarkCyan);
                    float prevX = -100, prevY = 0;
                    for (float x = left; x <= right; x += 0.5f)
                    {
                        float y = Math.Abs(0.25f * x + 3 * (float)Math.Cos(100f * x) * (float)Math.Sin(x));
                        GL.Vertex2(prevX, prevY);
                        GL.Vertex2(x, y);
                        prevX = x; prevY = y;
                    }


                    GL.End();

                    visualWindow.SwapBuffers();
                };

                visualWindow.Run(60.0);
            }
        }

        private void Task2Btn_Click(object sender, RoutedEventArgs e)
        {
            using (var visualWindow = new VisualWindow())
            {
                visualWindow.Load += (s, args) =>
                {
                    visualWindow.VSync = VSyncMode.On;
                };
                visualWindow.Resize += (s, args) =>
                {
                    GL.Viewport(0, 0, visualWindow.Width, visualWindow.Height);
                };
                visualWindow.UpdateFrame += (s, args) =>
                {
                };
                visualWindow.RenderFrame += (s, args) =>
                {
                    float left = 0,
                        right = 100,
                        down = 0,
                        up = 100;


                    GL.ClearColor(255, 255, 255, 255);
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.Ortho(left, right, down, up, 0, 100);

                    GL.Begin(PrimitiveType.Lines);

                    GL.Color3(Color.Red);

                    DrawFromFile("points.txt");

                    GL.End();

                    visualWindow.SwapBuffers();
                };

                visualWindow.Run(60.0);
            }
        }

        private void Task3Btn_Click(object sender, RoutedEventArgs e)
        {
            using (var visualWindow = new VisualWindow())
            {
                visualWindow.Load += (s, args) =>
                {
                    visualWindow.VSync = VSyncMode.On;
                };
                visualWindow.Resize += (s, args) =>
                {
                    GL.Viewport(0, 0, visualWindow.Width, visualWindow.Height);
                };
                visualWindow.UpdateFrame += (s, args) =>
                {
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

                    // T
                    GL.Begin(PrimitiveType.LineLoop);

                    GL.Color3(Color.DarkBlue);
                    GL.Vertex2(10, 80);
                    GL.Vertex2(90, 80);
                    GL.Vertex2(90, 70);
                    GL.Vertex2(55, 70);
                    GL.Vertex2(55, 20);
                    GL.Vertex2(45, 20);
                    GL.Vertex2(45, 70);
                    GL.Vertex2(10, 70);

                    GL.End();

                    // S
                    GL.Begin(PrimitiveType.LineLoop);

                    GL.Color3(Color.DarkBlue);
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

                    // I
                    GL.Begin(PrimitiveType.LineLoop);

                    GL.Color3(Color.DarkBlue);
                    GL.Vertex2(235, 80);
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

                    GL.End();

                    visualWindow.SwapBuffers();
                };

                visualWindow.Run(60.0);
            }
        }

        private void Task3FileBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var visualWindow = new VisualWindow())
            {
                visualWindow.Load += (s, args) =>
                {
                    visualWindow.VSync = VSyncMode.On;
                };
                visualWindow.Resize += (s, args) =>
                {
                    GL.Viewport(0, 0, visualWindow.Width, visualWindow.Height);
                };
                visualWindow.UpdateFrame += (s, args) =>
                {
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

                    GL.Begin(PrimitiveType.Lines);

                    GL.Color3(Color.Red);

                    DrawFromFile("points2.txt");

                    GL.End();

                    visualWindow.SwapBuffers();
                };

                visualWindow.Run(60.0);
            }
        }

        private void DrawFromFile(string fileName)
        {
            List<PointF> points = new List<PointF>();
            if (File.Exists(fileName))
            {
                var lines = File.ReadAllLines(fileName);
                bool wasSplitter = false;
                float prevX = 0, prevY = 0;
                foreach (var line in lines)
                {
                    if (line.Contains("#"))
                        wasSplitter = true;
                    else
                    {
                        if (!wasSplitter)
                        {
                            var coords = line.Split(' ');
                            points.Add(new PointF(float.Parse(coords[0]), float.Parse(coords[1])));
                        }
                        else
                        {
                            var index = int.Parse(line);
                            if (index > 0)
                            {
                                GL.Vertex2(prevX, prevY);
                                GL.Vertex2(points[index - 1].X, points[index - 1].Y);
                            }
                            prevX = points[Math.Abs(index) - 1].X;
                            prevY = points[Math.Abs(index) - 1].Y;
                        }
                    }
                }
            }
        }
    }
}
