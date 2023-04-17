using OpenTK.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

public static class InputManager
{
    private static KeyboardState[] keyboardStates;
    private static MouseState[] mouseStates;

    public static void Init(int numWindows)
    {
        keyboardStates = new KeyboardState[numWindows];
        mouseStates = new MouseState[numWindows];
    }

    public static void Update(GameWindow window)
    {
        int index = window.Context.SwapInterval;
        keyboardStates[index] = window.KeyboardState;
        mouseStates[index] = window.MouseState;
    }

    public static bool IsKeyDown(Keys key, int windowIndex = 0)
    {
        return keyboardStates[windowIndex].IsKeyDown(key);
    }

    public static bool IsKeyUp(Keys key, int windowIndex = 0)
    {
        return keyboardStates[windowIndex].IsKeyReleased(key);
    }

    public static bool IsMouseDown(MouseButton button, int windowIndex = 0)
    {
        return mouseStates[windowIndex].IsButtonDown(button);
    }

    public static float GetMouseX(int windowIndex = 0)
    {
        return mouseStates[windowIndex].X;
    }

    public static float GetMouseY(int windowIndex = 0)
    {
        return mouseStates[windowIndex].Y;
    }

    public static float GetMouseDeltaX(int windowIndex = 0)
    {
        return mouseStates[windowIndex].Delta.X;
    }

    public static float GetMouseDeltaY(int windowIndex = 0)
    {
        return mouseStates[windowIndex].Delta.Y;
    }
}
