Imports System.ServiceModel

<ServiceContract()>
Public Interface ISimpleServicesvc

    <OperationContract()>
    Function DoSomethingCool(input As String) As String

End Interface
