using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace DungeonCrawlerGame.Controls
{
    /// <summary>
    /// Clickable label that optionally opens an URI link.
    /// </summary>
    public class LinkLabel : ButtonBase
    {
        static LinkLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LinkLabel), new FrameworkPropertyMetadata(typeof(LinkLabel)));
        }

        public string Uri
        {
            get => (string)GetValue(UriProperty);
            set => SetValue(UriProperty, value);
        }

        // Using a DependencyProperty as the backing store for Uri.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register("Uri", typeof(string), typeof(LinkLabel), new PropertyMetadata(string.Empty));

        protected override void OnClick()
        {
            if (!string.IsNullOrWhiteSpace(Uri))
            {
                Process.Start("explorer", Uri);
            }

            base.OnClick();
        }
    }
}
