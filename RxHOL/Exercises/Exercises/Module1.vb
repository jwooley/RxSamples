Imports System.Windows.Forms
Imports System.Disposables
Imports System.Runtime.CompilerServices
Imports Exercises.DictionarySuggestService


Module Module1
    Sub main()
        Dim txt = New TextBox
        Dim lst = New ListBox With {.Top = txt.Height + 10}
        Dim frm = New Form
        frm.Controls.AddRange({txt, lst})

        ' Turn the user input into a tamed sequence of strings.
        'Dim input = (From evt In Observable.FromEvent(Of EventArgs)(txt, "TextChanged")
        '             Select DirectCast(evt.Sender, TextBox).Text).
        '            DistinctUntilChanged().
        '            Throttle(TimeSpan.FromSeconds(1))


        Const VALUE As String = "reactive"
        Dim rand = New Random
        Dim randTimespan As Func(Of Integer, TimeSpan) = Function(x) TimeSpan.FromMilliseconds(CDbl(rand.Next(200, 1200)))
        Dim input = Observable.GenerateWithTime(Of Integer, String)(3,
                                                                  Function(leng) leng <= VALUE.Length,
                                                                  Function(leng As Integer) leng + 1,
                                                                  Function(leng As Integer) VALUE.Substring(0, leng),
                                                                  randTimespan)

        'Dim input = {"reac", "reactive", "bing"}.ToObservable

        ' Bridge with the web service's MatchInDict method.
        Dim svc = New DictServiceSoapClient("DictServiceSoap")
        Dim matchInDict = Observable.FromAsyncPattern(Of String, String, String, DictionaryWord())(
            AddressOf svc.BeginMatchInDict, AddressOf svc.EndMatchInDict)


        'Dim matchInWordsNetByPrefix As Func(Of String, IObservable(Of DictionaryWord())) =
        '    Function(term)
        '        Observable.Return(
        '            (From i In Enumerable.Range(0, rand.Next(0, 50))
        '            Select New DictionaryWord With {.Word = term & i.ToString}).
        '            ToArray()).
        '            Delay(TimeSpan.FromSeconds(rand.Next(1, 10)))
        '    End Function

        Dim matchInWordsNetByPrefix As Func(Of String, IObservable(Of DictionaryWord())) =
            Function(term) matchInDict("wn", term, "prefix")

        ' The grand composition connecting the user input with the web service.
        Dim res = From term In input
                  From words In matchInWordsNetByPrefix(term).TakeUntil(input)
                  Select words

        Dim res1 = (From term In input
                    Select matchInWordsNetByPrefix(term)).
                    Switch()

        ' Synchronize with the UI Thread and populate the ListBox or signal an error.
        Using res1.ObserveOn(lst).Subscribe(
            Sub(words)
                lst.Items.Clear()
                lst.Items.AddRange((From word In words
                                    Select word.Word).ToArray)
            End Sub,
            Sub(ex)
                MessageBox.Show(
                    "An error occurred: " & ex.Message, frm.Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Sub)
            Application.Run(frm)
        End Using 'Proper Disposal happens upon exiting the application
       

    End Sub
    Sub Ex8()
        Dim txt = New TextBox
        Dim lst = New ListBox With {.Top = txt.Height + 10}
        Dim frm = New Form
        frm.Controls.AddRange({txt, lst})

        ' Turn the user input into a tamed sequence of strings.
        Dim textChanged = From evt In Observable.FromEvent(Of EventArgs)(txt, "TextChanged")
                          Select DirectCast(evt.Sender, TextBox).Text


        Dim input = (From evt In Observable.FromEvent(Of EventArgs)(txt, "TextChanged")
                     Select DirectCast(evt.Sender, TextBox).Text).
                    DistinctUntilChanged().
                    Throttle(TimeSpan.FromSeconds(1))




        ' Bridge with the web service's MatchInDict method.
        Dim svc = New DictServiceSoapClient("DictServiceSoap")
        Dim matchInDict = Observable.FromAsyncPattern(Of String, String, String, DictionaryWord())(
            AddressOf svc.BeginMatchInDict, AddressOf svc.EndMatchInDict)

        Dim matchInWordsNetByPrefix As Func(Of String, IObservable(Of DictionaryWord())) =
            Function(term) matchInDict("wn", term, "prefix")

        ' The grand composition connecting the user input with the web service.
        Dim res = From term In input
                  From words In matchInWordsNetByPrefix(term).TakeUntil(input)
                  Select words

        Dim res1 = (From term In input
                    Select matchInWordsNetByPrefix(term)).
                    Switch()

        ' Synchronize with the UI Thread and populate the ListBox or signal an error.
        Using res.ObserveOn(lst).Subscribe(
            Sub(words)
                lst.Items.Clear()
                lst.Items.AddRange((From word In words
                                    Select word.Word).ToArray)
            End Sub,
            Sub(ex)
                MessageBox.Show(
                    "An error occurred: " & ex.Message, frm.Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Sub)
            Application.Run(frm)
        End Using 'Proper Disposal happens upon exiting the application



        'Dim res = matchInWordsNetByPrefix("r")
        'Dim subscription = res.Subscribe(Sub(words)
        '                                     For Each word In words
        '                                         Console.WriteLine(word.Word)
        '                                     Next
        '                                 End Sub,
        '                                 Sub(ex)
        '                                     Console.WriteLine(ex.Message)
        '                                 End Sub)

        'Console.ReadLine()


        'Dim input = (From evt In Observable.FromEvent(Of EventArgs)(txt, "TextChanged")
        '            Select DirectCast(evt.Sender, TextBox).Text).
        '            Throttle(TimeSpan.FromSeconds(1)).
        '            DistinctUntilChanged()

        'Using input.ObserveOn(lbl).Subscribe(Sub(inp) lbl.Text = inp)
        '    Application.Run(frm)
        'End Using
    End Sub
















    Private Sub frm_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
        Dim position = e.Location
        If position.X = position.Y Then
            ' Only want to respond to events for mouse moves
            ' over the first bisector of the form
        End If
    End Sub

    Private Sub txt_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim text = DirectCast(sender, TextBox).Text
        ' And now we can forget about sender and e parameters
    End Sub

End Module


<Extension()>
Public Module Sample1

    <Extension()>
    Public Function LogTimestampedValues(Of T)(ByVal source As IObservable(Of T),
                                            ByVal onNext As Action(Of Timestamped(Of T))) As IObservable(Of T)
        Return source.Timestamp.Do(onNext).RemoveTimestamp
    End Function

    Sub Main()
        Dim source As IObservable(Of Integer) = Observable.GenerateWithTime(
         0,
         Function(i) i < 5,
         Function(i) i + 1,
         Function(i) i * i,
         Function(i) TimeSpan.FromSeconds(i))

        Using source.Subscribe(
                Sub(x) Console.WriteLine("OnNext: {0}", x),
                Sub(ex) Console.WriteLine("OnError: {0}", ex.Message),
                Sub() Console.WriteLine("OnCompleted")
            )

            Console.WriteLine("Press ENTER to unsubscribe...")
            Console.ReadLine()
        End Using
    End Sub
End Module