using System;
using System.Windows.Input;

namespace AiForms.Effects;

public class FloatingActionButton: View, IImageSourcePart
{
    public static BindableProperty CommandProperty =
        BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(FloatingActionButton),
            default(ICommand),
            defaultBindingMode: BindingMode.OneWay
        );

    public ICommand Command
    {
        get { return (ICommand)GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }

    public static BindableProperty ImageSourceProperty =
        BindableProperty.Create(
            nameof(ImageSource),
            typeof(ImageSource),
            typeof(FloatingActionButton),
            default(ImageSource),
            defaultBindingMode: BindingMode.OneWay
        );

    public ImageSource ImageSource
    {
        get { return (ImageSource)GetValue(ImageSourceProperty); }
        set { SetValue(ImageSourceProperty, value); }
    }

    public static BindableProperty ColorProperty =
        BindableProperty.Create(
            nameof(Color),
            typeof(Color),
            typeof(FloatingActionButton),
            KnownColor.Accent,
            defaultBindingMode: BindingMode.OneWay
        );

    public Color Color
    {
        get { return (Color)GetValue(ColorProperty); }
        set { SetValue(ColorProperty, value); }
    }

    public static BindableProperty ImageColorProperty = BindableProperty.Create(
            nameof(ImageColor),
            typeof(Color),
            typeof(FloatingActionButton),
            KnownColor.Default,
            defaultBindingMode: BindingMode.OneWay
        );

    public Color ImageColor{
        get { return (Color)GetValue(ImageColorProperty); }
        set { SetValue(ImageColorProperty, value); }
    }

    public IImageSource? Source => ImageSource;

    public bool IsAnimationPlaying { get; set; }

    public bool IsLoading { get; set; }
    public void UpdateIsLoading(bool isLoading)
    {
        IsLoading = isLoading;
    }
}

