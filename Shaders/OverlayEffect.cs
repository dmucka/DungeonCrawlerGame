using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace DungeonCrawlerGame.Shaders
{
    public class OverlayEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = 
            RegisterPixelShaderSamplerProperty(nameof(Input), typeof(OverlayEffect), 0);

        public static readonly DependencyProperty OverlayColorProperty = 
            DependencyProperty.Register(nameof(OverlayColor), typeof(Color), typeof(OverlayEffect), 
                new PropertyMetadata(Color.FromArgb(255, 0, 0, 0), PixelShaderConstantCallback(0)));

        public OverlayEffect()
        {
            PixelShader pixelShader = new PixelShader
            {
                UriSource = new Uri("pack://application:,,,/DungeonCrawlerGame;Component/Shaders/OverlayEffect.ps", UriKind.Absolute)
            };
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(OverlayColorProperty);
        }

        public Brush Input
        {
            get
            {
                return ((Brush)(GetValue(InputProperty)));
            }
            set
            {
                SetValue(InputProperty, value);
            }
        }

        public Color OverlayColor
        {
            get
            {
                return ((Color)(GetValue(OverlayColorProperty)));
            }
            set
            {
                SetValue(OverlayColorProperty, value);
            }
        }
    }
}
