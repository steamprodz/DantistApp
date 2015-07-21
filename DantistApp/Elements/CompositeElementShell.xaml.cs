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
using System.Text.RegularExpressions;

namespace DantistApp.Elements
{
    /// <summary>
    /// Interaction logic for GroupedImage.xaml
    /// </summary>
    public partial class CompositeElementShell : UserControl, IManipulatedElement
    {
        

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

        
        

        private int _centerDistance;
        public int CenterDistance
        {
            get { return _centerDistance; }

            set
            {
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

                        //if (image.Margin.Left < 0)
                        //{
                        //    this.Width += -image.Margin.Left;
                        //    image.Margin = new Thickness(0, image.Margin.Top,
                        //        image.Margin.Right + (-image.Margin.Left), image.Margin.Bottom);
                        //}
                        //if (image.Margin.Right < 0)
                        //{
                        //    this.Width += -image.Margin.Right;
                        //    image.Margin = new Thickness(image.Margin.Left + (-image.Margin.Right),
                        //        image.Margin.Top, 0, image.Margin.Bottom);
                        //}
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

        private double _size;
        public double Size
        {
            get { return _size; }

            set
            {
                if (Size != 0)
                {
                    Width /= (double)Size;

                }
                Width *= (double)value;

                _size = value;

                // TODO: Raise the NotifyPropertyChanged event here
                OnPropertyChanged("Size");
            }
        }

        //private double _sizeTop;
        //public double SizeTop
        //{
        //    get { return _sizeTop; }

        //    set
        //    {
        //        if (SizeTop != 0)
        //        {
        //            element_top.Width /= (double)SizeTop;
        //            element_top.Height /= (double)SizeTop;
        //        }
        //        element_top.Width *= (double)value;
        //        element_top.Height *= (double)value;

        //        _sizeTop = value;

        //        // TODO: Raise the NotifyPropertyChanged event here
        //        OnPropertyChanged("SizeTop");
        //    }
        //}

        //private double _sizeBot;
        //public double SizeBot
        //{
        //    get { return _sizeBot; }

        //    set
        //    {
        //        if (SizeBot != 0)
        //        {
        //            element_bot.Width /= (double)SizeBot;
        //            element_bot.Height /= (double)SizeBot;
        //        }
        //        element_bot.Width *= (double)value;
        //        element_bot.Height *= (double)value;

        //        _sizeBot = value;

        //        // TODO: Raise the NotifyPropertyChanged event here
        //        OnPropertyChanged("SizeBot");
        //    }
        //}

        private Point _startLocation;
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

        private string _compositeGroupID;
        public string CompositeGroupID
        {
            get { return _compositeGroupID; }
            set
            {
                _compositeGroupID = value;
                OnPropertyChanged("CompositeGroupID");
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

    
    }
}
