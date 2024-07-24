using System;
using System.ComponentModel;
using AiForms.Effects.Extensions;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using UIKit;

namespace AiForms.Effects;

public class FloatingEffect: PlatformEffect
{
    Page? _page;
    UIView? _nativePage;
    FloatingLayout? _formsLayout;
    Action? OnceInitializeAction;
    UIDeviceOrientation? _previousOrientation;

    protected override void OnAttached()
    {
        if (Element is not Page page) return;

        OnceInitializeAction = Initialize;

        _formsLayout = Floating.GetContent(Element);
        _formsLayout.Parent = Element;

        _page = page;
        _page.SizeChanged += PageSizeChanged;
        _page.LayoutChanged += PageLayoutChanged;
    }

    protected override void OnDetached()
    {
        if(_page == null || _formsLayout is null)
        {
            return;
        }
        _page.SizeChanged -= PageSizeChanged;
        _page.LayoutChanged -= PageLayoutChanged;
        _formsLayout.Parent = null;

        foreach (var child in _formsLayout)
        {
            child.DisposeModalAndChildHandlers();
        }
        _formsLayout.DisposeModalAndChildHandlers();
        _formsLayout = null;
        _nativePage = null;
        _page = null;

        System.Diagnostics.Debug.WriteLine($"Detached {GetType().Name} from {Element.GetType().FullName}");
    }

    void PageLayoutChanged(object? sender, EventArgs e)
    {
        if (OnceInitializeAction == null && _previousOrientation != UIDevice.CurrentDevice.Orientation && _nativePage is not null)
        {            
            _formsLayout?.Layout(_nativePage.Bounds.ToRectangle());
            _formsLayout?.LayoutChildren();
        }
        _previousOrientation = UIDevice.CurrentDevice.Orientation;
    }


    void PageSizeChanged(object? sender, EventArgs e)
    {
        OnceInitializeAction?.Invoke();
    }

    protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
    {
        base.OnElementPropertyChanged(args);
    }

    void Initialize()
    {
        OnceInitializeAction = null;

        _nativePage = Container;

        if(_formsLayout is null)
        {
            return;
        }

        _formsLayout.Layout(_nativePage.Bounds.ToRectangle());

        foreach (var child in _formsLayout)
        {
            child.IsVisible = FloatingLayout.GetIsVisible(child);
            var renderer = child.ToHandler(child.FindMauiContext()!);
            _formsLayout.LayoutChild(child);            
            SetLayoutAlignment(renderer.PlatformView!, _nativePage, child);
        }

        _nativePage.SetNeedsUpdateConstraints();
        _nativePage.SetNeedsLayout();
    }

    internal static void SetLayoutAlignment(UIView targetView, UIView parentView, View floating)
    {
        targetView.TranslatesAutoresizingMaskIntoConstraints = false;
        parentView.AddSubview(targetView);

        if (floating.HorizontalOptions.Alignment != LayoutAlignment.Fill)
        {
            targetView.WidthAnchor.ConstraintEqualTo((nfloat)floating.Bounds.Width).Active = true;
        }
        if (floating.VerticalOptions.Alignment != LayoutAlignment.Fill)
        {
            targetView.HeightAnchor.ConstraintEqualTo((nfloat)floating.Bounds.Height).Active = true;
        }

        var offsetX = (nfloat)FloatingLayout.GetOffsetX(floating);
        var offsetY = (nfloat)FloatingLayout.GetOffsetY(floating);

        switch (floating.VerticalOptions.Alignment)
        {
            case LayoutAlignment.Start:
                targetView.TopAnchor.ConstraintEqualTo(parentView.TopAnchor, offsetY).Active = true;
                break;
            case LayoutAlignment.End:
                targetView.BottomAnchor.ConstraintEqualTo(parentView.BottomAnchor, offsetY).Active = true;
                break;
            case LayoutAlignment.Center:
                targetView.CenterYAnchor.ConstraintEqualTo(parentView.CenterYAnchor, offsetY).Active = true;
                break;
            default:
                targetView.HeightAnchor.ConstraintEqualTo(parentView.HeightAnchor).Active = true;
                break;
        }

        switch (floating.HorizontalOptions.Alignment)
        {
            case LayoutAlignment.Start:
                targetView.LeftAnchor.ConstraintEqualTo(parentView.LeftAnchor, offsetX).Active = true;
                break;
            case LayoutAlignment.End:
                targetView.RightAnchor.ConstraintEqualTo(parentView.RightAnchor, offsetX).Active = true;
                break;
            case LayoutAlignment.Center:
                targetView.CenterXAnchor.ConstraintEqualTo(parentView.CenterXAnchor, offsetX).Active = true;
                break;
            default:
                targetView.WidthAnchor.ConstraintEqualTo(parentView.WidthAnchor).Active = true;
                break;
        }
    }
}

