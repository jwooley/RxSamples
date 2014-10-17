Imports Microsoft.VisualBasic
Imports System.Runtime.Serialization

<DataContract()> _
Public Class DiceContract
    <DataMember()> _
    Private _index As Integer
    Public Property Index() As Integer
        Get
            Return _index
        End Get
        Set(ByVal value As Integer)
            _index = value
        End Set
    End Property

    <DataMember()> _
    Private _DotCount As Integer
    Public Property DotCount() As Integer
        Get
            Return _DotCount
        End Get
        Set(ByVal value As Integer)
            _DotCount = value
        End Set
    End Property

    <DataMember()> _
    Private _SideCount As Integer
    Public Property SideCount() As Integer
        Get
            Return _SideCount
        End Get
        Set(ByVal value As Integer)
            _SideCount = value
        End Set
    End Property

End Class
