using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

namespace Lib.Tools.Watermark
{
    public static class WatermarkService
    {
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached("Watermark", typeof(object), typeof(WatermarkService), new FrameworkPropertyMetadata(null, OnWatermarkChanged));

        private static readonly Dictionary<object, ItemsControl> _itemsControls = new Dictionary<object, ItemsControl>();

        public static object GetWatermark(DependencyObject dependencyObject)
        {
            return dependencyObject.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(DependencyObject dependencyObject, object value)
        {
            dependencyObject.SetValue(WatermarkProperty, value);
        }

        private static void OnWatermarkChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (!(dependencyObject is Control control))
                return;

            control.Loaded += OnControlLoaded;

            if (dependencyObject is ComboBox)
            {
                control.GotKeyboardFocus += OnControlGotKeyboardFocus;
                control.LostKeyboardFocus += OnControlLoaded;
            }
            else if (dependencyObject is TextBox textBox)
            {
                control.GotKeyboardFocus += OnControlGotKeyboardFocus;
                control.LostKeyboardFocus += OnControlLoaded;
                textBox.TextChanged += OnControlGotKeyboardFocus;
            }

            if (dependencyObject is ItemsControl itemsControl && !(dependencyObject is ComboBox))
            {
                itemsControl.ItemContainerGenerator.ItemsChanged += OnItemsChanged;
                _itemsControls.Add(itemsControl.ItemContainerGenerator, itemsControl);

                DependencyPropertyDescriptor property = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, itemsControl.GetType());
                property.AddValueChanged(itemsControl, OnItemsSourceChanged);
            }
        }

        private static void OnControlLoaded(object sender, RoutedEventArgs args)
        {
            if (!(sender is Control control))
                return;

            if (ShouldShowWatermark(control))
            {
                ShowWatermark(control);
            }
        }

        private static void OnControlGotKeyboardFocus(object sender, RoutedEventArgs args)
        {
            if (!(sender is Control control))
                return;

            if (ShouldShowWatermark(control))
            {
                ShowWatermark(control);
            }
            else
            {
                RemoveWatermark(control);
            }
        }

        private static void OnItemsChanged(object sender, ItemsChangedEventArgs args)
        {
            if (!_itemsControls.TryGetValue(sender, out ItemsControl control))
                return;

            if (ShouldShowWatermark(control))
            {
                ShowWatermark(control);
            }
            else
            {
                RemoveWatermark(control);
            }
        }

        private static void OnItemsSourceChanged(object sender, EventArgs args)
        {
            if (!(sender is ItemsControl itemsControl))
                return;

            if (itemsControl.ItemsSource != null)
            {
                if (ShouldShowWatermark(itemsControl))
                {
                    ShowWatermark(itemsControl);
                }
                else
                {
                    RemoveWatermark(itemsControl);
                }
            }
            else
            {
                ShowWatermark(itemsControl);
            }
        }

        private static bool ShouldShowWatermark(Control control)
        {
            if (control is ComboBox comboBox)
                return comboBox.Text == string.Empty;
            else if (control is TextBox textBox)
                return textBox.Text == string.Empty;
            else if (control is ItemsControl itemsControl)
                return itemsControl.Items.Count == 0;
            else
                return false;
        }

        private static void ShowWatermark(Control control)
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(control);
            layer?.Add(new WatermarkAdorner(control, GetWatermark(control)));
        }

        private static void RemoveWatermark(UIElement control)
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(control);

            Adorner[] adorners = layer?.GetAdorners(control);
            if (adorners == null)
                return;

            foreach (var adorner in adorners)
            {
                if (!(adorner is WatermarkAdorner))
                    continue;

                adorner.Visibility = Visibility.Hidden;
                layer.Remove(adorner);
            }
        }
    }
}