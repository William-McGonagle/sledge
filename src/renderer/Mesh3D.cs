using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

public class Mesh3D
{
    private int vaoHandle;
    private int vboHandle;
    private int iboHandle;
    private int indexCount;

    private ShaderProgram shaderProgram;
    private Material material;

    private Matrix4 modelMatrix = Matrix4.Identity;

    public Mesh3D()
    {



    }

    public Mesh3D(Vector3[] vertices, int[] indices, ShaderProgram shaderProgram, Material material)
    {
        this.shaderProgram = shaderProgram;
        this.material = material;

        // Generate and bind VAO
        GL.GenVertexArrays(1, out vaoHandle);
        GL.BindVertexArray(vaoHandle);


        // Generate and bind VBO
        GL.GenBuffers(1, out vboHandle);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandle);

        // Upload vertex data to VBO
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float) * 3, vertices, BufferUsageHint.StaticDraw);

        // return;

        // Define vertex attribute pointers
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
        GL.EnableVertexAttribArray(0);

        // Generate and bind IBO
        GL.GenBuffers(1, out iboHandle);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, iboHandle);

        // Upload index data to IBO
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);

        // Store index count
        indexCount = indices.Length;

    }

    public void RenderWireframe(Matrix4 viewMatrix, Matrix4 projectionMatrix)
    {
        // Bind VAO
        GL.BindVertexArray(vaoHandle);
        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

        // Bind shader program
        GL.UseProgram(shaderProgram.ProgramHandle);

        // Set uniform variables
        shaderProgram.SetUniform("model", modelMatrix);
        shaderProgram.SetUniform("view", viewMatrix);
        shaderProgram.SetUniform("projection", projectionMatrix);
        shaderProgram.SetUniform("color", material.Color);

        // Draw mesh
        GL.DrawElements(BeginMode.Triangles, indexCount, DrawElementsType.UnsignedInt, 0);
    }

    public void Render(Matrix4 viewMatrix, Matrix4 projectionMatrix)
    {
        // Bind VAO
        GL.BindVertexArray(vaoHandle);
        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

        // Bind shader program
        GL.UseProgram(shaderProgram.ProgramHandle);

        // Set uniform variables
        shaderProgram.SetUniform("model", modelMatrix);
        shaderProgram.SetUniform("view", viewMatrix);
        shaderProgram.SetUniform("projection", projectionMatrix);
        shaderProgram.SetUniform("color", material.Color);

        // Draw mesh
        GL.DrawElements(BeginMode.Triangles, indexCount, DrawElementsType.UnsignedInt, 0);
    }

    public void RenderToBuffer(int fboHandle, Matrix4 viewMatrix, Matrix4 projectionMatrix)
    {
        // Bind framebuffer and shader program
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, fboHandle);
        GL.UseProgram(shaderProgram.ProgramHandle);

        // Bind VAO
        GL.BindVertexArray(vaoHandle);
        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

        // Set uniform variables
        shaderProgram.SetUniform("model", modelMatrix);
        shaderProgram.SetUniform("view", viewMatrix);
        shaderProgram.SetUniform("projection", projectionMatrix);
        shaderProgram.SetUniform("color", material.Color);

        // Draw mesh
        GL.DrawElements(BeginMode.Triangles, indexCount, DrawElementsType.UnsignedInt, 0);
    }

    public Matrix4 ModelMatrix
    {
        get { return modelMatrix; }
        set { modelMatrix = value; }
    }

    #region Static Functions

    public static Mesh3D LoadObj(string path, ShaderProgram shaderProgram, Material material)
    {
        var mesh = new Mesh3D();
        mesh.shaderProgram = shaderProgram;
        mesh.material = material;

        var vertices = new List<Vector3>();
        var normals = new List<Vector3>();
        var texCoords = new List<Vector2>();
        var indices = new List<uint>();

        using (var streamReader = new StreamReader(path))
        {
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                if (line == null)
                {
                    continue;
                }

                var tokens = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 0)
                {
                    continue;
                }

                switch (tokens[0])
                {
                    case "v":
                        var vertex = new Vector3(
                            float.Parse(tokens[1], CultureInfo.InvariantCulture),
                            float.Parse(tokens[2], CultureInfo.InvariantCulture),
                            float.Parse(tokens[3], CultureInfo.InvariantCulture));
                        vertices.Add(vertex);
                        break;

                    case "vn":
                        var normal = new Vector3(
                            float.Parse(tokens[1], CultureInfo.InvariantCulture),
                            float.Parse(tokens[2], CultureInfo.InvariantCulture),
                            float.Parse(tokens[3], CultureInfo.InvariantCulture));
                        normals.Add(normal);
                        break;

                    case "vt":
                        var texCoord = new Vector2(
                            float.Parse(tokens[1], CultureInfo.InvariantCulture),
                            float.Parse(tokens[2], CultureInfo.InvariantCulture));
                        texCoords.Add(texCoord);
                        break;

                    case "f":
                        for (int i = 1; i < tokens.Length; i++)
                        {
                            var faceToken = tokens[i];
                            var faceIndices = faceToken.Split('/')
                                .Select(x => string.IsNullOrEmpty(x) ? 0 : uint.Parse(x))
                                .ToArray();

                            if (faceIndices.Length < 3)
                            {
                                /// Face token doesn't contain enough indices, we thus need to skip
                                continue;
                            }
                            if (faceIndices.Length > 3)
                            {
                                /// Face token contains extra indices, only use the first three (working on triangles, OpenGl4)
                                faceIndices = faceIndices.Take(3).ToArray();
                            }
                            var vertexIndex = faceIndices[0] - 1;
                            var normalIndex = faceIndices[2] - 1;
                            if (vertexIndex < 0 || vertexIndex >= vertices.Count ||
                                normalIndex < 0 || normalIndex >= normals.Count)
                            {
                                /// Indices are out of range, skip the face
                                continue;
                            }
                            indices.Add((uint)vertexIndex);
                            indices.Add((uint)normalIndex);

                            /// Add indices for the other primitive types as well
                            if (faceIndices.Length >= 2 && faceIndices[1] > 0)
                            {
                                var texCoordIndex = faceIndices[1] - 1;
                                if (texCoordIndex < 0 || texCoordIndex >= texCoords.Count)
                                {
                                    /// Texture coordinate index is out of range, skip it
                                    continue;
                                }
                                indices.Add((uint)texCoordIndex);
                            }
                        }
                        break;
                }
            }
        }

        mesh.indexCount = indices.Count;

        GL.GenVertexArrays(1, out mesh.vaoHandle);
        GL.GenBuffers(1, out mesh.vboHandle);
        GL.GenBuffers(1, out mesh.iboHandle);

        GL.BindVertexArray(mesh.vaoHandle);

        GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.vboHandle);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * sizeof(float) * 3, vertices.ToArray(), BufferUsageHint.StaticDraw);
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.iboHandle);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(uint), indices.ToArray(), BufferUsageHint.StaticDraw);

        GL.BindVertexArray(0);

        return mesh;
    }

    #endregion
}