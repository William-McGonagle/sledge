using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

public class Scene3D
{
    private List<Mesh3D> meshes = new List<Mesh3D>();

    public void AddMesh(Mesh3D mesh)
    {
        meshes.Add(mesh);
    }

    public void RemoveMesh(Mesh3D mesh)
    {
        meshes.Remove(mesh);
    }

    // public void SetupRenderToBuffer(Matrix4 viewMatrix, Matrix4 projectionMatrix, int width, int height, out int fboHandle, out int textureHandle)
    // {
    //     // Create framebuffer object
    //     fboHandle = GL.GenFramebuffer();
    //     GL.BindFramebuffer(FramebufferTarget.Framebuffer, fboHandle);

    //     // Create texture for color and attach to framebuffer
    //     textureHandle = GL.GenTexture();
    //     GL.BindTexture(TextureTarget.Texture2D, textureHandle);
    //     GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
    //     GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
    //     GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
    //     GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, textureHandle, 0);

    //     // Create renderbuffer for depth and attach to framebuffer
    //     int depthRboHandle = GL.GenRenderbuffer();
    //     GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthRboHandle);
    //     GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent24, width, height);
    //     GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depthRboHandle);

    //     // Check framebuffer completeness
    //     if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
    //         throw new Exception("Framebuffer is not complete!");

    //     // Set viewport
    //     GL.Viewport(0, 0, width, height);
    // }

    public void Render(Matrix4 viewMatrix, Matrix4 projectionMatrix)
    {
        foreach (Mesh3D mesh in meshes)
        {
            mesh.Render(viewMatrix, projectionMatrix);
        }
    }
}