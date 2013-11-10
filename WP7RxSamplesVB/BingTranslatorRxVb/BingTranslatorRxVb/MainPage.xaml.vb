Imports Microsoft.Phone.Reactive
Imports System.Collections.ObjectModel

Partial Public Class MainPage
    Inherits PhoneApplicationPage

    ' Constructor
    Public Sub New()
        InitializeComponent()
    End Sub

    Const _sourceLanguage = "en"
    Const PressDelay As Double = 1

    Protected Overrides Sub OnNavigatedTo(ByVal e As System.Windows.Navigation.NavigationEventArgs)
        SetupTranslator()
    End Sub

    Private Sub SetupTranslator()
        Try
            Dim results As New ObservableCollection(Of KeyValuePair(Of String, String))
            Translations.ItemsSource = results

            Dim translationTexts As IObservable(Of String) =
                (From keyup In Observable.FromEvent(Of KeyEventArgs)(Input, "KeyUp")
                 Where Not String.IsNullOrEmpty(Input.Text)
                 Select Input.Text).
                Throttle(TimeSpan.FromSeconds(PressDelay)).
                    ObserveOnDispatcher().
                    Do(Sub(x As String)
                           results.Clear()
                       End Sub)

            Dim destLanguages() = {"de", "es", "zh-CHT", "fr", "it", "ar", "ht", "he", "ja", "ko", "no", "ru", "th"}
            Dim query =
                From lang In destLanguages.ToObservable
                From text In translationTexts
                From res In text.TranslateAsync(_sourceLanguage, lang).
                    TakeUntil(translationTexts)
                Select Result = res, TargetLanguage = lang

            query.ObserveOnDispatcher.
                Subscribe(Sub(trans) results.Add(New KeyValuePair(Of String, String)(trans.TargetLanguage, DirectCast(trans.Result, Object())(0).ToString)))
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine(ex)
        End Try
    End Sub

End Class
