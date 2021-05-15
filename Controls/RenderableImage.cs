using DungeonCrawlerGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DungeonCrawlerGame.Controls
{
    public class RenderableImage : Image, IRenderableElement
    {
        static RenderableImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RenderableImage), new FrameworkPropertyMetadata(typeof(RenderableImage)));
        }

        public RenderableImage()
        {
        }

        public RenderableImage(int id, double x, double y, double height, double width)
        {
            Id = id;
            X = x;
            Y = y;
            Height = height;
            Width = width;
        }

        public int Id { get; private set; }

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        // Using a DependencyProperty as the backing store for X.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(RenderableImage), new PropertyMetadata(0.0d, new PropertyChangedCallback(OnXSet)));

        private static void OnXSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var x = e.NewValue as double?;
            if (x != null && d is RenderableImage sender)
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
            DependencyProperty.Register("Y", typeof(double), typeof(RenderableImage), new PropertyMetadata(0.0d, new PropertyChangedCallback(OnYSet)));

        private static void OnYSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var y = e.NewValue as double?;
            if (y != null && d is RenderableImage sender)
            {
                Canvas.SetLeft(sender, y.Value);
            }
        }

        public void Update(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
