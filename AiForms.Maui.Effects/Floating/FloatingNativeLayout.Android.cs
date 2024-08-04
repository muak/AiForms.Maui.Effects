using AiForms.Effects.Extensions;
using Android.Content;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using MauiView = Microsoft.Maui.Controls.View;

namespace AiForms.Effects;

public class FloatingNativeLayout : FrameLayout
{
    Page? _page;
    Action? OnceInitializeAction;
    FloatingLayout? VirtualView;

    public FloatingNativeLayout(Context context, FloatingLayout virtualView) : base(context)
    {
        VirtualView = virtualView;
        SetClipChildren(false);
        SetClipToPadding(false);        

        OnceInitializeAction = Initialize;

        if (VirtualView.Parent is not Page page)
        {
            throw new Exception("Parent page is null");
        }

        _page = page;
        _page.SizeChanged += Page_SizeChanged;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if(_page is not null)
            {
                _page.SizeChanged -= Page_SizeChanged;
                _page = null;
            }
            OnceInitializeAction = null;
            VirtualView = null;
        }
        base.Dispose(disposing);
    }    

    private void Page_SizeChanged(object? sender, EventArgs e)
    {
        if (_page is null)
        {
            return;
        }

        if (OnceInitializeAction == null)
        {
            ResetLayout();            
        }
        else
        {
            OnceInitializeAction.Invoke();
        }
    }

    void ResetLayout()
    {
        // When rotating the screen, a problem exists where all child views
        // must be removed and then re-set before they can be drawn properly.
        RemoveAllViews();
        SetupChildren();
    }    

    void Initialize()
    {
        OnceInitializeAction = null;

        SetupChildren();

        if (_page!.Handler is not PageHandler handler)
        {
            return;
        }

        var pageView = handler.PlatformView as ViewGroup;
        pageView.AddView(this);
    }

    void SetupChildren()
    {
        if (_page is null || VirtualView is null)
        {
            return;
        }

        VirtualView.Layout(_page.Bounds);

        foreach (var child in VirtualView)
        {
            SetChildLayout(child);
        }        

        // For some reason, needs to call Measure and Layout Method manually.
        Measure((int)Context.ToPixels(_page.Bounds.Width), (int)Context.ToPixels(_page.Bounds.Height));
        Layout(0, 0, (int)Context.ToPixels(_page.Bounds.Width), (int)Context.ToPixels(_page.Bounds.Height));

        // On Android, If IsVisible is false when a page is rendered,
        // there is the issue that views are never displayed.
        // As a workaround, do not deal with the IsVisible property directly, but control the display with an Alpha value.
        foreach (var child in VirtualView)
        {
            var visible = FloatingLayout.GetIsVisible(child);
            if (!visible)
            {
                if (child.Handler is IPlatformViewHandler childHandler)
                {
                    childHandler.PlatformView!.Alpha = 0f;
                    // If not set to false once, it may not be reflected correctly.
                    child.InputTransparent = false;
                    child.InputTransparent = true;
                }
            }
        }
    }

    LayoutParams CreateChildLayoutParames(MauiView child)
    {
        VirtualView?.LayoutChild(child);

        int width = (int)Context.ToPixels(child.Width);
        int height = (int)Context.ToPixels(child.Height);

        // For some reason, Gravity Fill(Horizontal/Vertical) not work.
        // So set the fill size manually.
        if (child.HorizontalOptions.Alignment == LayoutAlignment.Fill)
        {
            width = (int)Context.ToPixels(_page!.Bounds.Width);
        }

        if (child.VerticalOptions.Alignment == LayoutAlignment.Fill)
        {
            height = (int)Context.ToPixels(_page!.Bounds.Height);
        }

        var param = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
        {
            Width = width,
            Height = height,
            Gravity = GetGravity(child),
        };
        SetOffsetMargin(param, child);

        return param;
    }

    void SetChildLayout(MauiView child)
    {
        var handler = child.ToHandler(child.FindMauiContext()!);
        var nativeChild = handler.PlatformView;
        nativeChild?.RemoveFromParent();

        AddView(nativeChild, CreateChildLayoutParames(child));
    }

    void SetOffsetMargin(FrameLayout.LayoutParams layoutParams, MauiView view)
    {
        var offsetX = (int)Context.ToPixels(FloatingLayout.GetOffsetX(view));
        if (view.HorizontalOptions.Alignment == LayoutAlignment.Fill)
        {
            offsetX = 0;
        }
        var offsetY = (int)Context.ToPixels(FloatingLayout.GetOffsetY(view));
        if (view.VerticalOptions.Alignment == LayoutAlignment.Fill)
        {
            offsetY = 0;
        }

        // the offset direction is reversed when GravityFlags contains Left or Bottom.
        if (view.HorizontalOptions.Alignment == LayoutAlignment.End)
        {
            layoutParams.RightMargin = offsetX * -1;
        }
        else
        {
            layoutParams.LeftMargin = offsetX;
        }

        if (view.VerticalOptions.Alignment == LayoutAlignment.End)
        {
            layoutParams.BottomMargin = offsetY * -1;
        }
        else
        {
            layoutParams.TopMargin = offsetY;
        }
    }

    GravityFlags GetGravity(MauiView view)
    {
        GravityFlags gravity = GravityFlags.NoGravity;
        gravity |= view.VerticalOptions.Alignment switch
        {
            LayoutAlignment.Start => GravityFlags.Top,
            LayoutAlignment.End => GravityFlags.Bottom,
            LayoutAlignment.Center => GravityFlags.CenterVertical,
            _ => GravityFlags.FillVertical,
        };
        gravity |= view.HorizontalOptions.Alignment switch
        {
            LayoutAlignment.Start => GravityFlags.Left,
            LayoutAlignment.End => GravityFlags.Right,
            LayoutAlignment.Center => GravityFlags.CenterHorizontal,
            _ => GravityFlags.FillHorizontal,
        };

        return gravity;
    }
}

