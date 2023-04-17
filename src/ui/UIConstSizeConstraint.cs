using OpenTK.Mathematics;

public class UIConstSizeConstraint : UIConstraint
{

    private Vector2 _size;

    public UIConstSizeConstraint(Vector2 size)
    {
        _size = size;
    }

    public override void Update()
    {
        foreach (UIElement element in DependentElements)
        {
            element.Size = _size;
        }
    }

}