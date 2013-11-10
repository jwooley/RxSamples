using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;


namespace PhoneTranslator
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            // Insert code required on object creation below this point
            Items = new ObservableCollection<ItemViewModel>() {
                       new ItemViewModel{Key="la", Value="Maecenas praesent accumsan bibendum"},
                       new ItemViewModel{Key="es", Value="Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent"},
                       new ItemViewModel{Key="kl",Value="Habitant inceptos interdum lobortis"},
                       new ItemViewModel{Key="c#", Value="foo =&gt; foo.bar"},
                       new ItemViewModel{Key="vb", Value="Function(foo) foo.bar"},
                       new ItemViewModel{Key="rb", Value="Senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend"}};

        }

        public ObservableCollection<ItemViewModel> Items { get; private set; }

        private string sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return sampleProperty;
            }
            set
            {
                if (value != sampleProperty)
                {
                    sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        /// <summary> Sample ViewModel method; this method is invoked by a Behavior that is associated with it in the View</summary>
        public void SampleMethod()
        {
            SampleProperty = "SampleMethod invoked.";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}