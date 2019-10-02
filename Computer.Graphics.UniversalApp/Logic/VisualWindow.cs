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
                    // Binding don't work
                    scene.WindowWidth += window.Width - scene.WindowWidth;
                    scene.WindowHeight += window.Height - scene.WindowHeight;

                    GL.Viewport(0, 0, window.Width, window.Height);
                };
                window.Closed += (s, args) =>
                {
                    window.Dispose();
                };
                window.UpdateFrame += (s, args) =>
                {
                    if (scene.WindowWidth != window.Width
                        || scene.WindowHeight != window.Height)
                    {
                        window.Width = scene.WindowWidth;
                        window.Height = scene.WindowHeight;
                    }
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

                    for (int p = 0; p < Math.Min(scene.Planes.Count, 6); p++)
                    {
                        GL.Enable(PlaneResources[p].EnableCap);
                        GL.ClipPlane(PlaneResources[p].ClipPlaneName, scene.Planes[p].Array);
                    }

                    //GL.ClipPlane(ClipPlaneName.ClipPlane0, new double[] { -1d, 3d, 1d, 0d });

                    GL.Begin(PrimitiveType.Lines);
                    GL.Color3(System.Drawing.Color.BlueViolet);
                    Coordinates currentCoords = new Coordinates();
                    foreach (int index in scene.IndexesSequence.Select(w => w.Value))
                    {
                        int absIndex = Math.Abs(index);
                        if (absIndex > 0 && absIndex < scene.Coordinates.Count + 1)
                        {
                            if (index > 0)
                            {
                                GL.Vertex2(currentCoords.X, currentCoords.Y);
                                GL.Vertex2(scene.Coordinates[index - 1].X, scene.Coordinates[index - 1].Y);
                            }
                            currentCoords = scene.Coordinates[absIndex - 1];
                        }
                    }

                    for (int p = 0; p < Math.Min(scene.Planes.Count, 6); p++)
                        GL.Disable(PlaneResources[p].EnableCap);

                    GL.End();
                    window.SwapBuffers();
                };

                window.Run(60.0);
            }
        }

        PlaneResource[] PlaneResources = new PlaneResource[]
        {
            new PlaneResource(EnableCap.ClipPlane0, ClipPlaneName.ClipPlane0),
            new PlaneResource(EnableCap.ClipPlane1, ClipPlaneName.ClipPlane1),
            new PlaneResource(EnableCap.ClipPlane2, ClipPlaneName.ClipPlane2),
            new PlaneResource(EnableCap.ClipPlane3, ClipPlaneName.ClipPlane3),
            new PlaneResource(EnableCap.ClipPlane4, ClipPlaneName.ClipPlane4),
            new PlaneResource(EnableCap.ClipPlane5, ClipPlaneName.ClipPlane5)
        };
    }

    class PlaneResource
    {
        public PlaneResource(EnableCap enableCap, ClipPlaneName clipPlaneName)
        {
            EnableCap = enableCap;
            ClipPlaneName = clipPlaneName;
        }

        public EnableCap EnableCap { get; set; }
        public ClipPlaneName ClipPlaneName { get; set; }
    }
}
