using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Database
{
   public interface PureDataCache
   {
      bool PutDataToCache(int id, byte[] rawData);
      bool GetDataFromCache(int id, ref byte[] rawData);
      bool DeleteOlderThan(int? type);
   }
}
