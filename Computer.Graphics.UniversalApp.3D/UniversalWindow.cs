using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Computer.Graphics.UniversalApp._3D.Models;
using Newtonsoft.Json;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using VisualWindow = OpenTK.GameWindow;

namespace Computer.Graphics.UniversalApp._3D
{
    public class UniversalWindow
    {
        protected Scene _loadedScene;
        protected VisualWindow _visualWindow;

        public void Start()
        {
            double xOffset = 0, yOffset = 0, zOffset = 0,
                xRotate = 0, yRotate = 0, zRotate = 0,
                scale = 1,
                moveSpeed = 3, rotateSpeed = 3, scaleSpeed = 0.05;
            bool perspectiveMode = false, drawAxis = false;
            _visualWindow = new VisualWindow();
            _visualWindow.Load += (s, args) =>
            {
                _visualWindow.Width = _visualWindow.Height = 400;
                _visualWindow.VSync = VSyncMode.On;

                LoadDefaultScene();
            };
            _visualWindow.Resize += (s, args) =>
            {
                GL.Viewport(0, 0, _visualWindow.Width, _visualWindow.Height);
            };
            _visualWindow.FileDrop += (s, args) =>
            {
                try
                {
                    string json = File.ReadAllText(args.FileName);
                    LoadScene(JsonConvert.DeserializeObject<Scene>(json));
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Load file error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
            _visualWindow.UpdateFrame += (s, args) =>
            {
                var keyboardState = Keyboard.GetState();

                if (keyboardState.IsKeyDown(Key.Escape))
                    _visualWindow.Exit();


                if (keyboardState.IsKeyDown(Key.Up))
                    yOffset += moveSpeed;
                if (keyboardState.IsKeyDown(Key.Down))
                    yOffset -= moveSpeed;
                if (keyboardState.IsKeyDown(Key.Right))
                    xOffset += moveSpeed;
                if (keyboardState.IsKeyDown(Key.Left))
                    xOffset -= moveSpeed;

                if (keyboardState.IsKeyDown(Key.Plus))
                    scale += scaleSpeed;
                if (keyboardState.IsKeyDown(Key.Minus))
                    scale -= scaleSpeed;

                if (keyboardState.IsKeyDown(Key.Home))
                    xRotate -= rotateSpeed;
                if (keyboardState.IsKeyDown(Key.End))
                    yRotate -= rotateSpeed;
                if (keyboardState.IsKeyDown(Key.Delete))
                    zRotate -= rotateSpeed;


                if (keyboardState.IsKeyDown(Key.PageUp))
                {
                    xRotate += rotateSpeed;
                    yRotate += rotateSpeed;
                    zRotate += rotateSpeed;
                }
                if (keyboardState.IsKeyDown(Key.PageDown))
                {
                    xRotate -= rotateSpeed;
                    yRotate -= rotateSpeed;
                    zRotate -= rotateSpeed;
                }

                if (keyboardState.IsKeyDown(Key.BackSpace))
                {
                    xOffset = 0; yOffset = 0; zOffset = 0;
                    xRotate = 0; yRotate = 0; zRotate = 0;
                    scale = 1;
                }

                if (keyboardState.IsKeyDown(Key.P))
                    perspectiveMode = true;
                if (keyboardState.IsKeyDown(Key.O))
                    perspectiveMode = false;
                if (keyboardState.IsKeyDown(Key.Z))
                    drawAxis = true;
                if (keyboardState.IsKeyDown(Key.Z) && keyboardState.IsKeyDown(Key.ShiftLeft))
                    drawAxis = false;
            };
            _visualWindow.RenderFrame += (s, args) =>
            {
                float width = _loadedScene.WorldRight - _loadedScene.WorldLeft,
                    height = _loadedScene.WorldTop - _loadedScene.WorldBottom,
                    depth = _loadedScene.WorldForward - _loadedScene.WorldBack;

                float halfX = (_loadedScene.WorldRight + _loadedScene.WorldLeft) / 2,
                    halfY = (_loadedScene.WorldTop + _loadedScene.WorldBottom) / 2,
                    halfZ = (_loadedScene.WorldBack + _loadedScene.WorldForward) / 2;


                GL.ClearColor(255, 255, 255, 255);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();

                if (perspectiveMode)
                {
                    // GLU Perspective
                    Matrix4 perspectiveMatrix = Matrix4.CreatePerspectiveFieldOfView(
                        MathHelper.DegreesToRadians(90),
                        width / height,
                        0.1f,
                        depth * 2);
                    GL.LoadMatrix(ref perspectiveMatrix);
                    // Look at
                    GL.Translate(-halfX, -halfY, -halfZ);
                }
                else
                {
                    GL.Ortho(_loadedScene.WorldLeft, _loadedScene.WorldRight,
                        _loadedScene.WorldBottom, _loadedScene.WorldTop,
                        _loadedScene.WorldForward, _loadedScene.WorldBack);
                }
                GL.Translate(0, 0, -depth);


                // Draw Axis
                if (drawAxis)
                {
                    GL.Color3(0f, 255f, 0f);
                    GL.Begin(BeginMode.Lines);
                    GL.Vertex3(-width, halfY, halfZ);
                    GL.Vertex3(width * 2, halfY, halfZ);
                    GL.Vertex3(halfX, -height, halfZ);
                    GL.Vertex3(halfX, height * 2, halfZ);
                    GL.Vertex3(halfX, halfY, -depth);
                    GL.Vertex3(halfX, halfY, depth);
                    GL.End();
                }

                // Translations
                GL.Translate(xOffset, yOffset, zOffset);
                GL.Translate(halfX, halfY, halfZ);
                GL.Rotate(xRotate, 1, 0, 0);
                GL.Rotate(yRotate, 0, 1, 0);
                GL.Rotate(zRotate, 0, 0, 1);
                GL.Scale(scale, scale, scale);
                GL.Translate(-halfX, -halfY, -halfZ);

                // Draw
                GL.Color3(255f, 0, 0);
                foreach (Parallelepiped parallelepiped in _loadedScene.Parallelepipeds)
                    DrawParallelepiped(parallelepiped.FirstCoordinates, parallelepiped.SecondCoordinates);


                _visualWindow.SwapBuffers();
            };

            _visualWindow.Run(60.0);
        }

        protected void DrawParallelepiped(Coordinates coord1, Coordinates coord2)
        {
            float
                minX = Math.Min(coord1.X, coord2.X),
                maxX = Math.Max(coord1.X, coord2.X),
                minY = Math.Min(coord1.Y, coord2.Y),
                maxY = Math.Max(coord1.Y, coord2.Y),
                minZ = Math.Min(coord1.Z, coord2.Z),
                maxZ = Math.Max(coord1.Z, coord2.Z);

            /* Front  Back
             * 0---3  3---0
             * |   |  |   |
             * 1---2  2---1
            */
            Coordinates[] frontCoordinates = new Coordinates[] {
                new Coordinates(minX, maxY, maxZ),
                new Coordinates(minX, minY, maxZ),
                new Coordinates(maxX, minY, maxZ),
                new Coordinates(maxX, maxY, maxZ),
            };
            Coordinates[] backCoordinates = new Coordinates[] {
                new Coordinates(minX, maxY, minZ),
                new Coordinates(minX, minY, minZ),
                new Coordinates(maxX, minY, minZ),
                new Coordinates(maxX, maxY, minZ),
            };

            BeginMode drawMode = BeginMode.LineStrip;

            // Front
            GL.Begin(drawMode);
            DrawVertex(frontCoordinates[0]);
            DrawVertex(frontCoordinates[1]);
            DrawVertex(frontCoordinates[3]);
            DrawVertex(frontCoordinates[0]);
            GL.End();
            GL.Begin(drawMode);
            DrawVertex(frontCoordinates[3]);
            DrawVertex(frontCoordinates[1]);
            DrawVertex(frontCoordinates[2]);
            DrawVertex(frontCoordinates[3]);
            GL.End();
            // Back
            GL.Begin(drawMode);
            DrawVertex(backCoordinates[3]);
            DrawVertex(backCoordinates[2]);
            DrawVertex(backCoordinates[0]);
            DrawVertex(backCoordinates[3]);
            GL.End();
            GL.Begin(drawMode);
            DrawVertex(backCoordinates[0]);
            DrawVertex(backCoordinates[2]);
            DrawVertex(backCoordinates[1]);
            DrawVertex(backCoordinates[0]);
            GL.End();
            // Left
            GL.Begin(drawMode);
            DrawVertex(frontCoordinates[0]);
            DrawVertex(backCoordinates[0]);
            DrawVertex(backCoordinates[1]);
            DrawVertex(frontCoordinates[0]);
            GL.End();
            GL.Begin(drawMode);
            DrawVertex(frontCoordinates[0]);
            DrawVertex(backCoordinates[1]);
            DrawVertex(frontCoordinates[1]);
            DrawVertex(frontCoordinates[0]);
            GL.End();
            // Up
            GL.Begin(drawMode);
            DrawVertex(backCoordinates[3]);
            DrawVertex(backCoordinates[0]);
            DrawVertex(frontCoordinates[0]);
            DrawVertex(backCoordinates[3]);
            GL.End();
            GL.Begin(drawMode);
            DrawVertex(backCoordinates[3]);
            DrawVertex(frontCoordinates[0]);
            DrawVertex(frontCoordinates[3]);
            DrawVertex(backCoordinates[3]);
            GL.End();
            // Right
            GL.Begin(drawMode);
            DrawVertex(backCoordinates[3]);
            DrawVertex(frontCoordinates[3]);
            DrawVertex(frontCoordinates[2]);
            DrawVertex(backCoordinates[3]);
            GL.End();
            GL.Begin(drawMode);
            DrawVertex(backCoordinates[3]);
            DrawVertex(frontCoordinates[2]);
            DrawVertex(backCoordinates[2]);
            DrawVertex(backCoordinates[3]);
            GL.End();
            // Down
            GL.Begin(drawMode);
            DrawVertex(frontCoordinates[2]);
            DrawVertex(frontCoordinates[1]);
            DrawVertex(backCoordinates[1]);
            DrawVertex(frontCoordinates[2]);
            GL.End();
            GL.Begin(drawMode);
            DrawVertex(frontCoordinates[2]);
            DrawVertex(backCoordinates[1]);
            DrawVertex(backCoordinates[2]);
            DrawVertex(frontCoordinates[2]);
            GL.End();
        }

        private void DrawVertex(Coordinates coord)
        {
            GL.Vertex3(coord.X, coord.Y, coord.Z);
        }

        protected void LoadScene(Scene scene)
        {
            _loadedScene = scene;
            _visualWindow.Width = _loadedScene.WindowWidth;
            _visualWindow.Height = _loadedScene.WindowHeight;
        }

        protected void LoadDefaultScene()
        {
            LoadScene(new Scene()
            {
                WindowWidth = 400,
                WindowHeight = 400,
                WorldLeft = 0,
                WorldRight = 100,
                WorldBottom = 0,
                WorldTop = 100,
                WorldBack = 0,
                WorldForward = 100,
                Parallelepipeds = new List<Parallelepiped>() {
                    new Parallelepiped() {
                        FirstCoordinates = new Coordinates(25, 25, 25),
                        SecondCoordinates = new Coordinates(75, 75, 75)
                    }
                }
            });
        }
    }
}
