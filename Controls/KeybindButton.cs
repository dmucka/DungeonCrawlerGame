using System.Windows.Controls;
using System;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls.Primitives;
using LambdaConverters;
using System.Windows.Data;

namespace DungeonCrawlerGame.Controls
{
    public static class KeybindButtonConverters
    {
        /// <summary>
        /// Converts <see cref="Key"/> to <see cref="string"/>.
        /// </summary>
        public static readonly IValueConverter KeybindConverter = ValueConverter.Create<Key, string>(e => e.Value.ToString());
    }

    /// <summary>
    /// Button for selecting key binding.
    /// </summary>
    public class KeybindButton : ButtonBase
    {
        static KeybindButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KeybindButton), new FrameworkPropertyMetadata(typeof(KeybindButton)));
        }

        /// <summary>
        /// Cleans up after deselecting.
        /// </summary>
        protected void CleanUp()
        {
            IsCapturingInput = false;
            InputManager.Current.PreProcessInput -= OnSecondClick;
        }

        /// <summary>
        /// Callback for second click anywhere in the window.
        /// </summary>
        private void OnSecondClick(object sender, PreProcessInputEventArgs e)
        {
            if (e.StagingItem.Input is MouseButtonEventArgs)
            {
                CleanUp();
                return;
            }
        }

        /// <summary>
        /// Callback for click on button.
        /// </summary>
        protected override void OnClick()
        {
            IsCapturingInput = true;
            InputManager.Current.PreProcessInput += OnSecondClick;
        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            if (!IsCapturingInput)
                return;

            if (e.Key == Key.Escape)
            {
                CleanUp();
                return;
            }

            Keybind = e.Key;
            CleanUp();
        }

        #region Dependency Properties

        public Key Keybind
        {
            get { return (Key)GetValue(KeybindProperty); }
            set { SetValue(KeybindProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Keybind.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeybindProperty =
            DependencyProperty.Register("Keybind", typeof(Key), typeof(KeybindButton), new PropertyMetadata(Key.None));


        public bool IsCapturingInput
        {
            get { return (bool)GetValue(IsCapturingInputProperty); }
            set { SetValue(IsCapturingInputProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCapturingInput.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCapturingInputProperty =
            DependencyProperty.Register("IsCapturingInput", typeof(bool), typeof(KeybindButton), new PropertyMetadata(false));

        #endregion
    }
}
