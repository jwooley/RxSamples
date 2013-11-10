Public Class ConsoleObserver(Of T)
    Implements IObserver(Of T)

    Public Sub New(ByVal color As System.ConsoleColor)

    End Sub

    Private _color As System.ConsoleColor = ConsoleColor.White

    Public Sub OnCompleted() Implements System.IObserver(Of T).OnCompleted

    End Sub

    Public Sub OnError(ByVal [error] As System.Exception) Implements System.IObserver(Of T).OnError

    End Sub

    Public Sub OnNext(ByVal value As T) Implements System.IObserver(Of T).OnNext
        Console.ForegroundColor = _color
        Console.WriteLine(value.ToString())
    End Sub
End Class
