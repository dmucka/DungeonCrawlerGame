using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace DungeonCrawlerGame.Controls
{
    public class LinkLabel : ButtonBase
    {
        static LinkLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LinkLabel), new FrameworkPropertyMetadata(typeof(LinkLabel)));
        }

        public string Uri
        {
            get
            {
                return (string)this.GetValue(UriProperty);
            }
            set
            {
                this.SetValue(UriProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Uri.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register("Uri", typeof(string), typeof(LinkLabel), new PropertyMetadata(string.Empty));

        protected override void OnClick()
        {
            base.OnClick();

            if (!string.IsNullOrWhiteSpace(this.Uri))
            {
                Process.Start("explorer", this.Uri);
            }
        }
    }
}
