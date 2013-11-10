using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Threading;
using System.Collections.ObjectModel;
using System.Concurrency;

namespace EnumerableObservable
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            
            var clickTest = from click in Observable.FromEvent<RoutedEventArgs>(this.ClickTestButton, "Click").TimeInterval()
                            where click.Interval.TotalMilliseconds < 200
                            select new Unit();
            clickTest.Subscribe(_ => Console.WriteLine("Double Click Detected - " + DateTime.Now));
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
            TotalTime.Text = "Total Time: " + sw.ElapsedTicks.ToString("n");
            
            Results.ItemsSource = query;
         
        }

        private void Observable_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            var list = new ObservableCollection<int>();
            Results.ItemsSource = list;
            sw.Start();
            
            var query = Enumerable.Range(1, 10).ToObservable().SubscribeOn(Scheduler.TaskPool)
                .Where(num => num % 2 == 0)
                .Do(num => Thread.SpinWait((10 - num) * 10000000));

            query.ObserveOnDispatcher().Subscribe(item => list.Add(item),
                () =>
                {
                    sw.Stop();
                    TotalTime.Text = "Total Time: " + sw.ElapsedTicks.ToString("n");
                });
            

        }
    }
}
