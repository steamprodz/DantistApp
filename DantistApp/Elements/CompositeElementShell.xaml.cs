﻿using System;
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
    public partial class CompositeElementShell : UserControl, IManipulatedElement
    {
        //Currently moving object
        private Image MovingImage;

        // Mouse tools
        private Point MousePosition;


        public CompositeElementShell()
        {
            InitializeComponent();
            DataContext = this;
        }

        public bool IsFixed { get; set; }

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

        public static readonly DependencyProperty SourceTopProperty =
            DependencyProperty.Register("SourceTop", typeof(ImageSource), typeof(CompositeElementShell));

        public static readonly DependencyProperty SourceBotProperty =
            DependencyProperty.Register("SourceBot", typeof(ImageSource), typeof(CompositeElementShell));



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
                //Height = image_bot.ActualHeight + value;

                //image_bot.Margin = new Thickness(0, value, 0, 0);

                _centerDistance = value;

                OnPropertyChanged("CenterDistance");
            }
        }

        private double _horizontalShift;

        public double HorizontalShift
        {
            get { return _horizontalShift; }

            set
            {
                foreach (Image image in (this.Content as Grid).Children)
                {
                    if (image.VerticalAlignment == System.Windows.VerticalAlignment.Bottom)
                    {
                        if (value != HorizontalShift)
                        {
                            image.Margin = new Thickness(image.Margin.Left - HorizontalShift, image.Margin.Top,
                            image.Margin.Right + HorizontalShift, image.Margin.Bottom);
                        }

                        image.Margin = new Thickness(image.Margin.Left + value, image.Margin.Top,
                            image.Margin.Right - value, image.Margin.Bottom);
                    }
                }

                _horizontalShift = value;

                OnPropertyChanged("HorizontalShift");
            }
        }

        private double _botImageSize;

        public double BotImageSize
        {
            get { return _botImageSize; }

            set
            {
                foreach (Image image in (this.Content as Grid).Children)
                {
                    if (image.VerticalAlignment == System.Windows.VerticalAlignment.Bottom)
                    {
                        //if (BotImageSize != 0)
                        //{
                        //    image.Width = BotImageSize;
                        //    //image.Height /= BotImageSize;
                        //}
                        ////image.Height *= value;
                        //image.Width *= value;

                        if (value != BotImageSize)
                        {
                            image.Margin = new Thickness(image.Margin.Left + BotImageSize, image.Margin.Top + BotImageSize,
                            image.Margin.Right, image.Margin.Bottom);
                        }

                        image.Margin = new Thickness(image.Margin.Left - value, image.Margin.Top - value,
                            image.Margin.Right, image.Margin.Bottom);
                    }
                }

                _botImageSize = value;

                OnPropertyChanged("BotImageSize");
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
                    //Height /= (double)Size;
                    //image_bot.Width /= (double)Size;
                    //image_top.Width /= (double)Size;
                    //image_bot.Height /= (double)Size;
                    //image_top.Height /= (double)Size;

                }
                Width *= (double)value;
                //Height *= (double)value;
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
            //var image = e.Source as Image;

            //if (image != null && ControlCanvas.CaptureMouse())
            //{
            //    MousePosition = e.GetPosition(ControlCanvas);
            //    MovingImage = image;
            //    //Panel.SetZIndex(MovingImage, 1); // in case of multiple images
            //}
        }

        private void CanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //if (MovingImage != null)
            //{
            //    ControlCanvas.ReleaseMouseCapture();
            //    //Panel.SetZIndex(MovingImage, 0);
            //    MovingImage = null;
            //}
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (MovingImage != null)
            {
                //var position = e.GetPosition(ControlCanvas);
                //var offset = position - MousePosition;
                //MousePosition = position;
                //Canvas.SetLeft(MovingImage, Canvas.GetLeft(MovingImage) + offset.X);
                //Canvas.SetTop(MovingImage, Canvas.GetTop(MovingImage) + offset.Y);

            }
        }

        private void MyCompositeElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //DropShadowEffect glowEffect = new DropShadowEffect()
            //{
            //    ShadowDepth = 0,
            //    Color = Colors.Red,
            //    Opacity = 1,
            //    BlurRadius = 10
            //};

            //this.Effect = glowEffect;
        }

    }
}
