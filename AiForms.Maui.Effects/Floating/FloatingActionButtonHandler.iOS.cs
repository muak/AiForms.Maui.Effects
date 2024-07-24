using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace AiForms.Effects;

public class FloatingActionButtonHandler: ViewHandler<FloatingActionButton, FloatingActionNativeButton>
{
    public static IPropertyMapper<FloatingActionButton, FloatingActionButtonHandler> Mapper =
    new PropertyMapper<FloatingActionButton, FloatingActionButtonHandler>(ViewMapper)
    {
        [nameof(FloatingActionButton.Color)] = MapColor,
        [nameof(FloatingActionButton.ImageColor)] = MapImageColor,
        [nameof(FloatingActionButton.ImageSource)] = MapImageSource,
        [nameof(FloatingActionButton.IsEnabled)] = MapOverrideIsEnabled

    };

    private static void MapOverrideIsEnabled(FloatingActionButtonHandler handler, FloatingActionButton button)
    {
        if (!button.IsEnabled)
        {
            handler.PlatformView.Alpha = (float)button.Opacity * 0.6f;
        }
        else
        {
            handler.PlatformView.Alpha = (float)button.Opacity;
        }
    }

    private static void MapImageColor(FloatingActionButtonHandler handler, FloatingActionButton button)
    {
        handler.PlatformView.UpdateImageColor();
    }

    private static void MapImageSource(FloatingActionButtonHandler handler, FloatingActionButton button)
    {
        handler.PlatformView.UpdateImageSource();
    }

    private static void MapColor(FloatingActionButtonHandler handler, FloatingActionButton button)
    {
        handler.PlatformView.UpdateColor();
    }
    

    public FloatingActionButtonHandler():base(Mapper)
    {        
    }

    protected override FloatingActionNativeButton CreatePlatformView()
    {
        return new FloatingActionNativeButton(VirtualView);
    }

    protected override void ConnectHandler(FloatingActionNativeButton platformView)
    {
        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(FloatingActionNativeButton platformView)
    {
        base.DisconnectHandler(platformView);

        platformView.Dispose();
    }


}

