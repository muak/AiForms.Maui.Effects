using System;
using Foundation;
using Microsoft.Maui.Platform;
using UIKit;

namespace AiForms.Effects;

public class FloatingActionNativeButton: UIView, IImageSourcePartSetter
{
    public IElementHandler? Handler => _virtualView?.Handler;
    public IImageSourcePart? ImageSourcePart => _virtualView;
    ImageSourcePartLoader? _imageLoader;
    UIImageView? _imageView;
    FloatingActionButton? _virtualView;
    UITapGestureRecognizer? _touch;

    public FloatingActionNativeButton(FloatingActionButton virtualView)
    {
        _virtualView = virtualView;

        Layer.ShadowOffset = new CoreGraphics.CGSize(1, 2);
        Layer.ShadowColor = UIColor.Black.CGColor;
        Layer.ShadowOpacity = 0.2f;
        Layer.ShadowRadius = 6;

        _imageView = new UIImageView();
        AddSubview(_imageView);

        _imageView.TranslatesAutoresizingMaskIntoConstraints = false;
        _imageView.CenterXAnchor.ConstraintEqualTo(CenterXAnchor).Active = true;
        _imageView.CenterYAnchor.ConstraintEqualTo(CenterYAnchor).Active = true;
        _imageView.WidthAnchor.ConstraintEqualTo(24).Active = true;
        _imageView.HeightAnchor.ConstraintEqualTo(24).Active = true;

        _touch = new UITapGestureRecognizer(OnClick);
        AddGestureRecognizer(_touch);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_touch is not null)
            {
                RemoveGestureRecognizer(_touch);
                _touch.Dispose();
                _touch = null;
            }

            _imageLoader?.Reset();
            _imageLoader = null;
            _virtualView = null;

            if(_imageView is not null)
            {
                _imageView.Image?.Dispose();
                _imageView.Image = null;
                _imageView.Dispose();
                _imageView = null;
            }            
        }
        base.Dispose(disposing);
    }

    public override void TouchesBegan(NSSet touches, UIEvent? evt)
    {
        base.TouchesBegan(touches, evt);

        Animate(0.25, () =>
        {
            Layer.BackgroundColor = _virtualView?.Color?.MultiplyAlpha(0.5f).ToCGColor();
        });
    }

    public override void TouchesEnded(NSSet touches, UIEvent? evt)
    {
        base.TouchesEnded(touches!, evt);

        Animate(0.25, () =>
        {
            Layer.BackgroundColor = _virtualView?.Color?.ToCGColor();
        });
    }

    public override void TouchesCancelled(NSSet touches, UIEvent? evt)
    {
        base.TouchesCancelled(touches, evt);
        Animate(0.25, () =>
        {
            Layer.BackgroundColor = _virtualView?.Color?.ToCGColor();
        });
    }

    public override void LayoutSubviews()
    {
        base.LayoutSubviews();
        Layer.CornerRadius = this.Frame.Width / 2f;
    }

    internal void UpdateColor()
    {
        BackgroundColor = _virtualView?.Color?.ToPlatform();        
    }

    internal void UpdateImageColor()
    {
        if (_virtualView?.ImageColor.IsDefault() ?? true)
        {
            return;
        }
        if(_imageView is not null)
        {
            _imageView.TintColor = _virtualView.ImageColor.ToPlatform();
        }        
    }

    internal void UpdateImageSource()
    {
        _imageLoader?.Reset();

        if (_imageView?.Image is not null)
        {
            _imageView.Image.Dispose();
            _imageView.Image = null;
        }

        if(_virtualView?.ImageSource is null)
        {
            return;
        }

        _imageLoader = new ImageSourcePartLoader(this);
        _imageLoader.UpdateImageSourceAsync();
    }

    void OnClick()
    {
        if (_virtualView?.Command == null)
        {
            return;
        }

        if (_virtualView.Command.CanExecute(null))
        {
            _virtualView.Command.Execute(null);
        }
    }

    public void SetImageSource(UIImage? platformImage)
    {
        if(_imageView is null || _virtualView is null)
        {
            return;
        }

        if (_virtualView.ImageColor.IsDefault())
        {
            _imageView.Image = platformImage;
        }
        else
        {
            _imageView.Image = platformImage?.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
        }   
    }
}

