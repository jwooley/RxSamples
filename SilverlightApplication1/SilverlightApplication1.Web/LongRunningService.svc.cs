using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace SilverlightApplication1.Web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LongRunningService" in code, svc and config file together.
    public class LongRunningService : ILongRunningService
    {
        public int DoWork(int seconds)
        {
            Thread.Sleep(seconds );
            return seconds;
        }

        public string SortIt(string value)
        {
            var ret = string.Concat(
                (from c in value.ToCharArray()
                 orderby c
                 select c)
                      .Distinct()
                      .ToArray());
            
            return ret;                
        }


    }
}
