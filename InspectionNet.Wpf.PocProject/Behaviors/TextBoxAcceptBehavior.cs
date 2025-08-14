using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Xaml.Behaviors;

namespace InspectionNet.Wpf.PocProject.Behaviors
{
    public class TextBoxAcceptBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.KeyUp += OnKeyUp;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
       {
            if (e.Key == Key.Enter)
            {
                var be = AssociatedObject.GetBindingExpression(TextBox.TextProperty);
                be?.UpdateSource();
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.KeyDown -= OnKeyUp;
        }
    }
}
