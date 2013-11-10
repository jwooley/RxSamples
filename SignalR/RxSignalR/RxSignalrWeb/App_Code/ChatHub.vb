Option Strict Off

Imports Microsoft.VisualBasic
Imports SignalR.Hubs

Public Class Chat
    Inherits Hub

    Public Sub Send(message As String)
        Clients.addMessage(message)
    End Sub
End Class
