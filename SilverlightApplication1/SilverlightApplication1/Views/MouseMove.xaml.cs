﻿using System;
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
using System.Windows.Navigation;
using System.Reactive.Linq;

namespace SilverlightApplication1.Views
{
    public partial class MouseMove : Page
    {
        public MouseMove()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            var mouseDown = from evt in Observable.FromEventPattern<MouseButtonEventArgs>(image, "MouseLeftButtonDown")
                            select evt.EventArgs.GetPosition(image);
            var mouseUp = Observable.FromEventPattern<MouseButtonEventArgs>(this, "MouseLeftButtonUp");
            var mouseMove = Observable.FromEventPattern<MouseEventArgs>(this, "MouseMove")
                .Select(evt => evt.EventArgs.GetPosition(this));

            var q = from startLocation in mouseDown
                    from endLocation in mouseMove.TakeUntil(mouseUp)
                    select new
                    {
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
