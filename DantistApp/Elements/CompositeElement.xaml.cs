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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DantistApp.Elements
{
    /// <summary>
    /// Interaction logic for GroupedImage.xaml
    /// </summary>
    public partial class CompositeElement : UserControl, IControlManipulate
    {
        private Canvas ControlCanvas;

        //Currently moving object
        private Image MovingImage;

        // Mouse tools
        private Point MousePosition;

        //Image binding
        private bool IsBinded;

        public CompositeElement()
        {
            InitializeComponent();
            DataContext = this;

            IsBinded = true;
        }

        //public double ElementWidth
        //{
        //    get { return (double)base.GetValue(ElementWidthProperty); }
        //    set { base.SetValue(ElementWidthProperty, value); }
        //}

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

        //public int CenterDistance
        //{
        //    get { return (int)base.GetValue(CenterDistanceProperty); }

        //    set 
        //    {
        //        base.SetValue(MarginProperty, new Thickness(0, value, 0, -value));
        //        image_bot.Margin = new Thickness(20, 20, 20, 20);
        //        base.SetValue(CenterDistanceProperty, value); 
        //    }
        //}

        public static readonly DependencyProperty SourceTopProperty =
            DependencyProperty.Register("SourceTop", typeof(ImageSource), typeof(CompositeElement));

        public static readonly DependencyProperty SourceBotProperty =
            DependencyProperty.Register("SourceBot", typeof(ImageSource), typeof(CompositeElement));

        //public static readonly DependencyProperty CenterDistanceProperty =
        //    DependencyProperty.Register("CenterDistance", typeof(int), typeof(CompositeElement));

        //public static readonly DependencyProperty ElementWidthProperty =
        //    DependencyProperty.Register("ElementWidth", typeof(double), typeof(CompositeElement));



        #region IControlManipulate Implementation + CenterDistance Property

        public event PropertyChangedEventHandler PropertyChanged;

        private double _size;
        private Point _startLocation;

        private int _centerDistance;

        public int CenterDistance
        {
            get { return _centerDistance; }

            set
            {
                Height = image_bot.ActualHeight + value;

                image_bot.Margin = new Thickness(0, value, 0, 0);

                _centerDistance = value;

                OnPropertyChanged("CenterDistance");
            }
        }

        public double Size
        {
            get { return _size; }

            set
            {
                if (Size != 0)
                {
                    Width /= (double)Size;
                    Height /= (double)Size;
                    //image_bot.Width /= (double)Size;
                    //image_top.Width /= (double)Size;
                    //image_bot.Height /= (double)Size;
                    //image_top.Height /= (double)Size;

                }
                Width *= (double)value;
                Height *= (double)value;
                //image_bot.Width *= (double)value;
                //image_top.Width *= (double)value;
                //image_bot.Height *= (double)value;
                //image_top.Height *= (double)value;

                _size = value;

                // TODO: Raise the NotifyPropertyChanged event here
                OnPropertyChanged("Size");
            }
        }

        public Point StartLocation
        {
            get { return _startLocation; }

            set
            {
                Canvas.SetLeft(this, value.X);
                Canvas.SetTop(this, value.Y);

                _startLocation = value;

                // TODO: Raise the NotifyPropertyChanged event here
                OnPropertyChanged("StartLocation");
            }
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void CanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = e.Source as Image;

            if (image != null && ControlCanvas.CaptureMouse())
            {
                MousePosition = e.GetPosition(ControlCanvas);
                MovingImage = image;
                //Panel.SetZIndex(MovingImage, 1); // in case of multiple images
            }
        }

        private void CanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MovingImage != null)
            {
                ControlCanvas.ReleaseMouseCapture();
                //Panel.SetZIndex(MovingImage, 0);
                MovingImage = null;
            }
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (MovingImage != null)
            {
                var position = e.GetPosition(ControlCanvas);
                var offset = position - MousePosition;
                MousePosition = position;
                Canvas.SetLeft(MovingImage, Canvas.GetLeft(MovingImage) + offset.X);
                Canvas.SetTop(MovingImage, Canvas.GetTop(MovingImage) + offset.Y);

                if (IsBinded)
                {
                    Image bindedImage;

                    if (MovingImage == image_bot)
                    {
                        bindedImage = image_top;
                    }
                    else
                    {
                        bindedImage = image_bot;
                    }

                    var halfWidth = MovingImage.Width / 2;
                    var halfHeight = MovingImage.Height / 2;

                    Canvas.SetLeft(bindedImage, Canvas.GetLeft(bindedImage) + offset.X);
                    Canvas.SetTop(bindedImage, Canvas.GetTop(bindedImage) + offset.Y);
                }
            }
        }

        private void MyCompositeElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DropShadowEffect glowEffect = new DropShadowEffect()
            {
                ShadowDepth = 0,
                Color = Colors.Red,
                Opacity = 1,
                BlurRadius = 10
            };

            MyCompositeElement.Effect = glowEffect;
        }

    }
}
