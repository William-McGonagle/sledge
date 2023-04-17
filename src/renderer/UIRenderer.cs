using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

public class UIRenderer
{
    private int vertexArrayObject;
    private int vertexBufferObject;
    private int elementBufferObject;
    private int shaderProgram;
    private Matrix4 projectionMatrix;

    public UIRenderer(int screenWidth, int screenHeight)
    {
        // Create Vertex Array Object
        vertexArrayObject = GL.GenVertexArray();

        // Create Vertex Buffer Object
        vertexBufferObject = GL.GenBuffer();

        // Create Element Buffer Object
        elementBufferObject = GL.GenBuffer();

        // Load shaders and create shader program
        shaderProgram = CreateShaderProgram();

        // Set up projection matrix
        projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, screenWidth, screenHeight, 0, -100, 1);
    }

    public void Resize(int screenWidth, int screenHeight)
    {

        // Resize the projection matrix
        projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, screenWidth, screenHeight, 0, -100, 1);

    }

    public void DrawRectangle(int x, int y, int width, int height, Vector4 color)
    {
        // Define the four vertices of the rectangle
        Vector2[] vertices = new Vector2[]
        {
            new Vector2(x, y),
            new Vector2(x + width, y),
            new Vector2(x + width, y + height),
            new Vector2(x, y + height)
        };

        // Define the indices of the two triangles that make up the rectangle
        int[] indices = new int[]
        {
            0, 1, 2,
            2, 3, 0
        };

        // Bind the vertex array object
        GL.BindVertexArray(vertexArrayObject);

        // Bind the vertex buffer object and upload the vertices to the GPU
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * Vector2.SizeInBytes, vertices, BufferUsageHint.StaticDraw);

        // Bind the element buffer object and upload the indices to the GPU
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);

        // Use the shader program
        GL.UseProgram(shaderProgram);

        // Set the color uniform in the shader program
        int colorLocation = GL.GetUniformLocation(shaderProgram, "color");
        GL.Uniform4(colorLocation, color);

        // Set the vertex attribute pointers in the shader program
        int positionLocation = GL.GetAttribLocation(shaderProgram, "position");
        GL.EnableVertexAttribArray(positionLocation);
        GL.VertexAttribPointer(positionLocation, 2, VertexAttribPointerType.Float, false, Vector2.SizeInBytes, 0);

        // Set the projection matrix uniform in the shader program
        int projectionLocation = GL.GetUniformLocation(shaderProgram, "projection");
        GL.UniformMatrix4(projectionLocation, false, ref projectionMatrix);

        // Draw the rectangle
        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

        // Disable the vertex attribute pointers
        GL.DisableVertexAttribArray(positionLocation);
    }

    private int CreateShaderProgram()
    {
        string vertexShaderSource = @"
            #version 330 core

            layout (location = 0) in vec2 position;

            uniform mat4 projection;
            
            void main()
            {
                gl_Position = projection * vec4(position, 0.0, 1.0);
            }
        ";

        string fragmentShaderSource = @"
            #version 330 core

            uniform vec4 color;

            out vec4 fragColor;

            void main()
            {
                fragColor = color;
            }
        ";

        // Create vertex shader
        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSource);
        GL.CompileShader(vertexShader);

        // Check for errors in vertex shader
        string infoLogVert;
        GL.GetShaderInfoLog(vertexShader, out infoLogVert);
        if (!string.IsNullOrWhiteSpace(infoLogVert))
        {
            throw new ApplicationException($"Error compiling vertex shader: {infoLogVert}");
        }

        // Create fragment shader
        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);
        GL.CompileShader(fragmentShader);

        // Check for errors in fragment shader
        string infoLogFrag;
        GL.GetShaderInfoLog(fragmentShader, out infoLogFrag);
        if (!string.IsNullOrWhiteSpace(infoLogFrag))
        {
            throw new ApplicationException($"Error compiling fragment shader: {infoLogFrag}");
        }

        // Create shader program
        int shaderProgram = GL.CreateProgram();
        GL.AttachShader(shaderProgram, vertexShader);
        GL.AttachShader(shaderProgram, fragmentShader);
        GL.LinkProgram(shaderProgram);

        // Check for errors in linking
        string infoLogLink;
        GL.GetProgramInfoLog(shaderProgram, out infoLogLink);
        if (!string.IsNullOrWhiteSpace(infoLogLink))
        {
            throw new ApplicationException($"Error linking shader program: {infoLogLink}");
        }

        // Delete the individual shaders
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        return shaderProgram;
    }

    public void Dispose()
    {
        GL.DeleteBuffer(vertexBufferObject);
        GL.DeleteBuffer(elementBufferObject);
        GL.DeleteVertexArray(vertexArrayObject);
        GL.DeleteProgram(shaderProgram);
    }
}