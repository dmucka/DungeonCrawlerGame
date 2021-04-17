using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Diagnostics;

namespace DungeonCrawlerGame.Controls
{
    public class LinkLabel : Label
    {
        public string Uri
        {
            get { return (string)this.GetValue(UriProperty); }
            set { this.SetValue(UriProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Uri.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register("Uri", typeof(string), typeof(LinkLabel), new PropertyMetadata(string.Empty));

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (!string.IsNullOrWhiteSpace(this.Uri))
            {
                Process.Start("explorer", this.Uri);
            }
        }
    }
}
