Imports System.Reactive.Linq
Imports System.Reactive.Threading.Tasks
Imports System.Net.Http

''' <summary>
''' An empty page that can be used on its own or navigated to within a Frame.
''' </summary>
Public NotInheritable Class BlankPage
    Inherits Page

    ''' <summary>
    ''' Invoked when this page is about to be displayed in a Frame.
    ''' </summary>
    ''' <param name="e">Event data that describes how this page was reached.  The Parameter
    ''' property is typically used to configure the page.</param>
    'IDisposable requestDisposable
    Private requestDisposable As IDisposable
    Protected Overrides Sub OnNavigatedTo(e As Navigation.NavigationEventArgs)
        Dim svc = New SimpleService.SimpleServicesvcClient

        requestDisposable = (From click In Observable.FromEventPattern(Of RoutedEventArgs)(SubmitButton, "Click")
                            From req In svc.DoSomethingCoolAsync(InputText.Text).ToObservable()
                            Select req).
                            ObserveOnDispatcher().
                            Subscribe(Sub(val) OutputText.Text = val)
    End Sub
    Protected Overrides Sub OnNavigatedFrom(e As Navigation.NavigationEventArgs)
        MyBase.OnNavigatedFrom(e)
        requestDisposable.Dispose()
        requestDisposable = Nothing
    End Sub

    Private Async Sub SubmitClicked() Handles SubmitButton.Click
        Dim svc = New SimpleService.SimpleServicesvcClient()

        Dim req = Await svc.DoSomethingCoolAsync(InputText.Text)

        OutputText.Text = req

        '   req.ObserveOnDispatcher().Subscribe(Sub(val) OutputText.Text = val)
    End Sub
End Class
