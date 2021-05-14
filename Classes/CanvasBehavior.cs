using DungeonCrawlerGame.Controls;
using DungeonCrawlerGame.Interfaces;
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
            DependencyProperty.RegisterAttached("RenderChildren", typeof(ObservableCollection<IRenderable>), typeof(CanvasBehavior),
                                                new PropertyMetadata(OnRenderChildrenChanged));

        public static ObservableCollection<IRenderable> GetRenderChildren(DependencyObject d)
        {
            var renderChildren = (ObservableCollection<IRenderable>)d.GetValue(RenderChildrenProperty);
            return renderChildren;
        }

        public static void SetRenderChildren(DependencyObject d, ObservableCollection<IRenderable> value)
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
                var renderChildrenOld = (ObservableCollection<IRenderable>)e.OldValue;
                renderChildrenOld.CollectionChanged -= RenderChildren_CollectionChanged;
            }

            canvas.Children.Clear();

            if (e.NewValue == null)
                return;

            var renderChildrenNew = (ObservableCollection<IRenderable>)e.NewValue;
            renderChildrenNew.CollectionChanged += RenderChildren_CollectionChanged;

            foreach (FrameworkElement item in renderChildrenNew)
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
                foreach (FrameworkElement item in e.NewItems)
                    canvas.Children.Add(item);

            if (e.Action == NotifyCollectionChangedAction.Remove)
                foreach (FrameworkElement item in e.OldItems)
                    canvas.Children.Remove(item);
        }
    }
}
