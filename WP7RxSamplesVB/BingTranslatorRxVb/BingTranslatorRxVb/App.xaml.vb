Partial Public Class App
    Inherits Application

    Private Shared _viewModel As MainViewModel = Nothing

    ''' <summary>
    ''' A static ViewModel used by the views to bind against.
    ''' </summary>
    ''' <returns>The MainViewModel object.</returns>
    Public Shared ReadOnly Property ViewModel() As MainViewModel
        Get
            ' Delay creation of the view model until necessary
            If _viewModel Is Nothing Then
                _viewModel = New MainViewModel()
            End If

            Return _viewModel
        End Get
    End Property

    ''' <summary>
    ''' Provides easy access to the root frame of the Phone Application.
    ''' </summary>
    ''' <returns>The root frame of the Phone Application.</returns>
    Public Property RootFrame As PhoneApplicationFrame

    ''' <summary>
    ''' Constructor for the Application object.
    ''' </summary>
    Public Sub New()
        ' Global handler for uncaught exceptions. 
        AddHandler UnhandledException, AddressOf Application_UnhandledException

        ' Show graphics profiling information while debugging.
        If Diagnostics.Debugger.IsAttached Then
            ' Display the current frame rate counters.
            Application.Current.Host.Settings.EnableFrameRateCounter = True

            ' Show the areas of the app that are being redrawn in each frame.
            'Application.Current.Host.Settings.EnableRedrawRegions = True

            ' Enable non-production analysis visualization mode, 
            ' which shows areas of a page that are being GPU accelerated with a colored overlay.
            'Application.Current.Host.Settings.EnableCacheVisualization = True
        End If

        ' Standard Silverlight initialization
        InitializeComponent()

        ' Phone-specific initialization
        InitializePhoneApplication()
    End Sub

    ' Code to execute when the application is launching (eg, from Start)
    ' This code will not execute when the application is reactivated
    Private Sub Application_Launching(ByVal sender As Object, ByVal e As LaunchingEventArgs)
    End Sub

    ' Code to execute when the application is activated (brought to foreground)
    ' This code will not execute when the application is first launched
    Private Sub Application_Activated(ByVal sender As Object, ByVal e As ActivatedEventArgs)
        ' Ensure that application state is restored appropriately
    End Sub

    ' Code to execute when the application is deactivated (sent to background)
    ' This code will not execute when the application is closing
    Private Sub Application_Deactivated(ByVal sender As Object, ByVal e As DeactivatedEventArgs)
        ' Ensure that required application state is persisted here.
    End Sub

    ' Code to execute when the application is closing (eg, user hit Back)
    ' This code will not execute when the application is deactivated
    Private Sub Application_Closing(ByVal sender As Object, ByVal e As ClosingEventArgs)
    End Sub

    ' Code to execute if a navigation fails
    Private Sub RootFrame_NavigationFailed(ByVal sender As Object, ByVal e As NavigationFailedEventArgs)
        If Diagnostics.Debugger.IsAttached Then
            ' A navigation has failed; break into the debugger
            Diagnostics.Debugger.Break()
        End If
    End Sub

    Public Sub Application_UnhandledException(ByVal sender As Object, ByVal e As ApplicationUnhandledExceptionEventArgs) Handles Me.UnhandledException

        ' Show graphics profiling information while debugging.
        If Diagnostics.Debugger.IsAttached Then
            Diagnostics.Debugger.Break()
        End If
    End Sub

#Region "Phone application initialization"
    ' Avoid double-initialization
    Private phoneApplicationInitialized As Boolean = False

    ' Do not add any additional code to this method
    Private Sub InitializePhoneApplication()
        If phoneApplicationInitialized Then
            Return
        End If

        ' Create the frame but don't set it as RootVisual yet; this allows the splash
        ' screen to remain active until the application is ready to render.
        RootFrame = New PhoneApplicationFrame()
        AddHandler RootFrame.Navigated, AddressOf CompleteInitializePhoneApplication

        ' Handle navigation failures
        AddHandler RootFrame.NavigationFailed, AddressOf RootFrame_NavigationFailed

        ' Ensure we don't initialize again
        phoneApplicationInitialized = True
    End Sub

    ' Do not add any additional code to this method
    Private Sub CompleteInitializePhoneApplication(ByVal sender As Object, ByVal e As NavigationEventArgs)
        ' Set the root visual to allow the application to render
        If RootVisual IsNot RootFrame Then
            RootVisual = RootFrame
        End If

        ' Remove this handler since it is no longer needed
        RemoveHandler RootFrame.Navigated, AddressOf CompleteInitializePhoneApplication
    End Sub
#End Region

End Class