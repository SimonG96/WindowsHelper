using System;
using System.Windows;
using System.Windows.Controls;
using Lib.Tools;

namespace WindowsHelper.Settings
{
    public class SettingsEntryTemplateSelector : DataTemplateSelector //TODO: check if more Templates are needed
    {
        public DataTemplate CheckBoxTemplate { private get; set; }
        public DataTemplate NumericTemplate { private get; set; }
        public DataTemplate KeyCombinationTemplate { private get; set; }
        public DataTemplate EditTemplate { private get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(item is SettingsEntryViewModel entry))
                throw new InvalidOperationException("Item is not a Settings Entry View Model");

            if (entry.EntryType == typeof(bool))
                return CheckBoxTemplate;
            else if (entry.Value.IsNumericType())
                return NumericTemplate;
            else if (entry.EntryType == typeof(KeyCombination))
                return KeyCombinationTemplate;
            else
                return EditTemplate;
        }
    }
}