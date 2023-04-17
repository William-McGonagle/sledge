using System.Collections.Generic;
using OpenTK.Mathematics;

public class UIElement
{
    // Position and size of the element in screen coordinates
    private Vector2 _position;
    private Vector2 _size;

    public Vector2 Position
    {
        get => _position;
        set
        {
            _position = value;
            Update();
        }
    }

    public Vector2 Size
    {
        get => _size;
        set
        {
            _size = value;
            Update();
        }
    }

    // Color of the element
    public Color4 Color { get; set; }

    // Parent and child elements in the hierarchy
    private UIElement _parent;
    private List<UIElement> _children;

    public UIElement Parent
    {
        get => _parent;
        set
        {
            _parent = value;
            Update();
        }
    }

    public List<UIElement> Children
    {
        get => _children;
        set
        {
            _children = value;
            Update();
        }
    }

    // Constraints applied to the element
    private List<UIConstraint> _constraints;

    public UIElement()
    {
        _position = Vector2.Zero;
        _size = Vector2.Zero;
        Color = Color4.White;
        _parent = null;
        _children = new List<UIElement>();
        _constraints = new List<UIConstraint>();
    }

    public void AddChild(UIElement child)
    {
        child.Parent = this;
        Children.Add(child);
    }

    public void RemoveChild(UIElement child)
    {
        child.Parent = null;
        Children.Remove(child);
    }

    public bool ContainsPoint(Vector2 point)
    {
        return point.X >= Position.X && point.X <= Position.X + Size.X &&
               point.Y >= Position.Y && point.Y <= Position.Y + Size.Y;
    }

    public void AddConstraint(UIConstraint constraint)
    {
        constraint.AddDependentElement(this);
        _constraints.Add(constraint);
    }

    public void RemoveConstraint(UIConstraint constraint)
    {
        constraint.RemoveDependentElement(this);
        _constraints.Remove(constraint);
    }

    public void Update()
    {
        // Apply constraints
        foreach (UIConstraint constraint in _constraints)
        {
            constraint.Update();
        }

        // Update child positions
        foreach (UIElement child in Children)
        {
            child.Position = Position + child.Position;
        }
    }

    public void Render(UIRenderer renderer)
    {
        // Override this method in derived classes to render the element
        // For example, to add vertices to the renderer's vertex buffer
    }
}
