Imports Microsoft.VisualBasic
Imports SignalR.Hubs

Public Class ObservableSensorHub
    Inherits Hub

    Public Sub SaySomething(something As String)
        Clients.someoneSaid(something)
    End Sub
End Class
