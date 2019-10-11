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

namespace Computer.Graphics.LR2.Wpf
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

        private void DrawBeziers(double [] arr)
        {
            GL.Map1(MapTarget.Map1Vertex3, -100, 100, 3, arr.Length / 3, arr);
            GL.Enable(EnableCap.Map1Vertex3);

            GL.Begin(PrimitiveType.LineStrip);
            GL.Color3(System.Drawing.Color.Red);
            for (double i = -100; i < 100; i += 0.01)
                GL.EvalCoord1(i);

            GL.Disable(EnableCap.Map1Vertex3);

            GL.End();
        }

        private void DrawSplines(List<Point> points, int addPointsCount = 100)
        {
            if (points != null && points.Count > 1)
            {
                GL.Begin(PrimitiveType.LineStrip);
                GL.Color3(System.Drawing.Color.Red);

                points.Insert(0, points.First());
                points.Insert(0, points.First());
                points.Add(points.Last());
                points.Add(points.Last());
                for (int p = 1; p < points.Count - 2; p++)
                {
                    double a3 = (-points[p - 1].X + 3 * points[p].X - 3 * points[p + 1].X + points[p + 2].X) / 6;
                    double a2 = (points[p - 1].X - 2 * points[p].X + points[p + 1].X) / 2;
                    double a1 = (-points[p - 1].X + points[p + 1].X) / 2;
                    double a0 = (points[p - 1].X + 4 * points[p].X + points[p + 1].X) / 6;

                    double b3 = (-points[p - 1].Y + 3 * points[p].Y - 3 * points[p + 1].Y + points[p + 2].Y) / 6;
                    double b2 = (points[p - 1].Y - 2 * points[p].Y + points[p + 1].Y) / 2;
                    double b1 = (-points[p - 1].Y + points[p + 1].Y) / 2;
                    double b0 = (points[p - 1].Y + 4 * points[p].Y + points[p + 1].Y) / 6;

                    double step = 1d / addPointsCount;
                    for (double t = 0; t <= 1; t += step)
                    {
                        double x = a3 * Math.Pow(t, 3) + a2 * Math.Pow(t, 2) + a1 * t + a0;
                        double y = b3 * Math.Pow(t, 3) + b2 * Math.Pow(t, 2) + b1 * t + b0;

                        GL.Vertex2(x, y);
                    }
                }

                GL.End();
            }
        }

        private void DrawLine(double x1, double y1, double x2, double y2)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(System.Drawing.Color.Red);

            GL.Vertex2(x1, y1);
            GL.Vertex2(x2, y2);

            GL.End();
        }

        private void Task2Btn_Click(object sender, RoutedEventArgs e)
        {
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
                };
                visualWindow.RenderFrame += (s, args) =>
                {
                    float left = -100,
                        right = 100,
                        down = -100,
                        up = 100;

                    GL.ClearColor(255, 255, 255, 255);
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.Ortho(left, right, down, up, 0, 100);

                    DrawBeziers(new double[] {
                        0, -80, 0,
                        -70, 20, 0,
                        -40, 85, 0,
                        0, 35, 0
                    });
                    DrawBeziers(new double[] {
                        0, -80, 0,
                        70, 20, 0,
                        40, 85, 0,
                        0, 35, 0
                    });

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
                    visualWindow.Width = visualWindow.Height = 400;
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
                        down = -100,
                        up = 100;

                    GL.ClearColor(255, 255, 255, 255);
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.Ortho(left, right, down, up, 0, 100);

                    DrawSplines(new List<Point>() {
                        new Point(0, -80),
                        new Point(-70, 20),
                        new Point(-40, 85),
                        new Point(0, 35)
                    });
                    DrawSplines(new List<Point>() {
                        new Point(0, -80),
                        new Point(70, 20),
                        new Point(40, 85),
                        new Point(0, 35)
                    });

                    visualWindow.SwapBuffers();
                };

                visualWindow.Run(60.0);
            }
        }

        private void Task4Btn_Click(object sender, RoutedEventArgs e)
        {
            using (var visualWindow = new VisualWindow())
            {
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
                    DrawSplines(new List<Point>() {
                        new Point(20, 70),
                        new Point(18, 70),
                        new Point(10, 75),
                        new Point(10, 85),
                        new Point(18, 90),
                        new Point(20, 90)
                    });
                    DrawLine(20, 90, 80, 90);
                    DrawSplines(new List<Point>() {
                        new Point(80, 70),
                        new Point(82, 70),
                        new Point(90, 75),
                        new Point(90, 85),
                        new Point(82, 90),
                        new Point(80, 90)
                    });
                    DrawLine(20, 70, 40, 70);
                    DrawLine(60, 70, 80, 70);
                    DrawLine(40, 70, 40, 20);
                    DrawLine(60, 70, 60, 20);
                    DrawSplines(new List<Point>() {
                        new Point(40, 20),
                        new Point(40, 18),
                        new Point(45, 10),
                        new Point(55, 10),
                        new Point(60, 18),
                        new Point(60, 20)
                    });

                    // S
                    DrawSplines(new List<Point>() {
                        new Point(180, 90),
                        new Point(182, 90),
                        new Point(190, 85),
                        new Point(190, 75),
                        new Point(182, 70),
                        new Point(180, 70)
                    });
                    DrawLine(180, 70, 140, 70);
                    DrawLine(180, 90, 140, 90);
                    DrawSplines(new List<Point>() {
                        new Point(140, 70),
                        new Point(138, 70),
                        new Point(130, 67),
                        new Point(130, 63),
                        new Point(138, 60),
                        new Point(140, 60),
                    });
                    DrawSplines(new List<Point>() {
                        new Point(140, 90),
                        new Point(138, 90),
                        new Point(120, 85),
                        new Point(110, 70),
                        new Point(110, 60),
                        new Point(120, 45),
                        new Point(138, 40),
                        new Point(140, 40),
                    });
                    DrawLine(140, 40, 160, 40);
                    DrawLine(140, 60, 160, 60);
                    DrawSplines(new List<Point>() {
                        new Point(120, 10),
                        new Point(118, 10),
                        new Point(110, 15),
                        new Point(110, 25),
                        new Point(118, 30),
                        new Point(120, 30)
                    });
                    DrawLine(120, 10, 160, 10);
                    DrawLine(120, 30, 160, 30);
                    DrawSplines(new List<Point>() {
                        new Point(160, 30),
                        new Point(162, 30),
                        new Point(170, 33),
                        new Point(170, 37),
                        new Point(162, 40),
                        new Point(160, 40)
                    });
                    DrawSplines(new List<Point>() {
                        new Point(160, 10),
                        new Point(162, 10),
                        new Point(180, 15),
                        new Point(190, 30),
                        new Point(190, 40),
                        new Point(180, 55),
                        new Point(162, 60),
                        new Point(160, 60),
                    });

                    // I
                    DrawSplines(new List<Point>() {
                        new Point(220, 70),
                        new Point(218, 70),
                        new Point(210, 75),
                        new Point(210, 85),
                        new Point(218, 90),
                        new Point(220, 90)
                    });
                    DrawLine(220, 90, 280, 90);
                    DrawSplines(new List<Point>() {
                        new Point(280, 70),
                        new Point(282, 70),
                        new Point(290, 75),
                        new Point(290, 85),
                        new Point(282, 90),
                        new Point(280, 90)
                    });
                    DrawLine(260, 70, 280, 70);
                    DrawLine(220, 70, 240, 70);

                    DrawLine(240, 70, 240, 30);
                    DrawLine(260, 70, 260, 30);

                    DrawLine(260, 30, 280, 30);
                    DrawLine(220, 30, 240, 30);

                    DrawSplines(new List<Point>() {
                        new Point(220, 30),
                        new Point(218, 30),
                        new Point(210, 25),
                        new Point(210, 15),
                        new Point(218, 10),
                        new Point(220, 10)
                    });
                    DrawLine(220, 10, 280, 10);
                    DrawSplines(new List<Point>() {
                        new Point(280, 30),
                        new Point(282, 30),
                        new Point(290, 25),
                        new Point(290, 15),
                        new Point(282, 10),
                        new Point(280, 10)
                    });

                    visualWindow.SwapBuffers();
                };

                visualWindow.Run(60.0);
            }
        }
    }
}
