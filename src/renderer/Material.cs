using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

public class Material
{
    public Vector3 Color { get; set; }

    public Material(Vector3 color)
    {
        Color = color;
    }

    public void SetUniforms(int shaderProgramHandle)
    {
        int colorLocation = GL.GetUniformLocation(shaderProgramHandle, "materialColor");
        GL.Uniform3(colorLocation, Color);
    }
}