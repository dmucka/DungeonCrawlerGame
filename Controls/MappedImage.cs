using DungeonCrawlerGame.Enums;
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
        private static readonly BitmapSource _interfaceBitmap = new BitmapImage(new Uri("pack://application:,,,/DungeonCrawlerGame;component/Assets/ui.png"));
        private static readonly BitmapSource _tilesetBitmap = new BitmapImage(new Uri("pack://application:,,,/DungeonCrawlerGame;component/Assets/sheet.png"));

        public MappedImage()
        {
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
        }

        public MappedImage(MapSourceType map, Int32Rect location) : this()
        {
            Map = map;
            Location = location;
        }

        public MapSourceType Map
        {
            get { return (MapSourceType)GetValue(MapProperty); }
            set { SetValue(MapProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Map.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapProperty =
            DependencyProperty.Register("Map", typeof(MapSourceType), typeof(MappedImage), new PropertyMetadata(MapSourceType.Interface, new PropertyChangedCallback(OnMapSet)));

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
                sender.SetTexture(sender.Map, location.Value);
            }
        }

        private static void OnMapSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapType = e.NewValue as MapSourceType?;
            if (mapType != null && d is MappedImage sender)
            {
                sender.SetTexture(mapType.Value, sender.Location);
            }
        }

        public void SetTexture(MapSourceType mapType, Int32Rect location)
        {
            switch (mapType)
            {
                case MapSourceType.Interface:
                    Source = new CroppedBitmap(_interfaceBitmap, location);
                    break;
                case MapSourceType.Tileset:
                    Source = new CroppedBitmap(_tilesetBitmap, location);
                    break;
            }
        }
    }
}
