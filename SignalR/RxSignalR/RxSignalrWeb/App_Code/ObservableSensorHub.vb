Imports Microsoft.AspNet.SignalR
Imports Microsoft.VisualBasic

Public Class ObservableSensorHub
    Inherits Hub

    Public Sub SaySomething(something As String)
        Clients.All.someoneSaid(something)
    End Sub
End Class
