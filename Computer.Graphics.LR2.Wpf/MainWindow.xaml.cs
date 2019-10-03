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
                        new Point(13.5, -80),
                        new Point(-70, 20),
                        new Point(-40, 85),
                        new Point(8.5, 35)
                    });
                    DrawSplines(new List<Point>() {
                        new Point(-13.5, -80),
                        new Point(70, 20),
                        new Point(40, 85),
                        new Point(-8.5, 35)
                    });

                    visualWindow.SwapBuffers();
                };

                visualWindow.Run(60.0);
            }
        }
    }
}
