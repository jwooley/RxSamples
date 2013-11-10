using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SilverlightApplication1.Web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ILongRunningService" in both code and config file together.
    [ServiceContract]
    public interface ILongRunningService
    {
        [OperationContract]
        int DoWork(int seconds);

        [OperationContract]
        string SortIt(string value);
    }
}
