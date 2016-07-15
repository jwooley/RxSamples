Imports Microsoft.VisualBasic
Imports Microsoft.AspNet.SignalR
Imports Owin

Public Class Startup
    Public Sub Configuration(app As IAppBuilder)
        app.MapSignalR()
    End Sub
End Class
