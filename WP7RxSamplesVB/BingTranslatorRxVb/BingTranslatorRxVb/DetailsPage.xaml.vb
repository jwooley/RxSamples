Partial Public Class DetailsPage
    Inherits PhoneApplicationPage
    ' Constructor
    Public Sub New()
        InitializeComponent()
    End Sub

    ' When page is navigated to set data context to selected item in list
    Protected Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
        Dim selectedIndex As String = ""
        If NavigationContext.QueryString.TryGetValue("selectedItem", selectedIndex) Then
            Dim index As Integer = Integer.Parse(selectedIndex)
            DataContext = App.ViewModel.Items(index)
        End If
    End Sub
End Class