using Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MVVMProject
{
    public class MyDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BoxItemTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is Box)
                return null /*BoxItemTemplate*/;

            return base.SelectTemplateCore(item);
        }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) => SelectTemplateCore(item);
    }
}