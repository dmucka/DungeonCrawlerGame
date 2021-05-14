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
    public class TileImage : RenderableImage
    {
        private static readonly BitmapSource _wallBitmap = new BitmapImage(new Uri("pack://application:,,,/DungeonCrawlerGame;component/Assets/wall1.png"));
        private static readonly BitmapSource _doorBitmap = new BitmapImage(new Uri("pack://application:,,,/DungeonCrawlerGame;component/Assets/door1.png"));
        private static readonly BitmapSource _floorBitmap = new BitmapImage(new Uri("pack://application:,,,/DungeonCrawlerGame;component/Assets/floor.png"));
        private static readonly BitmapSource _stairsBitmap = new BitmapImage(new Uri("pack://application:,,,/DungeonCrawlerGame;component/Assets/stairs.png"));

        public TileImage() : base()
        {
        }

        public TileImage(double x, double y, double height, double width, TileType type) : base(x, y, height, width)
        {
            Type = type;
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
        }

        public TileType Type
        {
            get { return (TileType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(TileType), typeof(TileImage), new PropertyMetadata(TileType.None, new PropertyChangedCallback(OnTypeSet)));

        private static void OnTypeSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var type = e.NewValue as TileType?;
            if (type != null && d is TileImage sender)
            {
                sender.SetTexture(type.Value);
            }
        }

        public void SetTexture(TileType type)
        {
            switch (type)
            {
                case TileType.Floor:
                    Source = _floorBitmap;
                    break;
                case TileType.Wall:
                    Source = _wallBitmap;
                    break;
                case TileType.Door:
                    Source = _doorBitmap;
                    break;
                case TileType.Stairs:
                    Source = _stairsBitmap;
                    break;
            }
        }
    }
}
