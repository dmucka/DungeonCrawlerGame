using DungeonCrawlerGame.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DungeonCrawlerGame.Classes
{
    // https://stackoverflow.com/questions/889825/is-it-possible-to-bind-a-canvass-children-property-in-xaml
    public static class CanvasBehavior
    {
        #region Dependency Properties

        public static readonly DependencyProperty RenderChildrenProperty =
            DependencyProperty.RegisterAttached("RenderChildren", typeof(ObservableCollection<RenderableImage>), typeof(CanvasBehavior),
                                                new PropertyMetadata(OnRenderChildrenChanged));

        public static ObservableCollection<RenderableImage> GetRenderChildren(DependencyObject d)
        {
            var renderChildren = (ObservableCollection<RenderableImage>)d.GetValue(RenderChildrenProperty);
            return renderChildren;
        }

        public static void SetRenderChildren(DependencyObject d, ObservableCollection<RenderableImage> value)
        {
            d.SetValue(RenderChildrenProperty, value);
        }

        #endregion

        private static void OnRenderChildrenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Canvas canvas)
                return;

            if (e.OldValue == e.NewValue)
                return;

            if (e.OldValue != null)
            {
                var renderChildrenOld = (ObservableCollection<RenderableImage>)e.OldValue;
                renderChildrenOld.CollectionChanged -= RenderChildren_CollectionChanged;
            }

            canvas.Children.Clear();

            if (e.NewValue == null)
                return;

            var renderChildrenNew = (ObservableCollection<RenderableImage>)e.NewValue;
            renderChildrenNew.CollectionChanged += RenderChildren_CollectionChanged;

            foreach (var item in renderChildrenNew)
            {
                if (item.Parent is Canvas c)
                    c.Children.Remove(item);

                canvas.Children.Add(item);
            }
        }

        private static void RenderChildren_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var canvas = (Canvas)sender;

            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (RenderableImage item in e.NewItems)
                    canvas.Children.Add(item);

            if (e.Action == NotifyCollectionChangedAction.Remove)
                foreach (RenderableImage item in e.OldItems)
                    canvas.Children.Remove(item);
        }
    }
}
