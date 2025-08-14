using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InspectionNet.Wpf.PocProject.UIElements
{
    /// <summary>
    /// StatusIndicatorControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StatusIndicatorControl : UserControl
    {
        public StatusIndicatorControl()
        {
            InitializeComponent();
        }



        public bool StatusValue
        {
            get { return (bool)GetValue(StatusValueProperty); }
            set { SetValue(StatusValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StatusValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusValueProperty =
            DependencyProperty.Register("StatusValue", typeof(bool), typeof(StatusIndicatorControl), new PropertyMetadata(StatusValuePropertyChanged));

        private static void StatusValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StatusIndicatorControl uc && e.NewValue is bool status)
            {
                uc.ellpse.Fill = status ? System.Windows.Media.Brushes.Lime : System.Windows.Media.Brushes.DimGray;
            }
        }
    }
}
