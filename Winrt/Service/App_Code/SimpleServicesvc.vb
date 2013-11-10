Imports System.Threading

Public Class SimpleServicesvc
    Implements ISimpleServicesvc

    Public Function DoSomethingCool(input As String) As String Implements ISimpleServicesvc.DoSomethingCool
        Dim rnd = New Random()
        Thread.Sleep(rnd.NextDouble() * 1000)
        Return (String.Join("", From letter In input.ToCharArray()
               Order By letter
               Distinct))
    End Function

End Class
