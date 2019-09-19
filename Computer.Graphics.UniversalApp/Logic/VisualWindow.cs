using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Computer.Graphics.UniversalApp.DataModels;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Computer.Graphics.UniversalApp.Logic
{
    public class VisualWindow
    {
        public VisualWindow(Scene scene)
        {
            using (var window = new GameWindow())
            {
                window.Load += (s, args) =>
                {
                    window.Width = scene.WindowWidth;
                    window.Height = scene.WindowHeight;
                    window.VSync = VSyncMode.On;
                };
                window.Resize += (s, args) =>
                {
                    GL.Viewport(0, 0, window.Width, window.Height);
                };
                window.Closed += (s, args) =>
                {
                    window.Dispose();
                };
                window.UpdateFrame += (s, args) =>
                {
                };
                window.RenderFrame += (s, args) =>
                {
                    GL.ClearColor(255, 255, 255, 255);
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.Ortho(
                        scene.WorldLeft,
                        scene.WorldRight,
                        scene.WorldBottom,
                        scene.WorldTop,
                        0, 100);

                    GL.Begin(PrimitiveType.Lines);

                    GL.Color3(System.Drawing.Color.BlueViolet);

                    Coordinates currentCoords = new Coordinates();
                    foreach (int index in scene.IndexesSequence)
                    {
                        if (index > 0)
                        {
                            GL.Vertex2(currentCoords.X, currentCoords.Y);
                            GL.Vertex2(scene.Coordinates[index - 1].X, scene.Coordinates[index - 1].Y);
                        }
                        currentCoords = scene.Coordinates[Math.Abs(index) - 1];
                    }

                    GL.End();
                    window.SwapBuffers();
                };

                window.Run(60.0);
            }
        }
    }
}
