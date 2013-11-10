Imports Microsoft.Phone.Reactive

Partial Public Class MainPage
    Inherits PhoneApplicationPage

    ' Constructor
    Public Sub New()
        InitializeComponent()
        MouseEventHandler()
    End Sub

    Private Sub MouseEventHandler()
        Dim mouseDown = From evt In Observable.FromEvent(Of MouseButtonEventArgs)(image, "MouseLeftButtonDown")
                            Select evt.EventArgs.GetPosition(image)
        Dim mouseUp = Observable.FromEvent(Of MouseButtonEventArgs)(Me, "MouseLeftButtonUp")
        Dim mouseMove = From evt In Observable.FromEvent(Of MouseEventArgs)(Me, "MouseMove")
                        Select evt.EventArgs.GetPosition(Me)

        Dim q = From startLocation In mouseDown
                From endLocation In mouseMove.TakeUntil(mouseUp)
                Select New With {
                    .X = endLocation.X - startLocation.X,
                    .Y = endLocation.Y - startLocation.Y
                }

        q.Subscribe(Sub(value)
                        Canvas.SetLeft(image, value.X)
                        Canvas.SetTop(image, value.Y)

                    End Sub)
    End Sub
End Class
