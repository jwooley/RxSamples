using System;
using System.ServiceModel;

namespace RxDemo
{
    [ServiceContract]
    public interface IWCFService
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        string GetDataSlowly(int value);
    }
}
