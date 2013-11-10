Imports System.IO
Imports System.Linq
Imports System.Reactive.Linq

Public Class FileWatcher

    Private Sub FileWatcher_Initialized(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Initialized
        Dim createWatcher As New FileSystemWatcher With {.Path = "c:\Temp", .EnableRaisingEvents = True}

        Dim AllEvents = Observable.FromEventPattern(Of FileSystemEventArgs)(createWatcher, "Created").
                    Merge(Observable.FromEventPattern(Of FileSystemEventArgs)(createWatcher, "Changed")).
                    Merge(Observable.FromEventPattern(Of FileSystemEventArgs)(createWatcher, "Deleted")).
                    DistinctUntilChanged.
                    ObserveOnDispatcher.
                    Do(Sub(fsArgs) RefreshFileList()).
                    Subscribe()

            RefreshFileList()

    End Sub

    Private Sub RefreshFileList()
        Dim files = From file In New DirectoryInfo("c:\temp").GetFiles
                    Order By file.LastWriteTime Descending
                    Select file.Name

        FileList.ItemsSource = files
    End Sub

End Class
