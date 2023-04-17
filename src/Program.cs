using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;

namespace Sledge
{
    class Program
    {

        static void Main(string[] args)
        {

            // PluginManager.UsePlugins();

            var windowSettings = new GameWindowSettings
            {
                IsMultiThreaded = false
            };

            var nativeWindowSettings = new NativeWindowSettings
            {
                Title = "Sledge UI Renderer",
                Size = new OpenTK.Mathematics.Vector2i(800, 600),
                APIVersion = new Version(4, 1),
                Flags = ContextFlags.ForwardCompatible
            };

            // Initialize OpenTK window and OpenGL context
            using (var gameWindow = new GameWindow(windowSettings, nativeWindowSettings))
            {
                gameWindow.Load += () =>
                {

                    InputManager.Init(1);

                    UIRenderer uiRenderer = new UIRenderer(gameWindow.Size.X, gameWindow.Size.Y);

                    // Set clear color
                    GL.ClearColor(0.2f, 0.3f, 0.4f, 1.0f);

                    // Enable depth testing
                    GL.Enable(EnableCap.DepthTest);

                    // Define vertex and index data for cube
                    Vector3[] cubeVertices = new Vector3[]
                    {
                                new Vector3(-0.5f, -0.5f, -0.5f),
                                new Vector3(0.5f, -0.5f, -0.5f),
                                new Vector3(0.5f, 0.5f, -0.5f),
                                new Vector3(-0.5f, 0.5f, -0.5f),
                                new Vector3(-0.5f, -0.5f, 0.5f),
                                new Vector3(0.5f, -0.5f, 0.5f),
                                new Vector3(0.5f, 0.5f, 0.5f),
                                new Vector3(-0.5f, 0.5f, 0.5f),
                    };
                    int[] cubeIndices = new int[]
                    {
                                0, 1, 2, 2, 3, 0, // front
                                1, 5, 6, 6, 2, 1, // right
                                5, 4, 7, 7, 6, 5, // back
                                4, 0, 3, 3, 7, 4, // left
                                3, 2, 6, 6, 7, 3, // top
                                4, 5, 1, 1, 0, 4, // bottom
                    };

                    // Define material properties for cube
                    Material cubeMaterial = new Material(new Vector3(1.0f, 0.0f, 0.0f)); // red color

                    // Define vertex and fragment shader sources for cube
                    string vertexShaderSource = @"
                                #version 330 core

                                layout (location = 0) in vec3 position;

                                uniform mat4 model;
                                uniform mat4 view;
                                uniform mat4 projection;

                                void main()
                                {
                                    gl_Position = projection * view * model * vec4(position, 1.0);
                                }";
                    string fragmentShaderSource = @"
                                #version 330 core

                                uniform vec3 color;

                                out vec4 fragColor;

                                void main()
                                {
                                    fragColor = vec4(color, 1.0);
                                }";

                    // Create shader program for cube
                    ShaderProgram cubeShaderProgram = new ShaderProgram(vertexShaderSource, fragmentShaderSource);

                    // Create mesh for cube with custom shader and material
                    Mesh3D cubeMesh = Mesh3D.LoadObj("./box.obj", cubeShaderProgram, cubeMaterial);
                    Mesh3D sideCubeMesh = new Mesh3D(cubeVertices, cubeIndices, cubeShaderProgram, cubeMaterial);

                    // Set up camera
                    Matrix4 viewMatrix = Matrix4.LookAt(new Vector3(0.0f, 0.0f, 3.0f), Vector3.Zero, Vector3.UnitY);
                    Matrix4 projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)800 / 600, 0.1f, 100.0f);

                    // Set up scene with cube
                    Scene3D scene = new Scene3D();
                    scene.AddMesh(cubeMesh);
                    scene.AddMesh(sideCubeMesh);

                    float time = 0;

                    gameWindow.UpdateFrame += (e) =>
                    {

                        InputManager.Update(gameWindow);

                    };

                    gameWindow.Resize += (e) =>
                    {

                        GL.Viewport(0, 0, gameWindow.Size.X, gameWindow.Size.Y);

                        uiRenderer.Resize(gameWindow.Size.X, gameWindow.Size.Y);

                    };

                    // Set up render function
                    gameWindow.RenderFrame += (sender2) =>
                    {

                        // Clear screen
                        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                        // Update cube rotation
                        time += (float)sender2.Time;
                        cubeMesh.ModelMatrix = Matrix4.CreateRotationY(time);
                        sideCubeMesh.ModelMatrix = Matrix4.CreateTranslation(2, 0, 0);

                        // Render scene
                        cubeMesh.Render(viewMatrix, projectionMatrix);
                        uiRenderer.DrawRectangle(40, 40, 400, 500, new Vector4(0, 1, 0, 1));

                        sideCubeMesh.RenderWireframe(viewMatrix, projectionMatrix);

                        // GL.Viewport(40, 40, 400, 500);

                        scene.Render(viewMatrix, projectionMatrix);

                        // GL.Viewport(0, 0, gameWindow.Size.X * 2, gameWindow.Size.Y * 2);

                        // Swap buffers
                        gameWindow.SwapBuffers();

                    };

                };

                gameWindow.Run();
            }
        }
    }
}