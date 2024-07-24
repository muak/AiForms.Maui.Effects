# AiForms.Effects for .NET MAUI

AiForms.Effects is the effects library that provides you with more flexible functions than default by targetting only Android and iOS in a MAUI project.

[Japanese](./README-ja.md)


## Features

* [Floating](#floating)
    * arrange some floating views (e.g. FAB) at any place over a page.


> Many features have been implemented as standard, so the owner himself has ported only the features he needs from the Xamarin version.
> Other features may be ported or added as needed.

## **Trigger Property**

An effect can be enabled by setting one or more main properties of the effect.
These properties are named "Trigger Properties".

 **Trigger properties** correspond to main properties such as Command and LongCommand in case of AddCommand Effect.

In this document,  trigger properties are written "trigger".



## Nuget Installation

```bash
Install-Package AiForms.Maui.Effects
```

### Get Started

You need to write the following code in MauiProgram.cs:

```csharp
var builder = MauiApp.CreateBuilder();
builder.UseAiEffects();
```


## Floating

This is the effect that arranges some floating views (e.g. FAB) at any place on a page.
The arranged elements are displayed more front than a ContentPage and are not affected a ContentPage scrolling.

### How to use

This sample is that an element is arranged at above 25dp from the vertical end and left 25dp from the horizontal end.

```xml
<ContentPage xmlns:ef="clr-namespace:AiForms.Effects;assembly=AiForms.Maui.Effects">
    
    <ef:Floating.Content>
        <ef:FloatingLayout>
            <!-- This element is arranged at above 25dp from the vertical end and left 25dp from the horizontal end. -->
            <!-- Code behind handling / ViewModel binding OK -->
            <!-- FloatingView is gone from the MAUI version -->
            <Button
                VerticalLayoutAlignment="End" 
                HorizontalLayoutAlignment="End"
                ef:FloatingLayout.OffsetX="-25"
                ef:FloatingLayout.OffsetY="-25"
                ef:FloatingLayout.IsVisible="{Binding ButtonVisible}"
                Clicked="BlueTap"
                BackgroundColor="{Binding ButtonColor}" 
                BorderRadius="28"
                WidthRequest="56"
                HeightRequest="56"
                Text="+"
                FontSize="24"
                TextColor="White"
                Padding="0" />
        </ef:FloatingLayout>
    </ef:Floating.Content>

    <StackLayout>
        <Label Text="MainContents" />
    </StackLayout>
</ContentPage>
```

<img src="images/floating.jpg" width="600" /> 

### Property

* Content (trigger)
    * The root element to arrange floating views.
    * This property is of type FloatingLayout.


### FloatingLayout

A layout element that allows multiple Views to be placed freely on a page.
Additional information such as ``OffsetX``, ``OffsetY``, etc. can be set for each child element using its attached properties.

The placement of the child elements is determined by ``VerticalOptions`` and ``HorizontalOptions``, and the relative positioning is adjusted by the values of ``OffsetX`` and ``OffsetY``.


#### Attached Properties

* OffsetX
    * The adjustment relative value from the horizontal layout position. (without Fill)
* OffsetY
    * The adjustment relative value from the vertical layout position. (without Fill)
* IsVisible
    * A bool value indicating whether the View should be displayed or not.
    * Standard IsVisible may not function properly on FloatingLayout.

### FloatingActionButton

This is a FloatingActionButton control.
It is not an Effect, but is implemented for convenience.
iOS has its own implementation, while Android uses the Native FloatingActionButton.

It is a normal control, so it can be used for purposes other than FloatingLayout.

#### Properties

* Command
    * Action when tapped
* ImageSource
    * Image to place in the center
* Color
    * Color of entire button
* ImageColor
    * Color of the image

## Donation

I am asking for your donation for continuous development🙇

Your donation will allow me to work harder and harder.

* [PayPalMe](https://paypal.me/kamusoftJP?locale.x=ja_JP)


## Sponsors

I am asking for sponsors too.
This is a subscription.

* [GitHub Sponsors](https://github.com/sponsors/muak)


## License

MIT Licensed.
