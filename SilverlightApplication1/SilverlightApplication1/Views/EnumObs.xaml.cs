﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SilverlightApplication1.Views
{

    public partial class EnumObs : Page
    {
        public EnumObs()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Enumerable_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            var query = Enumerable.Range(1, 10)
                .Where(num => num % 2 == 0)
                .Do(num => Thread.SpinWait((10 - num) * 10000000))
                .ToList();

            sw.Stop();

            Results.ItemsSource = query;
            TotalTime.Text = "Total Time: " + sw.ElapsedTicks.ToString("n");
        }

        private void Observable_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            var query = Enumerable.Range(1, 10)
                .Where(num => num % 2 == 0)
                .Do(num => Thread.SpinWait((10 - num) * 10000000))
                .ToList();

            sw.Stop();

            Results.ItemsSource = query;
            TotalTime.Text = "Total Time: " + sw.ElapsedTicks.ToString("n");

        }

        //private void Observable_Click_After(object sender, RoutedEventArgs e)
        //{
        //    Stopwatch sw = new Stopwatch();
        //    var items = new ObservableCollection<int>();
        //    Results.ItemsSource = items;

        //    sw.Start();
        //    var query = Enumerable.Range(1, 10).ToObservable().SubscribeOn(NewThreadScheduler.Default)
        //        .Where(num => num % 2 == 0)
        //        .Do(num => Thread.SpinWait((10 - num) * 10000000))
        //        .ObserveOnDispatcher()
        //        .Subscribe(num => items.Add(num),
        //            () =>
        //            {
        //                sw.Stop();
        //                TotalTime.Text = "Total Time: " + sw.ElapsedTicks.ToString("n");
        //            })
        //        ;

        //}

    }
}
