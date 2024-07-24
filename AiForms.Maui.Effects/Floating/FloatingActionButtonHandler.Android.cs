using System;
using Android.Content.OM;
using Fab = Google.Android.Material.FloatingActionButton.FloatingActionButton;
using Microsoft.Maui.Handlers;
using Android.Content.Res;
using Microsoft.Maui.Platform;
using Android.Graphics.Drawables;
using Microsoft.Maui.Controls;

namespace AiForms.Effects;

public class FloatingActionButtonHandler: ViewHandler<FloatingActionButton, Fab>, IImageSourcePartSetter
{
    public static IPropertyMapper<FloatingActionButton, FloatingActionButtonHandler> Mapper =
    new PropertyMapper<FloatingActionButton, FloatingActionButtonHandler>(ViewMapper)
    {
        [nameof(FloatingActionButton.Color)] = MapColor,
        [nameof(FloatingActionButton.ImageColor)] = MapImageColor,
        [nameof(FloatingActionButton.ImageSource)] = MapImageSource
    };

    private static void MapImageColor(FloatingActionButtonHandler handler, FloatingActionButton button)
    {
        if (button.ImageColor.IsNotDefault())
        {
            handler.PlatformView.ImageTintList = DrawableUtility.GetPressedColorSelector(button.ImageColor.ToPlatform());
        }
    }

    private static void MapImageSource(FloatingActionButtonHandler handler, FloatingActionButton button)
    {
        handler.UpdateImageSource();
    }

    private static void MapColor(FloatingActionButtonHandler handler, FloatingActionButton button)
    {
        handler.PlatformView.BackgroundTintList = ColorStateList.ValueOf(button.Color.ToPlatform());
        //var ripple = Colors.Black.MultiplyAlpha(0.3f);        handler.PlatformView.SetRippleColor(DrawableUtility.GetPressedColorSelector(ripple.ToPlatform()));        
    }

    ClickListener? _clickListener;
    ImageSourcePartLoader? _imageLoader;
    IElementHandler? IImageSourcePartSetter.Handler => this;

    IImageSourcePart? IImageSourcePartSetter.ImageSourcePart => VirtualView;

    public FloatingActionButtonHandler():base(Mapper)
    {

    }   

    protected override Fab CreatePlatformView()
    {
        var fab = new Fab(Context);
        _clickListener = new ClickListener(VirtualView);

        fab.SetOnClickListener(_clickListener);
        fab.Size = Fab.SizeAuto;
        fab.SetScaleType(Android.Widget.ImageView.ScaleType.Center);

        return fab;
    }

    protected override void ConnectHandler(Fab platformView)
    {
        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(Fab platformView)
    {
        base.DisconnectHandler(platformView);

        _imageLoader?.Reset();
        _imageLoader = null;
        platformView.SetOnClickListener(null);
        platformView.Dispose();
        _clickListener?.Dispose();
        _clickListener = null;
    }

    void UpdateImageSource()
    {
        _imageLoader?.Reset();

        if (PlatformView.Drawable != null)
        {
            PlatformView.Drawable?.Dispose();
            PlatformView.SetImageDrawable(null);
            PlatformView.SetImageBitmap(null);
        }

        if(VirtualView.ImageSource is null)
        {
            return;
        }       

        _imageLoader = new ImageSourcePartLoader(this);
        _imageLoader.UpdateImageSourceAsync();
    }

    void IImageSourcePartSetter.SetImageSource(Drawable? platformImage)
    {
        PlatformView.SetImageDrawable(platformImage);
        PlatformView.Invalidate();
    }

    class ClickListener : Java.Lang.Object, Android.Views.View.IOnClickListener
    {
        FloatingActionButton _virtualView;
        public ClickListener(FloatingActionButton virtualView)
        {
            _virtualView = virtualView;
        }
        public void OnClick(Android.Views.View? v)
        {
            if (_virtualView.Command == null)
            {
                return;
            }

            if (_virtualView.Command.CanExecute(null))
            {
                _virtualView.Command.Execute(null);
            }
        }
    }
}

