' NOTE: You can use the "Rename" command on the context menu to change the class name "Randomizer" in code, svc and config file together.
Public Class Randomizer
    Implements IRandomizer

    Public Function RandomDiceResult(ByVal sideCount As Integer) As Integer Implements IRandomizer.RandomDiceResult
        Dim rand As New Random()
        Return CInt(rand.NextDouble * (sideCount - 1)) + 1
    End Function


End Class
