using System;
namespace AiForms.Effects;

public static class MauiAppBuilderExtension
{
    public static MauiAppBuilder UseAiEffects(this MauiAppBuilder builder, bool enableTriggerProperty = true)
    {
        EffectConfig.EnableTriggerProperty = enableTriggerProperty;

        builder.ConfigureMauiHandlers(handler =>
        {
            handler.AddHandler(typeof(FloatingActionButton), typeof(FloatingActionButtonHandler));
#if ANDROID
            handler.AddHandler(typeof(FloatingLayout), typeof(FloatingLayoutHandler));
#endif
        })
        .ConfigureEffects(effects =>
        {
            effects.Add<FloatingRoutingEffect, FloatingEffect>();
        });

        return builder;
    }
}

