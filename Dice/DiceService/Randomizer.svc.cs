using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DiceService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class Randomizer : IRandomizer
    {
        public DiceContract RandomDiceResult(int sideCount, int index)
        {
            Random rand = new Random();
            return new DiceContract
            {
                DotCount = (int)(rand.NextDouble() * sideCount) + 1,
                Index = index,
                SideCount = sideCount
            };
        }


        public int RollDice(int sideCount)
        {
            Random rand = new Random();
            return (int)(rand.NextDouble() * sideCount) + 1;
        }
    }
}
