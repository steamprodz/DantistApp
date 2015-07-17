using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DantistApp
{
    /// <summary>
    /// Interaction logic for GroupedImage.xaml
    /// </summary>
    public partial class GroupedImage : UserControl
    {
        //Currently moving object
        private Image MovingImage;

        // Mouse tools
        private Point MousePosition;

        //Image binding
        private bool IsBinded;

        public GroupedImage()
        {
            InitializeComponent();
            DataContext = this;

            // Related positioning (bottom image to top image, based on CenterDistance parameter)
            //Canvas.SetTop(image_bot, Canvas.GetTop(image_top) + (double)CenterDistance);
        }

        public ImageSource SourceTop
        {
            get { return base.GetValue(SourceTopProperty) as ImageSource; }
            set { base.SetValue(SourceTopProperty, value); }
        }

        public ImageSource SourceBot
        {
            get { return base.GetValue(SourceBotProperty) as ImageSource; }
            set { base.SetValue(SourceBotProperty, value); }
        }

        public int? CenterDistance
        {
            get { return base.GetValue(CenterDistanceProperty) as int?; }
            set { base.SetValue(CenterDistanceProperty, value); }
        }

        public static readonly DependencyProperty SourceTopProperty =
            DependencyProperty.Register("SourceTop", typeof(ImageSource), typeof(GroupedImage));

        public static readonly DependencyProperty SourceBotProperty =
            DependencyProperty.Register("SourceBot", typeof(ImageSource), typeof(GroupedImage));

        public static readonly DependencyProperty CenterDistanceProperty =
            DependencyProperty.Register("CenterDistance", typeof(int?), typeof(GroupedImage));


        //erlgmelrgmwlerkhm


        private void CanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = e.Source as Image;

            if (image != null && controlCanvas.CaptureMouse())
            {
                MousePosition = e.GetPosition(controlCanvas);
                MovingImage = image;
                //Panel.SetZIndex(MovingImage, 1); // in case of multiple images
            }
        }

        private void CanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MovingImage != null)
            {
                controlCanvas.ReleaseMouseCapture();
                //Panel.SetZIndex(MovingImage, 0);
                MovingImage = null;
            }
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (MovingImage != null)
            {
                var position = e.GetPosition(controlCanvas);
                var offset = position - MousePosition;
                MousePosition = position;
                Canvas.SetLeft(MovingImage, Canvas.GetLeft(MovingImage) + offset.X);
                Canvas.SetTop(MovingImage, Canvas.GetTop(MovingImage) + offset.Y);

                Image bindedImage;

                if (MovingImage == image_bot)
                {
                    bindedImage = image_bot;
                }
                else
                {
                    bindedImage = image_top;
                }

                var halfWidth = MovingImage.Width / 2;
                var halfHeight = MovingImage.Height / 2;

                //Canvas.SetLeft(bindedImage, Canvas.GetLeft(bindedImage) + offset.X);
                //Canvas.SetTop(bindedImage, Canvas.GetTop(bindedImage) + offset.Y);
            }
        }

    }
}
