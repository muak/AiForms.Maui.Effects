using System;
using AiForms.Effects.Extensions;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace AiForms.Effects;

public class FloatingLayoutHandler: ViewHandler<FloatingLayout, FloatingNativeLayout>
{
    public static IPropertyMapper<FloatingLayout, FloatingLayoutHandler> Mapper =
    new PropertyMapper<FloatingLayout, FloatingLayoutHandler>(ViewMapper)
    {
    };

    public FloatingLayoutHandler():base(Mapper)
    {
    }

    protected override FloatingNativeLayout CreatePlatformView()
    {
        var layout = new FloatingNativeLayout(Context, VirtualView);
        return layout;
    }

    protected override void ConnectHandler(FloatingNativeLayout platformView)
    {
        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(FloatingNativeLayout platformView)
    {
        base.DisconnectHandler(platformView);

        platformView.Dispose();
    }    
}

