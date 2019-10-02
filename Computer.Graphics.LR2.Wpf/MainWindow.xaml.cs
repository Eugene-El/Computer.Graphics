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
    }
}
