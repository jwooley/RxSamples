using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Reactive;

namespace PhoneDragDrop
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            MouseEventHandler();
        }

        private void MouseEventHandler()
        {
            var mouseDown = from evt in Observable.FromEvent<MouseButtonEventArgs>(image, "MouseLeftButtonDown")
                            select evt.EventArgs.GetPosition(image);
            var mouseUp = Observable.FromEvent<MouseButtonEventArgs>(this, "MouseLeftButtonUp");
            var mouseMove = from evt in Observable.FromEvent<MouseEventArgs>(this, "MouseMove")
                            select evt.EventArgs.GetPosition(this);

            var q = from startLocation in mouseDown
                    from endLocation in mouseMove.TakeUntil(mouseUp)
                    select new { 
                        X = endLocation.X - startLocation.X, 
                        Y = endLocation.Y - startLocation.Y 
                    };

            q.Subscribe(value =>
            {
                Canvas.SetLeft(image, value.X);
                Canvas.SetTop(image, value.Y);
            });
        }
    }
}
