using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Reactive.Linq;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR.Client;

namespace WpfSharp
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
        ObservableCollection<SensorData> items;

        protected override async void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            items = new ObservableCollection<SensorData>();
            ItemList.ItemsSource = items;

            // Connect to the service
            var hubConnection = new HubConnection("http://localhost:4734/");

            // Create a proxy to the chat service
            var chat = hubConnection.CreateHubProxy("observableSensorHub");

            // Print the message when it comes in
            //chat.On<SensorData>("broadcast", value =>
            //    Dispatcher.BeginInvoke(new Action(() => items.Insert(0, value))));

            chat.Observe("broadcast")
                .Select(item => JsonConvert.DeserializeObject<SensorData>(item[0].ToString()))
                .Where(item => item.Category == "1")
                .ObserveOnDispatcher()
                .Subscribe(item => items.Insert(0, item));

            // Start the connection
            await hubConnection.Start();

        }
    }
}
