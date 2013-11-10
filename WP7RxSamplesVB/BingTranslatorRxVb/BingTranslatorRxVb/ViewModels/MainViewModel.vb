Imports System.ComponentModel
Imports System.Collections.ObjectModel

Public Class MainViewModel
    Implements INotifyPropertyChanged

    Public Property Items As ObservableCollection(Of ItemViewModel)

    Public Sub New()
        Items = New ObservableCollection(Of ItemViewModel) From {
                    New ItemViewModel With {.Key = "la", .Value = "Maecenas praesent accumsan bibendum"},
                    New ItemViewModel With {.Key = "es", .Value = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent"},
                    New ItemViewModel With {.Key = "kl", .Value = "Habitant inceptos interdum lobortis"},
                    New ItemViewModel With {.Key = "c#", .Value = "foo =&gt; foo.bar"},
                    New ItemViewModel With {.Key = "vb", .Value = "Function(foo) foo.bar"},
                    New ItemViewModel With {.Key = "rb", .Value = "Senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend"}}
    End Sub


    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Private Sub NotifyPropertyChanged(ByVal propertyName As String)
        Dim handler As PropertyChangedEventHandler = PropertyChangedEvent
        If handler IsNot Nothing Then
            handler(Me, New PropertyChangedEventArgs(propertyName))
        End If
    End Sub
End Class