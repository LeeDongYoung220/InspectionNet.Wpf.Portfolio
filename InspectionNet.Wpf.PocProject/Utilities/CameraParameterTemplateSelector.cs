using System.Windows;
using System.Windows.Controls;

using InspectionNet.Core.Implementations;
using InspectionNet.Core.Models;

namespace InspectionNet.Wpf.PocProject.Utilities
{
    public class CameraParameterTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IGenApiParameter parameter && container is FrameworkElement frameworkElement)
            {
                switch (parameter)
                {
                    case EnumParameter _:
                        return (DataTemplate)frameworkElement.FindResource("EnumParameterTemplate");
                    case StringParameter _:
                        return (DataTemplate)frameworkElement.FindResource("StringParameterTemplate");
                    case FloatParameter _:
                        return (DataTemplate)frameworkElement.FindResource("FloatParameterTemplate");
                    case IntegerParameter _:
                        return (DataTemplate)frameworkElement.FindResource("IntegerParameterTemplate");
                    case CommandParameter _:
                        return (DataTemplate)frameworkElement.FindResource("CommandParameterTemplate");
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
