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
        public TileImage() : base()
        {
        }

        public TileImage(int id, double x, double y, double height, double width, TileType type, TileSideType side) : base(id, x, y, height, width)
        {
            Side = side;
            Type = type;
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
            // Stretch = Stretch.UniformToFill;
        }

        public TileType Type
        {
            get { return (TileType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public TileSideType Side
        {
            get { return (TileSideType)GetValue(SideProperty); }
            set { SetValue(SideProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Side.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SideProperty =
            DependencyProperty.Register("Side", typeof(TileSideType), typeof(TileImage), new PropertyMetadata(TileSideType.None, new PropertyChangedCallback(OnSideSet)));

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(TileType), typeof(TileImage), new PropertyMetadata(TileType.None, new PropertyChangedCallback(OnTypeSet)));

        private static void OnTypeSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var type = e.NewValue as TileType?;
            if (type != null && d is TileImage sender)
            {
                sender.SetTexture(type.Value, sender.Side);
            }
        }

        private static void OnSideSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var side = e.NewValue as TileSideType?;
            if (side != null && d is TileImage sender)
            {
                sender.SetTexture(sender.Type, side.Value);
            }
        }

        public void SetTexture(TileType type, TileSideType side)
        {
            switch (type)
            {
                case TileType.Floor:
                    Source = new MappedImage(MapSourceType.Tileset, new Int32Rect(128, 32, 16, 16)).Source;
                    break;
                case TileType.Wall:
                    switch (side)
                    {
                        case TileSideType.Up:
                            Source = new MappedImage(MapSourceType.Tileset, new Int32Rect(0, 48, 16, 16)).Source;
                            break;
                        case TileSideType.Down:
                            Source = new MappedImage(MapSourceType.Tileset, new Int32Rect(32, 64, 16, 16)).Source;
                            break;
                        case TileSideType.Left:
                            Source = new MappedImage(MapSourceType.Tileset, new Int32Rect(0, 96, 16, 16)).Source;
                            break;
                        case TileSideType.Right:
                            Source = new MappedImage(MapSourceType.Tileset, new Int32Rect(16, 96, 16, 16)).Source;
                            break;
                        case TileSideType.OuterCornerUpLeft:
                            Source = new MappedImage(MapSourceType.Tileset, new Int32Rect(48, 0, 16, 16)).Source;
                            break;
                        case TileSideType.OuterCornerUpRight:
                            Source = new MappedImage(MapSourceType.Tileset, new Int32Rect(64, 0, 16, 16)).Source;
                            break;
                        case TileSideType.OuterCornerDownLeft:
                            Source = new MappedImage(MapSourceType.Tileset, new Int32Rect(48, 16, 16, 16)).Source;
                            break;
                        case TileSideType.OuterCornerDownRight:
                            Source = new MappedImage(MapSourceType.Tileset, new Int32Rect(64, 16, 16, 16)).Source;
                            break;
                        case TileSideType.InnerCornerUpLeft:
                            Source = new MappedImage(MapSourceType.Tileset, new Int32Rect(32, 32, 16, 16)).Source;
                            break;
                        case TileSideType.InnerCornerUpRight:
                            Source = new MappedImage(MapSourceType.Tileset, new Int32Rect(0, 32, 16, 16)).Source;
                            break;
                        case TileSideType.InnerCornerDownLeft:
                            Source = new MappedImage(MapSourceType.Tileset, new Int32Rect(32, 0, 16, 16)).Source;
                            break;
                        case TileSideType.InnerCornerDownRight:
                            Source = new MappedImage(MapSourceType.Tileset, new Int32Rect(0, 0, 16, 16)).Source;
                            break;
                    }
                    break;
                case TileType.Door:
                    switch (side)
                    {
                        case TileSideType.Up:
                            Source = new MappedImage(MapSourceType.Tileset, new Int32Rect(32, 128, 16, 16)).Source;
                            break;
                        case TileSideType.Down:
                            Source = new MappedImage(MapSourceType.Tileset, new Int32Rect(128, 176, 16, 16)).Source;
                            break;
                        case TileSideType.Left:
                            Source = new MappedImage(MapSourceType.Tileset, new Int32Rect(48, 192, 16, 16)).Source;
                            break;
                        case TileSideType.Right:
                            Source = new MappedImage(MapSourceType.Tileset, new Int32Rect(32, 176, 16, 16)).Source;
                            break;
                    }
                    break;
                case TileType.Stairs:
                    Source = new MappedImage(MapSourceType.Tileset, new Int32Rect(80, 64, 16, 16)).Source;
                    break;
            }
        }
    }
}
