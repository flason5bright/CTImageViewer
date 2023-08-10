using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CTImageViewer.UserControls
{
    public class OperationButton : Button
    {


        public ImageSource FirstImage
        {
            get { return (ImageSource)GetValue(FirstImageProperty); }
            set { SetValue(FirstImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FirstImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FirstImageProperty =
            DependencyProperty.Register("FirstImage", typeof(ImageSource), typeof(OperationButton), new PropertyMetadata(null));

        public ImageSource SecondImage
        {
            get { return (ImageSource)GetValue(SecondImageProperty); }
            set { SetValue(SecondImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FirstImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondImageProperty =
            DependencyProperty.Register("SecondImage", typeof(ImageSource), typeof(OperationButton), new PropertyMetadata(null));

    }
}