using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace Lib.Tools.Watermark
{
    public class WatermarkAdorner : Adorner
    {
        private readonly ContentPresenter _contentPresenter;

        public WatermarkAdorner(UIElement adornedElement, object watermark)
            : base(adornedElement)
        {
            IsHitTestVisible = false;

            _contentPresenter = new ContentPresenter
            {
                Content = watermark,
                Opacity = 0.5,
                //Margin = new Thickness(Control.Margin.Left + Control.Padding.Left, Control.Margin.Top + Control.Padding.Top, 0, 0)
                Margin = new Thickness(Control.Padding.Left, Control.Padding.Top + 1, Control.Padding.Right, Control.Padding.Bottom)
            };

            if (Control is ItemsControl && !(Control is ComboBox))
            {
                _contentPresenter.VerticalAlignment = VerticalAlignment.Center;
                _contentPresenter.HorizontalAlignment = HorizontalAlignment.Center;
            }

            Binding binding = new Binding("IsVisible")
            {
                Source = adornedElement,
                Converter = new BooleanToVisibilityConverter()
            };

            SetBinding(VisibilityProperty, binding);
        }


        private Control Control => (Control) AdornedElement;

        protected override int VisualChildrenCount => 1;


        protected override Visual GetVisualChild(int index)
        {
            return _contentPresenter;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _contentPresenter.Measure(Control.RenderSize);
            return Control.RenderSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _contentPresenter.Arrange(new Rect(finalSize));
            return finalSize;
        }
    }
}