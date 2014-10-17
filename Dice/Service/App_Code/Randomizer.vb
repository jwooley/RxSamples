' NOTE: You can use the "Rename" command on the context menu to change the class name "Randomizer" in code, svc and config file together.
Public Class Randomizer
    Implements IRandomizer

    Public Function Roll(sideCount As Integer) As Integer Implements IRandomizer.Roll
        If sideCount = 3 Then
            Throw New ArgumentException("Invalid side count", "sideCount")
        End If

        Dim rand As New Random
        Dim randValue = rand.NextDouble

        'Threading.Thread.Sleep(randValue * 1000)
        Return CInt(randValue * (sideCount - 1)) + 1

    End Function

    Public Function RandomDiceResult(ByVal sideCount As Integer, ByVal index As Integer) As DiceContract Implements IRandomizer.RandomDiceResult
        Dim rand As New Random()
        'Threading.Thread.Sleep(1000)

        Return New DiceContract With { _
            .DotCount = CInt(rand.NextDouble * (sideCount - 1)) + 1, _
            .Index = index, _
            .SideCount = sideCount _
        }
    End Function


End Class
