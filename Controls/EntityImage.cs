using DungeonCrawlerGame.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DungeonCrawlerGame.Controls
{
    public class EntityImage : RenderableImage
    {
        private static readonly BitmapSource _playerBitmap = new BitmapImage(new Uri("pack://application:,,,/DungeonCrawlerGame;component/Assets/player.png"));
        private static readonly BitmapSource _slimeBitmap = new BitmapImage(new Uri("pack://application:,,,/DungeonCrawlerGame;component/Assets/slime.png"));

        public EntityImage() : base()
        {
        }

        public EntityImage(int id, double x, double y, double height, double width, EntityType type) : base(id, x, y, height, width)
        {
            Type = type;
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
        }

        public EntityType Type
        {
            get { return (EntityType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(EntityType), typeof(EntityImage), new PropertyMetadata(EntityType.None, new PropertyChangedCallback(OnTypeSet)));

        private static void OnTypeSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var type = e.NewValue as EntityType?;
            if (type != null && d is EntityImage sender)
            {
                sender.SetTexture(type.Value);
            }
        }

        public void SetTexture(EntityType type)
        {
            switch (type)
            {
                case EntityType.None:
                    Source = null;
                    break;
                case EntityType.Player:
                    Source = _playerBitmap;
                    break;
                case EntityType.Slime:
                    Source = _slimeBitmap;
                    break;
            }
        }
    }
}
