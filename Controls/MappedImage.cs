using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DungeonCrawlerGame.Controls
{
    /// <summary>
    /// Maps a rectangle from an image as a new image.
    /// </summary>
    public class MappedImage : Image
    {
        public static readonly BitmapSource uiSource = new BitmapImage(new Uri("pack://application:,,,/DungeonCrawlerGame;component/Assets/ui.png"));

        public MappedImage()
        {
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
        }

        public Int32Rect Location
        {
            get { return (Int32Rect)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Location.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register("Location", typeof(Int32Rect), typeof(MappedImage), new PropertyMetadata(new Int32Rect(), new PropertyChangedCallback(OnLocationSet)));

        private static void OnLocationSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var location = e.NewValue as Int32Rect?;
            if (location != null && d is MappedImage sender)
            {
                sender.Source = new CroppedBitmap(uiSource, location.Value);
            }
        }
    }
}
