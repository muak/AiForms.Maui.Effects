using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Maui;
using Microsoft.Maui.Platform;
using System.ComponentModel;

namespace AiForms.Effects;

/// <summary>
/// Floating layout.
/// </summary>
public class FloatingLayout :View, IList<View>
{
    public static readonly BindableProperty OffsetXProperty =
    BindableProperty.CreateAttached(
            "OffsetX",
            typeof(double),
            typeof(FloatingLayout),
            default(double)
        );

    public static void SetOffsetX(BindableObject view, double value)
    {
        view.SetValue(OffsetXProperty, value);
    }

    public static double GetOffsetX(BindableObject view)
    {
        return (double)view.GetValue(OffsetXProperty);
    }

    public static readonly BindableProperty OffsetYProperty =
    BindableProperty.CreateAttached(
            "OffsetY",
            typeof(double),
            typeof(FloatingLayout),
            default(double)
        );

    public static void SetOffsetY(BindableObject view, double value)
    {
        view.SetValue(OffsetYProperty, value);
    }

    public static double GetOffsetY(BindableObject view)
    {
        return (double)view.GetValue(OffsetYProperty);
    }

    public static new readonly BindableProperty IsVisibleProperty =
    BindableProperty.CreateAttached(
            "IsVisible",
            typeof(bool),
            typeof(FloatingLayout),
            true,            
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                if(bindable is not VisualElement element)
                {
                    return;
                }
                if(element.Handler is not IPlatformViewHandler handler)
                {
                    return;
                }
                if(handler.PlatformView is null)
                {
                    return;
                }

                var isVisible = (bool)newValue;

#if IOS
                element.IsVisible = isVisible;
#endif
#if ANDROID
                handler.PlatformView!.Alpha = isVisible ? (float)element.Opacity : 0;
                element.InputTransparent = !isVisible;                
#endif
            }            
        );

    public static void SetIsVisible(BindableObject view, bool value)
    {
        view.SetValue(IsVisibleProperty, value);
    }

    public static bool GetIsVisible(BindableObject view)
    {
        return (bool)view.GetValue(IsVisibleProperty);
    }   

    

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AiForms.Effects.FloatingLayout"/> class.
    /// </summary>
    public FloatingLayout()
    {
    }    

    /// <summary>
    /// Ons the parent set.
    /// </summary>
    protected override void OnParentSet()
    {
        base.OnParentSet();

        foreach (var child in _children)
        {
            child.Parent = Parent;
        }
    }

    /// <summary>
    /// Layouts the children.
    /// </summary>
    public void LayoutChildren()
    {
        foreach(var child in _children)
        {
            LayoutChild(child);
        }
    }    

    /// <summary>
    /// Layouts the child.
    /// </summary>
    /// <param name="child">Child.</param>
    public void LayoutChild(View child)
    {
        var sizeRequest = child.Measure(Width, Height);                

        var finalW = child.HorizontalOptions.Alignment == LayoutAlignment.Fill ? Width : sizeRequest.Request.Width;
        var finalH = child.VerticalOptions.Alignment == LayoutAlignment.Fill ? Height : sizeRequest.Request.Height;

        child.Layout(new Rect(0, 0, finalW,finalH));        
    }

    ObservableCollection<View> _children = new ObservableCollection<View>();

    /// <summary>
    /// Gets or sets the <see cref="T:AiForms.Effects.FloatingLayout"/> at the specified index.
    /// </summary>
    /// <param name="index">Index.</param>
    public View this[int index] {
        get { return _children[index]; }
        set { _children[index] = value; }
    }

    /// <summary>
    /// Gets the count.
    /// </summary>
    /// <value>The count.</value>
    public int Count => _children.Count;

    /// <summary>
    /// Gets a value indicating whether this <see cref="T:AiForms.Effects.FloatingLayout"/> is read only.
    /// </summary>
    /// <value><c>true</c> if is read only; otherwise, <c>false</c>.</value>
    public bool IsReadOnly => false;

    /// <summary>
    /// Add the specified item.
    /// </summary>
    /// <param name="item">Item.</param>
    public void Add(View item)
    {
        // On Android, MauiView does not function properly unless the parent is a page.
        item.Parent = this;
        _children.Add(item);
    }

    /// <summary>
    /// Clear this instance.
    /// </summary>
    public void Clear()
    {
        _children.Clear();
    }

    /// <summary>
    /// Contains the specified item.
    /// </summary>
    /// <returns>The contains.</returns>
    /// <param name="item">Item.</param>
    public bool Contains(View item)
    {
        return _children.Contains(item);
    }

    /// <summary>
    /// Copies to.
    /// </summary>
    /// <param name="array">Array.</param>
    /// <param name="arrayIndex">Array index.</param>
    public void CopyTo(View[] array, int arrayIndex)
    {
        _children.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>The enumerator.</returns>
    public IEnumerator<View> GetEnumerator()
    {
        return _children.GetEnumerator();
    }

    /// <summary>
    /// Indexs the of.
    /// </summary>
    /// <returns>The of.</returns>
    /// <param name="item">Item.</param>
    public int IndexOf(View item)
    {
        return _children.IndexOf(item);
    }

    /// <summary>
    /// Insert the specified index and item.
    /// </summary>
    /// <param name="index">Index.</param>
    /// <param name="item">Item.</param>
    public void Insert(int index, View item)
    {
        item.Parent = Parent;
        _children.Insert(index, item);
    }

    /// <summary>
    /// Remove the specified item.
    /// </summary>
    /// <returns>The remove.</returns>
    /// <param name="item">Item.</param>
    public bool Remove(View item)
    {
        item.Parent = Parent;
        return _children.Remove(item);
    }

    /// <summary>
    /// Removes at index.
    /// </summary>
    /// <param name="index">Index.</param>
    public void RemoveAt(int index)
    {
        _children[index].Parent = null;
        _children.RemoveAt(index);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _children.GetEnumerator();
    }   

}
