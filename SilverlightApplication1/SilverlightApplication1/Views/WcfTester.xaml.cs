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
using System.Windows.Navigation;
using SilverlightApplication1.ReactiveService;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace SilverlightApplication1.Views
{
    public partial class WcfTester : Page
    {
        public WcfTester()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var svc = new ReactiveService.LongRunningServiceClient();

            IObservable<string> inputChanges =
               (from keyup in Observable.FromEventPattern<KeyEventArgs>(this.Input, "KeyUp")
                select Input.Text)
               .Throttle(TimeSpan.FromSeconds(1))
               .ObserveOnDispatcher()
               .Do(val => svc.SortItAsync(val))
               ;

            var svcCompleted = Observable.FromEventPattern<SortItCompletedEventArgs>(svc, "SortItCompleted");

            var results = from change in inputChanges
                          from res in svcCompleted
                            .TakeUntil(inputChanges)
                          select res;

            ObservableCollection<string> resultList = new ObservableCollection<string>();
            destList.ItemsSource = resultList;

            results.ObserveOnDispatcher().Subscribe(res => resultList.Add(res.EventArgs.Result));


        }

    }
}
