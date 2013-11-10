Imports System.ServiceModel

' NOTE: You can use the "Rename" command on the context menu to change the interface name "IRandomizer" in both code and config file together.
<ServiceContract()> _
Public Interface IRandomizer

    <OperationContract()> _
    Function RandomDiceResult(ByVal sideCount As Integer) As Integer


End Interface
