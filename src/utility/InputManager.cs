using OpenTK.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

/// <summary>
/// This class manages the inputs happening in each GameWindow.
/// </summary>
public static class InputManager
{
    /// <summary>
    /// The keyboard states for each GameWindow.
    /// </summary>
    private static KeyboardState[] keyboardStates;

    /// <summary>
    /// The mouse states for each GameWindow.
    /// </summary>
    private static MouseState[] mouseStates;

    /// <summary>
    /// Initializes the manager inputs for the number of GameWindows specified.
    /// </summary>
    /// <param name="numWindows">The number of GameWindows to manage.</param>
    public static void Init(int numWindows)
    {
        keyboardStates = new KeyboardState[numWindows];
        mouseStates = new MouseState[numWindows];
    }

    /// <summary>
    /// Updates the input manager reading the inputs happening on a specified GameWindow.<br/>
    /// This method must be called once for each GameWindow running in the application every Update Frame.
    /// </summary>
    /// <param name="window">The GameWindow to update inputs.</param>
    public static void Update(GameWindow window)
    {
        int index = window.Context.SwapInterval;
        keyboardStates[index] = window.KeyboardState;
        mouseStates[index] = window.MouseState;
    }

    /// <summary>
    /// Returns true while the specified key is currently pressed on the selected GameWindow.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <param name="windowIndex">The GameWindow to check the key in.</param>
    public static bool IsKeyDown(Keys key, int windowIndex = 0)
    {
        return keyboardStates[windowIndex].IsKeyDown(key);
    }

    /// <summary>
    /// Returns true if the specified key has been released on the selected GameWindow.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <param name="windowIndex">The window to check the key in.</param>
    public static bool IsKeyUp(Keys key, int windowIndex = 0)
    {
        return keyboardStates[windowIndex].IsKeyReleased(key);
    }

    /// <summary>
    /// Returns true while the specified mouse button is pressed for the selected GameWindow.
    /// </summary>
    /// <param name="button">The mouse button to check.</param>
    /// <param name="windowIndex">The GameWindow to check the mouse button in.</param>
    public static bool IsMouseDown(MouseButton button, int windowIndex = 0)
    {
        return mouseStates[windowIndex].IsButtonDown(button);
    }

    /// <summary>
    /// Returns the current X coordinate of the mouse cursor in the GameWindow.
    /// </summary>
    /// <param name="windowIndex">The GameWindow to check.</param>
    public static float GetMouseX(int windowIndex = 0)
    {
        return mouseStates[windowIndex].X;
    }

    /// <summary>
    /// Returns the current Y coordinate of the mouse cursor in the GameWindow.
    /// </summary>
    /// <param name="windowIndex">The GameWindow to check.</param>
    public static float GetMouseY(int windowIndex = 0)
    {
        return mouseStates[windowIndex].Y;
    }

    /// <summary>
    /// Obtains the mouse X coordinate difference to the last frame in the GameWindow.
    /// </summary>
    /// <param name="windowIndex">The GameWindow to check.</param>
    /// <returns>The mouse Y delta.</returns>
    public static float GetMouseDeltaX(int windowIndex = 0)
    {
        return mouseStates[windowIndex].Delta.X;
    }

    /// <summary>
    /// Obtains the mouse Y coordinate difference to the last frame in the GameWindow.
    /// </summary>
    /// <param name="windowIndex">The GameWindow to check.</param>
    /// <returns>The mouse Y delta.</returns>
    public static float GetMouseDeltaY(int windowIndex = 0)
    {
        return mouseStates[windowIndex].Delta.Y;
    }
}
