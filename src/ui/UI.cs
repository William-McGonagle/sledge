using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

public static class UI
{

    public static UIRenderer renderer;
    public static Vector4 baseColor = new Vector4(0.7f, 0.7f, 0.7f, 1);
    public static Vector4 hoverColor = new Vector4(0.8f, 0.8f, 0.8f, 1);
    public static Vector4 activeColor = new Vector4(0.85f, 0.85f, 0.85f, 1);

    public static void Init(GameWindow gameWindow)
    {

        renderer = new UIRenderer(gameWindow.Size.X, gameWindow.Size.Y);

    }

    public static void Resize(GameWindow gameWindow)
    {

        renderer.Resize(gameWindow.Size.X, gameWindow.Size.Y);

    }

    public static void Box(Rect rect)
    {

        renderer.DrawRectangle(rect.x, rect.y, rect.width, rect.height, baseColor);

    }

    public static bool RepeatButton(Rect rect, string label)
    {

        if (rect.isPointInside((int)Input.GetMouseX(), (int)Input.GetMouseY()))
        {

            if (Input.IsMouseDown(MouseButton.Left, 0))
            {

                renderer.DrawRectangle(rect.x, rect.y, rect.width, rect.height, activeColor);
                return true;

            }
            else
            {

                renderer.DrawRectangle(rect.x, rect.y, rect.width, rect.height, hoverColor);

            }

        }
        else
        {

            renderer.DrawRectangle(rect.x, rect.y, rect.width, rect.height, baseColor);

        }

        return false;

    }

    public static bool Button(Rect rect, string label)
    {

        if (rect.isPointInside((int)Input.GetMouseX(), (int)Input.GetMouseY()))
        {

            if (Input.IsMouseDown(MouseButton.Left, 0))
            {

                renderer.DrawRectangle(rect.x, rect.y, rect.width, rect.height, activeColor);

            }
            else
            {

                renderer.DrawRectangle(rect.x, rect.y, rect.width, rect.height, hoverColor);

            }

            // if (Input.IsMouseReleased(MouseButton.Left, 0))
            // {

            // return true;

            // }

        }
        else
        {

            renderer.DrawRectangle(rect.x, rect.y, rect.width, rect.height, baseColor);

        }

        return false;

    }

}

public class Rect
{

    public int x;
    public int y;
    public int width;
    public int height;

    public Rect()
    {

        x = 0;
        y = 0;
        width = 0;
        height = 0;

    }

    public Rect(int _x, int _y, int _width, int _height)
    {

        x = _x;
        y = _y;
        width = _width;
        height = _height;

    }

    public bool isPointInside(int _x, int _y)
    {

        if (_x < x) return false;
        if (_y < y) return false;
        if (_x > x + width) return false;
        if (_y > y + height) return false;

        return true;

    }

}