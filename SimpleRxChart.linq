<Query Kind="VBProgram">
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.DataVisualization.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <NuGetReference>System.Reactive</NuGetReference>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Windows.Forms.DataVisualization.Charting</Namespace>
</Query>

Sub Main
	dim chart As New Chart()
	dim ca As New ChartArea()
	
	chart.ChartAreas.Add(ca)

        Dim values = From val In GetSensor().
                    GroupBy(Function(s) GetOrCreateSeries(chart, s.SensorType)).
            Select(Function(s) New With {.series = s.Key, s})

        values.Subscribe(Sub(valObs)
                             valObs.s.Subscribe(Sub(val) valObs.series.Points.AddXY(val.TimeStamp, val.SensorValue))
                         End Sub)
	
'	dim series As New Series With { 
'		.ChartType = SeriesChartType.Line,
'		.XValueType = ChartValueType.Time}
'		
'	chart.Series.Add(series)
	
'	values.Subscribe(Sub(val)
'		series.Points.AddXY(val.TimeStamp, val.SensorValue)
'	end sub)
	
	chart.Dump("Chart")
End Sub

' Define other methods and classes here
	' Define other methods and classes here
	Public Class SensorInfo
	    Public Property TimeStamp As DateTime
	    Public Property SensorType As String
	    Public Property SensorValue As Double
		public overrides Function ToString() as String
		    Return String.Format("Time: {0}  , Type: {1}  Value: {2}", TimeStamp, SensorType, SensorValue)
		End Function
	End Class
	
	Public Function GetSensor() as IObservable(Of SensorInfo)
		Dim rnd = new Random(Date.Now.Millisecond)
		Return Observable.Generate(Of Int16, SensorInfo)( _
			initialState:=0,
			condition:=Function(x) True,
			iterate:= Function(inVal) inval,
			resultSelector:= Function(x)
					Dim randValue = rnd.NextDouble
					return new SensorInfo with {.SensorType = (math.Floor(randValue * 4) + 1).ToString,
						.SensorValue = randValue * 20,
						.TimeStamp = DateTime.Now}
				end Function,
			timeSelector := Function(x) TimeSpan.FromMilliseconds(rnd.NextDouble * 500)
		)
	End Function
	
	private ChartSeries As New Dictionary(of string, Series)
	public function GetOrCreateSeries(target as Chart, seriesKey as String)
		Dim currentSeries as Series
		If ChartSeries.ContainsKey(seriesKey)
			currentSeries = ChartSeries(seriesKey)
		else
			currentSeries = New Series With { 
				.ChartType = SeriesChartType.Line,
				.XValueType = ChartValueType.Time}
			
			target.Series.Add(currentSeries)
		End If
		return currentSeries
	End Function