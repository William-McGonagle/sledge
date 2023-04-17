using System.Collections.Generic;

public abstract class UIConstraint
{
    protected List<UIElement> DependentElements;

    public UIConstraint()
    {
        DependentElements = new List<UIElement>();
    }

    public void AddDependentElement(UIElement element)
    {
        DependentElements.Add(element);
    }

    public void RemoveDependentElement(UIElement element)
    {
        DependentElements.Remove(element);
    }

    public abstract void Update();

}