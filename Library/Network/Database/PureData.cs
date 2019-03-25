using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Database
{
   public class PureData : IDisposable
   {
      public MemoryStream Data;
      public int ID;

      public void Dispose()
      {
      }
   }
}
