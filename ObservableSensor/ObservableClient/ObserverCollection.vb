Imports System.Collections.ObjectModel

Public Class ObserverCollection(Of T)
    Inherits ObservableCollection(Of T)
    Implements IObserver(Of T)


    Public Sub OnCompleted() Implements System.IObserver(Of T).OnCompleted

    End Sub

    Public Sub OnError(ByVal [error] As System.Exception) Implements System.IObserver(Of T).OnError

    End Sub

    Public Sub OnNext(ByVal value As T) Implements System.IObserver(Of T).OnNext
        Me.Add(value)
    End Sub
End Class
