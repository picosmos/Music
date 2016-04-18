using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Koopakiller.Apps.MusicManager.UI
{
    public class AutoTypeTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
            {
                return null;
            }
            var typeName = item.GetType().FullName;
            typeName = typeName.Replace("ViewModel", "View");
            var type = Type.GetType(typeName);
            var viewModelType = item.GetType();
            if (type?.Namespace == null || viewModelType.Namespace == null)
            {
                return null;
            }
            

            var xaml = $"<DataTemplate DataType=\"{{x:Type viewModel:{viewModelType.Name}}}\"><view:{type.Name} /></DataTemplate>";

            var context = new ParserContext { XamlTypeMapper = new XamlTypeMapper(new string[0]) };

            context.XamlTypeMapper.AddMappingProcessingInstruction("viewModel", viewModelType.Namespace, viewModelType.Assembly.FullName);
            context.XamlTypeMapper.AddMappingProcessingInstruction("view", type.Namespace, type.Assembly.FullName);

            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            context.XmlnsDictionary.Add("viewModel", "viewModel");
            context.XmlnsDictionary.Add("view", "view");

            return (DataTemplate)XamlReader.Parse(xaml, context);
        }
    }
}
