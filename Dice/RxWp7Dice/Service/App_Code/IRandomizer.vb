Imports System.ServiceModel

' NOTE: You can use the "Rename" command on the context menu to change the interface name "IRandomizer" in both code and config file together.
<ServiceContract()>
Public Interface IRandomizer

    <OperationContract()>
    Function Roll(sideCount As Integer) As Integer

    <OperationContract()>
    Function RandomDiceResult(ByVal sideCount As Integer, ByVal index As Integer) As DiceContract


End Interface
