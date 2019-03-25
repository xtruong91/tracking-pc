using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Database
{
   public class SQLitePureDataCache : PureDataCache
   {
      public bool DeleteOlderThan(int? type)
      {
         throw new NotImplementedException();
      }

      public bool GetDataFromCache(int id, ref byte[] rawData)
      {
         throw new NotImplementedException();
      }

      public bool PutDataToCache(int id, byte[] rawData)
      {
         throw new NotImplementedException();
      }
   }
}
