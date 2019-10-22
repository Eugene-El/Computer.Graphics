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

namespace Computer.Graphics.LR4.Wpf
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
            double xOffset = 0, yOffset = 0, zOffset = 0,
                scale = 1, xRotate = 0, yRotate = 0, zRotate = 0;
            bool perspectiveMode = false;
            int teaPotMode = 1;
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


                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Up))
                        yOffset += 10;
                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Down))
                        yOffset -= 10;
                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Right))
                        xOffset += 10;
                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Left))
                        xOffset -= 10;

                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Plus))
                        scale += 0.1;
                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Minus))
                        scale -= 0.1;


                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Home))
                        xRotate -= 5;
                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.End))
                        yRotate -= 5;
                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.Delete))
                        zRotate -= 5;


                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.PageUp))
                    {
                        xRotate += 5;
                        yRotate += 5;
                        zRotate += 5;
                    }
                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.PageDown))
                    {
                        xRotate -= 5;
                        yRotate -= 5;
                        zRotate -= 5;
                    }


                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.P))
                        perspectiveMode = true;
                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.O))
                        perspectiveMode = false;

                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.F1))
                        teaPotMode = 1;
                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.F2))
                        teaPotMode = 2;
                    if (keyboardState.IsKeyDown(OpenTK.Input.Key.F3))
                        teaPotMode = 3;
                };
                visualWindow.RenderFrame += (s, args) =>
                {
                    float left = -1000,
                        right = 1000,
                        down = -1000,
                        up = 1000,
                        forward = -1000,
                        back = 1000;

                    float width = right - left,
                        height = up - down,
                        depth = back - forward;

                    GL.ClearColor(255, 255, 255, 255);
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();

                    if (perspectiveMode)
                    {
                        // GLU Perspective
                        Matrix4 perspectiveMatrix = Matrix4.CreatePerspectiveFieldOfView(
                            MathHelper.PiOver2,
                            width / height,
                            0.1f,
                            depth);
                        GL.LoadMatrix(ref perspectiveMatrix);
                        // GLU Look at
                        GL.Translate(0, 0, forward);
                    }
                    else
                    {
                        GL.Ortho(left, right, down, up, forward, back);
                    }

                    // Draw Axis
                    GL.Color3(255d, 0d, 0d);
                    GL.Begin(BeginMode.Lines);
                    GL.Vertex3(left, 0, 0);
                    GL.Vertex3(right, 0, 0);
                    GL.Vertex3(0, up, 0);
                    GL.Vertex3(0, down, 0);
                    GL.Vertex3(0, 0, forward);
                    GL.Vertex3(0, 0, back);
                    GL.End();

                    // Translations
                    GL.Translate(xOffset, yOffset, zOffset);
                    GL.Scale(scale, scale, scale);
                    GL.Rotate(xRotate, 1, 0, 0);
                    GL.Rotate(yRotate, 0, 1, 0);
                    GL.Rotate(zRotate, 0, 0, 1);


                    // Drawing
                    GL.Color3(0d, 0d, 0d);
                    switch (teaPotMode)
                    {
                        case 1:
                            Teapot.DrawWireTeapot(500);
                            break;

                        case 2:
                            Teapot.DrawSolidTeapot(500);
                            break;

                        case 3:
                            Teapot.DrawPointTeapot(500);
                            break;
                    }

                    visualWindow.SwapBuffers();
                };

                visualWindow.Run(60.0);
            }
        }
    }


    public static class Teapot
    {
        // Rim, body, lid, and bottom data must be reflected in x and
        // y; handle and spout data across the y axis only.
        public static int[,] patchdata = new int[,]
      {
      // rim
      {102, 103, 104, 105, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15},
      // body
      {12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27},
      {24, 25, 26, 27, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40},
      // lid
      {96, 96, 96, 96, 97, 98, 99, 100, 101, 101, 101, 101, 0, 1, 2, 3,},
      {0, 1, 2, 3, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117},
      // bottom 
      {118, 118, 118, 118, 124, 122, 119, 121, 123, 126, 125, 120, 40, 39, 38, 37},
      // handle
      {41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56},
      {53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 28, 65, 66, 67},
      // spout
      {68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83},
      {80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95}
      };

        public static float[,] cpdata =
      {
        {0.2f, 0, 2.7f}, {0.2f, -0.112f, 2.7f}, {0.112f, -0.2f, 2.7f}, {0,
        -0.2f, 2.7f}, {1.3375f, 0, 2.53125f}, {1.3375f, -0.749f, 2.53125f},
        {0.749f, -1.3375f, 2.53125f}, {0, -1.3375f, 2.53125f}, {1.4375f,
        0, 2.53125f}, {1.4375f, -0.805f, 2.53125f}, {0.805f, -1.4375f,
        2.53125f}, {0, -1.4375f, 2.53125f}, {1.5f, 0, 2.4f}, {1.5f, -0.84f,
        2.4f}, {0.84f, -1.5f, 2.4f}, {0, -1.5f, 2.4f}, {1.75f, 0, 1.875f},
        {1.75f, -0.98f, 1.875f}, {0.98f, -1.75f, 1.875f}, {0, -1.75f,
        1.875f}, {2, 0, 1.35f}, {2, -1.12f, 1.35f}, {1.12f, -2, 1.35f},
        {0, -2, 1.35f}, {2, 0, 0.9f}, {2, -1.12f, 0.9f}, {1.12f, -2,
        0.9f}, {0, -2, 0.9f}, {-2, 0, 0.9f}, {2, 0, 0.45f}, {2, -1.12f,
        0.45f}, {1.12f, -2, 0.45f}, {0, -2, 0.45f}, {1.5f, 0, 0.225f},
        {1.5f, -0.84f, 0.225f}, {0.84f, -1.5f, 0.225f}, {0, -1.5f, 0.225f},
        {1.5f, 0, 0.15f}, {1.5f, -0.84f, 0.15f}, {0.84f, -1.5f, 0.15f}, {0,
        -1.5f, 0.15f}, {-1.6f, 0, 2.025f}, {-1.6f, -0.3f, 2.025f}, {-1.5f,
        -0.3f, 2.25f}, {-1.5f, 0, 2.25f}, {-2.3f, 0, 2.025f}, {-2.3f, -0.3f,
        2.025f}, {-2.5f, -0.3f, 2.25f}, {-2.5f, 0, 2.25f}, {-2.7f, 0,
        2.025f}, {-2.7f, -0.3f, 2.025f}, {-3, -0.3f, 2.25f}, {-3, 0,
        2.25f}, {-2.7f, 0, 1.8f}, {-2.7f, -0.3f, 1.8f}, {-3, -0.3f, 1.8f},
        {-3, 0, 1.8f}, {-2.7f, 0, 1.575f}, {-2.7f, -0.3f, 1.575f}, {-3,
        -0.3f, 1.35f}, {-3, 0, 1.35f}, {-2.5f, 0, 1.125f}, {-2.5f, -0.3f,
        1.125f}, {-2.65f, -0.3f, 0.9375f}, {-2.65f, 0, 0.9375f}, {-2,
        -0.3f, 0.9f}, {-1.9f, -0.3f, 0.6f}, {-1.9f, 0, 0.6f}, {1.7f, 0,
        1.425f}, {1.7f, -0.66f, 1.425f}, {1.7f, -0.66f, 0.6f}, {1.7f, 0,
        0.6f}, {2.6f, 0, 1.425f}, {2.6f, -0.66f, 1.425f}, {3.1f, -0.66f,
        0.825f}, {3.1f, 0, 0.825f}, {2.3f, 0, 2.1f}, {2.3f, -0.25f, 2.1f},
        {2.4f, -0.25f, 2.025f}, {2.4f, 0, 2.025f}, {2.7f, 0, 2.4f}, {2.7f,
        -0.25f, 2.4f}, {3.3f, -0.25f, 2.4f}, {3.3f, 0, 2.4f}, {2.8f, 0,
        2.475f}, {2.8f, -0.25f, 2.475f}, {3.525f, -0.25f, 2.49375f},
        {3.525f, 0, 2.49375f}, {2.9f, 0, 2.475f}, {2.9f, -0.15f, 2.475f},
        {3.45f, -0.15f, 2.5125f}, {3.45f, 0, 2.5125f}, {2.8f, 0, 2.4f},
        {2.8f, -0.15f, 2.4f}, {3.2f, -0.15f, 2.4f}, {3.2f, 0, 2.4f}, {0, 0,
        3.15f}, {0.8f, 0, 3.15f}, {0.8f, -0.45f, 3.15f}, {0.45f, -0.8f,
        3.15f}, {0, -0.8f, 3.15f}, {0, 0, 2.85f}, {1.4f, 0, 2.4f}, {1.4f,
        -0.784f, 2.4f}, {0.784f, -1.4f, 2.4f}, {0, -1.4f, 2.4f}, {0.4f, 0,
        2.55f}, {0.4f, -0.224f, 2.55f}, {0.224f, -0.4f, 2.55f}, {0, -0.4f,
        2.55f}, {1.3f, 0, 2.55f}, {1.3f, -0.728f, 2.55f}, {0.728f, -1.3f,
        2.55f}, {0, -1.3f, 2.55f}, {1.3f, 0, 2.4f}, {1.3f, -0.728f, 2.4f},
        {0.728f, -1.3f, 2.4f}, {0, -1.3f, 2.4f}, {0, 0, 0}, {1.425f,
        -0.798f, 0}, {1.5f, 0, 0.075f}, {1.425f, 0, 0}, {0.798f, -1.425f,
        0}, {0, -1.5f, 0.075f}, {0, -1.425f, 0}, {1.5f, -0.84f, 0.075f},
        {0.84f, -1.5f, 0.075f}
    };

        public static float[] tex =
      {
      0, 0,
      1, 0,
      0, 1,
      1, 1
    };

        private static void DrawTeapot(int grid, float scale, MeshMode2 type)
        {
            float[] p = new float[48], q = new float[48], r = new float[48], s = new float[48];
            int i, j, k, l;

            GL.PushAttrib(AttribMask.EnableBit | AttribMask.EvalBit);
            GL.Enable(EnableCap.AutoNormal);
            GL.Enable(EnableCap.Normalize);
            GL.Enable(EnableCap.Map2Vertex3);
            GL.Enable(EnableCap.Map2TextureCoord2);

            // time for the math portion: remember augmented matrices?  here's where you use them!
            // prep the matrix for the data to be loaded
            GL.PushMatrix();
            // rotate the view
            GL.Rotate(270.0f, 1.0f, 0.0f, 0.0f);
            // set the size of the data
            GL.Scale(0.5f * scale, 0.5f * scale, 0.5f * scale);
            // move the data via X/Y/Z coordinates
            GL.Translate(0.0f, 0.0f, -1.5f);
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    for (k = 0; k < 4; k++)
                    {
                        for (l = 0; l < 3; l++)
                        {
                            p[j * 12 + k * 3 + l] = cpdata[patchdata[i, j * 4 + k], l];
                            q[j * 12 + k * 3 + l] = cpdata[patchdata[i, j * 4 + (3 - k)], l];
                            if (l == 1)
                                q[j * 12 + k * 3 + l] *= -1.0f;
                            if (i < 6)
                            {
                                r[j * 12 + k * 3 + l] = cpdata[patchdata[i, j * 4 + (3 - k)], l];
                                if (l == 0)
                                    r[j * 12 + k * 3 + l] *= -1.0f;
                                s[j * 12 + k * 3 + l] = cpdata[patchdata[i, j * 4 + k], l];
                                if (l == 0)
                                    s[j * 12 + k * 3 + l] *= -1.0f;
                                if (l == 1)
                                    s[j * 12 + k * 3 + l] *= -1.0f;
                            }
                        }
                    }
                }

                // high level math for the texture coordinates
                GL.Map2(MapTarget.Map2TextureCoord2, 0f, 1f, 2, 2, 0f, 1f, 4, 2, tex);
                // high level math for the vertices
                GL.Map2(MapTarget.Map2Vertex3, 0f, 1f, 3, 4, 0f, 1f, 12, 4, p);
                // high level math for a 2 dimensional map
                GL.MapGrid2(grid, 0.0, 1.0, grid, 0.0, 1.0);
                // high level math to do the evaluation of the grids
                GL.EvalMesh2(type, 0, grid, 0, grid);
                // high level math for the vertices
                GL.Map2(MapTarget.Map2Vertex3, 0, 1, 3, 4, 0, 1, 12, 4, q);
                // high level math to do the evaluation of the grids
                GL.EvalMesh2(type, 0, grid, 0, grid);
                if (i < 6)
                {
                    // high level math for the vertices
                    GL.Map2(MapTarget.Map2Vertex3, 0, 1, 3, 4, 0, 1, 12, 4, r);
                    // high level math to do the evaluation of the grids
                    GL.EvalMesh2(type, 0, grid, 0, grid);
                    // high level math for the vertices
                    GL.Map2(MapTarget.Map2Vertex3, 0, 1, 3, 4, 0, 1, 12, 4, s);
                    // high level math to do the evaluation of the grids
                    GL.EvalMesh2(type, 0, grid, 0, grid);
                }
            }

            // release the manipulated data from the matrix
            GL.PopMatrix();
            // release the manipulated data from the matrix attributes
            GL.PopAttrib();
        }

        public static void DrawSolidTeapot(float scale)
        {
            DrawTeapot(6, scale, MeshMode2.Fill);
        }

        public static void DrawWireTeapot(float scale)
        {
            DrawTeapot(6, scale, MeshMode2.Line);
        }

        public static void DrawPointTeapot(float scale)
        {
            DrawTeapot(6, scale, MeshMode2.Point);
        }

    }
}
