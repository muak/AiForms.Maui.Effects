using System;
using AiForms.Effects.Extensions;
using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.Startup;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using static Android.Views.ViewGroup;

namespace AiForms.Effects;

public class FloatingEffect: PlatformEffect
{
    FloatingLayout? _layout;
    protected override void OnAttached()
    {
        if (Element is not Page) return;

        _layout = Floating.GetContent(Element);
        _layout.Parent = Element;        

        // All following process is done on the side of FloatingLayoutHandler.
        _layout.ToHandler(_layout.FindMauiContext()!);
    }

    protected override void OnDetached()
    {
        if(_layout is not null)
        {
            CleanUp(_layout);
            _layout = null;
        }

        System.Diagnostics.Debug.WriteLine($"Detached {GetType().Name} from {Element.GetType().FullName}");
    }

    private static void CleanUp(FloatingLayout layout)
    {
        foreach(var child in layout)
        {
            Disconnect(child);
        }

        Disconnect(layout);

        return;

        static void Disconnect(IVisualTreeElement element)
        {            
            foreach (IVisualTreeElement childElement in element.GetVisualChildren())
            {
                Disconnect(childElement);
            }

            if (element is VisualElement visualElement)
            {
                try
                {
                    visualElement.Handler?.DisconnectHandler();
                }
                catch { }
            }
        }
    }
}

