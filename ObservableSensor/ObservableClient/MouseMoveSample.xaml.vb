Imports System.Reactive.Linq

Class MouseMoveSample

    Private Sub MouseMoveSample_Initialized(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Initialized

        Dim mouseDown = From evt In Observable.FromEventPattern(Of MouseButtonEventArgs)(image, "MouseDown")
                        Select evt.EventArgs.GetPosition(image)

        Dim mouseUp = Observable.FromEventPattern(Of MouseButtonEventArgs)(Me, "MouseUp")
        Dim mouseMove = From evt In Observable.FromEventPattern(Of MouseEventArgs)(Me, "MouseMove")
                        Select evt.EventArgs.GetPosition(Me)

        Dim q = From startLocation In mouseDown
                From endLocation In mouseMove.TakeUntil(mouseUp)
                Select X = endLocation.X - startLocation.X,
                    Y = endLocation.Y - startLocation.Y

        q.Subscribe(Sub(value)
                        Canvas.SetLeft(image, value.X)
                        Canvas.SetTop(image, value.Y)
                    End Sub)

    End Sub
End Class
