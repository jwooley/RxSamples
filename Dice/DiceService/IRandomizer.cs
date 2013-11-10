using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DiceService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IRandomizer
    {
        [OperationContract]
        DiceContract RandomDiceResult(int sideCount, int index);

        [OperationContract]
        int RollDice(int sideCount);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class DiceContract
    {
        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public int DotCount {get; set;}

        [DataMember]
        public int SideCount { get; set; }
    }
}
