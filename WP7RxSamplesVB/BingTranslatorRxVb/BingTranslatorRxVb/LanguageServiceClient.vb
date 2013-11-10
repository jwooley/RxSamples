Namespace BingTranslatorService
    Public Class LanguageServiceClient
        Public Function BeginTranslate(ByVal values As Object,
                                       ByVal callback As AsyncCallback,
                                       ByVal state As Object) As IAsyncResult
            Dim input = DirectCast(values, ServiceParams)
            Return (DirectCast(Me, BingTranslatorService.LanguageService)).BeginTranslate(input.AppID, input.Text, input.SourceLanguage, input.TargetLanguage, callback, "")
        End Function

        Public Overloads Sub TranslateAsync(ByVal values As Object)
            Dim input = DirectCast(values, ServiceParams)
            TranslateAsync(input.AppID, input.Text, input.Text, input.SourceLanguage, input.TargetLanguage)
        End Sub
    End Class
End Namespace