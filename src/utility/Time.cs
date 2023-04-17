using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

public static class Time
{
    private static double _lastTime;
    private static Dictionary<GameWindow, double> _windowLastTimes = new Dictionary<GameWindow, double>();

    public static double DeltaTime { get; private set; }

    public static double CurrentTime { get; private set; }

    public static void Initialize()
    {
        _lastTime = GLFW.GetTime();
    }

    public static void Update()
    {
        var currentTime = GLFW.GetTime();
        DeltaTime = currentTime - _lastTime;
        _lastTime = currentTime;
        CurrentTime = currentTime;
    }

    public static void Update(GameWindow window)
    {
        if (!_windowLastTimes.ContainsKey(window))
        {
            _windowLastTimes.Add(window, GLFW.GetTime());
        }

        var lastTime = _windowLastTimes[window];
        var currentTime = GLFW.GetTime();
        DeltaTime = currentTime - lastTime;
        _windowLastTimes[window] = currentTime;
        CurrentTime = currentTime;
    }
}