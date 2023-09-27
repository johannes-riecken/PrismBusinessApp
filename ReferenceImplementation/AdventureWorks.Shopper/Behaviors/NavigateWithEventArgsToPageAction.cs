

using Microsoft.Xaml.Interactivity;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AdventureWorks.Shopper.Behaviors
{
    public class NavigateWithEventArgsToPageAction : DependencyObject, IAction
    {
        public string TargetPage { get; set; }
        public string EventArgsParameterPath { get; set; }
        object IAction.Execute(object sender, object parameter)
        {
            var propertyPathParts = EventArgsParameterPath.Split('.');
            object propertyValue = parameter;
            foreach (var propertyPathPart in propertyPathParts)
            {
                var propInfo = propertyValue.GetType().GetTypeInfo().GetDeclaredProperty(propertyPathPart);
                propertyValue = propInfo.GetValue(propertyValue);    
            }
            
            var pageType = Type.GetType(TargetPage);
            var frame = GetFrame(sender as DependencyObject);

            return frame.Navigate(pageType, propertyValue);
        }

        private Frame GetFrame(DependencyObject dependencyObject)
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);
            var parentFrame = parent as Frame;
            if (parentFrame != null) return parentFrame;
            return GetFrame(parent);
        }
    }
}
