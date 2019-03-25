using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Network.Sockets
{
   public class SckHttpResponse : WebResponse
   {
      #region Member Variables

      WebHeaderCollection _httpResponseHeaders;
      MemoryStream data;

      public override long ContentLength
      {
         get;
         set;
      }

      public override string ContentType
      {
         get;
         set;
      }

      #endregion

      #region Constructors

      public SckHttpResponse(MemoryStream data, string headers)
      {
         this.data = data;

         var headerValues = headers.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

         // ignore the first line in the header since it is the HTTP response code
         for (int i = 1; i < headerValues.Length; i++)
         {
            var headerEntry = headerValues[i].Split(new[] { ':' });
            Headers.Add(headerEntry[0], headerEntry[1]);

            switch (headerEntry[0])
            {
               case "Content-Type":
                  {
                     ContentType = headerEntry[1];
                  }
                  break;

               case "Content-Length":
                  {
                     long r = 0;
                     if (long.TryParse(headerEntry[1], out r))
                     {
                        ContentLength = r;
                     }
                  }
                  break;
            }
         }
      }

      #endregion

      #region WebResponse Members

      public override Stream GetResponseStream()
      {
         return data != null ? data : Stream.Null;
      }

      public override void Close()
      {
         if (data != null)
         {
            data.Close();
         }
         /* the base implementation throws an exception */
      }

      public override WebHeaderCollection Headers
      {
         get
         {
            if (_httpResponseHeaders == null)
            {
               _httpResponseHeaders = new WebHeaderCollection();
            }
            return _httpResponseHeaders;
         }
      }

      #endregion
   }
}
