using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

/// <summary>
/// A utility class for managing time in a game or graphics application.
/// </summary>
public static class Time
{
    /// <summary>
    /// The time in seconds since the last time this class was updated.
    /// </summary>
    private static double _lastTime;

    /// <summary>
    /// A dictionary of the last updated times for each GameWindow.
    /// </summary>
    private static Dictionary<GameWindow, double> _windowLastTimes = new Dictionary<GameWindow, double>();

    /// <summary>
    /// The time in seconds since the last time this class was updated.
    /// </summary>
    public static double DeltaTime { get; private set; }

    /// <summary>
    /// The current time in seconds since the application started running.
    /// </summary>
    public static double CurrentTime { get; private set; }

    /// <summary>
    /// Initializes the Time class assigning the GLFW current time as the last updated time.
    /// </summary>
    public static void Initialize()
    {
        _lastTime = GLFW.GetTime();
    }

    /// <summary>
    /// Updates the Time class by calculating the time difference since the last update.
    /// </summary>
    public static void Update()
    {
        var currentTime = GLFW.GetTime();
        DeltaTime = currentTime - _lastTime;
        _lastTime = currentTime;
        CurrentTime = currentTime;
    }

    /// <summary>
    /// Updates the last updated time for the specific GameWindow by calculating the time difference since the last update for that window.
    /// </summary>
    /// <param name="window">The GameWindow to update its last updated time.</param>
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