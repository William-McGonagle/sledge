using OpenTK.Graphics;
using OpenTK.Mathematics;
using System;

public class UIBox : UIElement
{
    public Vector4 color;

    // Constructor
    public UIBox()
    {
        color = new Vector4(1, 1, 1, 1);
    }

    public UIBox(Vector4 _color)
    {
        color = _color;
    }

}
