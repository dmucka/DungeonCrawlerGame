using DungeonCrawlerGame.Enums;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace DungeonCrawlerGame.Controls
{
    /// <summary>
    /// Round themed button that contains an icon.
    /// </summary>
    public class RoundButton : ButtonBase
    {
        static RoundButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundButton), new FrameworkPropertyMetadata(typeof(RoundButton)));
        }

        public RoundButtonIcon Icon
        {
            get => (RoundButtonIcon)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(RoundButtonIcon), typeof(RoundButton), new PropertyMetadata(RoundButtonIcon.None));
    }
}
