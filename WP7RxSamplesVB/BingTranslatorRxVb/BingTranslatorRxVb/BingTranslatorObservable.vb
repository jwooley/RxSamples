Imports System.Runtime.CompilerServices
Imports Microsoft.Phone.Reactive

Public Module BingTranslatorObservable
    ''' <summary>
    ''' The AppId identifies your application at the Bing online service.
    ''' Before running the application, you have to get your free AppId
    ''' here: http://www.bing.com/developers/createapp.aspx
    ''' </summary>
    Const AppID As String = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"

    <Extension()>
    Public Function TranslateAsync(ByVal text As String,
                                   ByVal sourceLanguage As String,
                                   ByVal targetLanguage As String
                                   ) As IObservable(Of Object())
        Dim svc As New BingTranslatorService.LanguageServiceClient
        Dim values As New ServiceParams With {
            .AppID = AppID,
            .SourceLanguage = sourceLanguage,
            .TargetLanguage = targetLanguage,
            .Text = text}

        Dim obs = Observable.FromAsyncPattern(Of ServiceParams, Object())(AddressOf svc.BeginTranslate, AddressOf svc.OnEndTranslate)(values).
            ObserveOnDispatcher
        Return obs

    End Function
End Module