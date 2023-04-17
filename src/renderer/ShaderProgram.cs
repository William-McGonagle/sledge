using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

public class ShaderProgram : IDisposable
{
    private int programHandle;
    private int vertexShaderHandle;
    private int fragmentShaderHandle;

    public ShaderProgram(string vertexShaderSource, string fragmentShaderSource)
    {
        // Compile vertex shader
        vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShaderHandle, vertexShaderSource);
        GL.CompileShader(vertexShaderHandle);

        // Check vertex shader compilation errors
        int vertexShaderCompileStatus;
        GL.GetShader(vertexShaderHandle, ShaderParameter.CompileStatus, out vertexShaderCompileStatus);
        if (vertexShaderCompileStatus != 1)
        {
            string infoLog = GL.GetShaderInfoLog(vertexShaderHandle);
            throw new ApplicationException("Vertex shader compilation error:\n" + infoLog);
        }

        // Compile fragment shader
        fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShaderHandle, fragmentShaderSource);
        GL.CompileShader(fragmentShaderHandle);

        // Check fragment shader compilation errors
        int fragmentShaderCompileStatus;
        GL.GetShader(fragmentShaderHandle, ShaderParameter.CompileStatus, out fragmentShaderCompileStatus);
        if (fragmentShaderCompileStatus != 1)
        {
            string infoLog = GL.GetShaderInfoLog(fragmentShaderHandle);
            throw new ApplicationException("Fragment shader compilation error:\n" + infoLog);
        }

        // Link shader program
        programHandle = GL.CreateProgram();
        GL.AttachShader(programHandle, vertexShaderHandle);
        GL.AttachShader(programHandle, fragmentShaderHandle);
        GL.LinkProgram(programHandle);

        // Check program link errors
        int programLinkStatus;
        GL.GetProgram(programHandle, GetProgramParameterName.LinkStatus, out programLinkStatus);
        if (programLinkStatus != 1)
        {
            string infoLog = GL.GetProgramInfoLog(programHandle);
            throw new ApplicationException("Program link error:\n" + infoLog);
        }
    }

    public void Use()
    {
        GL.UseProgram(programHandle);
    }

    public void SetUniform(string name, float value)
    {
        int location = GL.GetUniformLocation(programHandle, name);
        GL.Uniform1(location, value);
    }

    public void SetUniform(string name, int value)
    {
        int location = GL.GetUniformLocation(programHandle, name);
        GL.Uniform1(location, value);
    }

    public void SetUniform(string name, Vector3 value)
    {
        int location = GL.GetUniformLocation(programHandle, name);
        GL.Uniform3(location, value);
    }

    public void SetUniform(string name, Matrix4 value)
    {
        int location = GL.GetUniformLocation(programHandle, name);
        GL.UniformMatrix4(location, false, ref value);
    }

    public int ProgramHandle
    {
        get { return programHandle; }
    }

    public void Dispose()
    {
        GL.DeleteShader(vertexShaderHandle);
        GL.DeleteShader(fragmentShaderHandle);
        GL.DeleteProgram(programHandle);
    }
}
