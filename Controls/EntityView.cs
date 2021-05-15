using DungeonCrawlerGame.Enums;
using DungeonCrawlerGame.Interfaces;
using LambdaConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DungeonCrawlerGame.Controls
{
    public static class EntityViewConverters
    {
        public static readonly IValueConverter EntityStatusConverter = ValueConverter.Create<int, string>(e => $"HP: {e.Value}");
    }

    public class EntityView : StackPanel, IRenderableElement
    {
        private static readonly BitmapSource _playerBitmap = new BitmapImage(new Uri("pack://application:,,,/DungeonCrawlerGame;component/Assets/player.png"));
        private static readonly BitmapSource _slimeBitmap = new BitmapImage(new Uri("pack://application:,,,/DungeonCrawlerGame;component/Assets/slime.png"));

        public Image EntityImage { get; }
        public Label EntityStatus { get; }

        public EntityView()
        {
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
            Orientation = Orientation.Vertical;

            EntityImage = new Image();
            EntityStatus = new Label();
            EntityStatus.HorizontalAlignment = HorizontalAlignment.Center;
            EntityStatus.DataContext = this;
            EntityStatus.SetBinding(Label.ContentProperty, new Binding("Health") { Converter = EntityViewConverters.EntityStatusConverter });

            Children.Add(EntityStatus);
            Children.Add(EntityImage);
        }

        public EntityView(int id, double x, double y, double height, double width, EntityType type, int health) : this()
        {
            Id = id;
            X = x;
            Y = y;
            EntityImage.Height = height;
            EntityImage.Width = width;
            Type = type;
            Health = health;
        }

        public int Health
        {
            get { return (int)GetValue(HealthProperty); }
            set { SetValue(HealthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Health.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HealthProperty =
            DependencyProperty.Register("Health", typeof(int), typeof(EntityView), new PropertyMetadata(0));



        public EntityType Type
        {
            get { return (EntityType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(EntityType), typeof(EntityView), new PropertyMetadata(EntityType.None, new PropertyChangedCallback(OnTypeSet)));

        private static void OnTypeSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var type = e.NewValue as EntityType?;
            if (type != null && d is EntityView sender)
            {
                sender.SetTexture(type.Value);
            }
        }

        public void SetTexture(EntityType type)
        {
            switch (type)
            {
                case EntityType.None:
                    EntityImage.Source = null;
                    break;
                case EntityType.Player:
                    EntityImage.Source = _playerBitmap;
                    break;
                case EntityType.Slime:
                    EntityImage.Source = _slimeBitmap;
                    break;
            }
        }

        #region IRenderableElement

        public int Id { get; private set; }

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        // Using a DependencyProperty as the backing store for X.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(EntityView), new PropertyMetadata(0.0d, new PropertyChangedCallback(OnXSet)));

        private static void OnXSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var x = e.NewValue as double?;
            if (x != null && d is EntityView sender)
            {
                Canvas.SetTop(sender, x.Value);
            }
        }

        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Y.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(EntityView), new PropertyMetadata(0.0d, new PropertyChangedCallback(OnYSet)));

        private static void OnYSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var y = e.NewValue as double?;
            if (y != null && d is EntityView sender)
            {
                Canvas.SetLeft(sender, y.Value);
            }
        }

        public void Update(double x, double y)
        {
            X = x;
            Y = y;
        }

        public void Update(double x, double y, int health)
        {
            X = x;
            Y = y;
            Health = health;
        }

        #endregion
    }
}
