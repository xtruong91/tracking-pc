using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network.Caches
{
   public struct Statistics
   {
      /// <summary>
      /// The number of times the lock has been acquired in exclusive mode.
      /// </summary>
      public int AcqExcl;
      /// <summary>
      /// The number of times the lock has been acquired in shared mode.
      /// </summary>
      public int AcqShrd;
      /// <summary>
      /// The number of times either the fast path was retried due to the 
      /// spin count or the exclusive waiter went to sleep.
      /// </summary>
      /// <remarks>
      /// This number is usually much higher than AcqExcl, and indicates 
      /// a good spin count if AcqExclSlp is very small.
      /// </remarks>
      public int AcqExclCont;
      /// <summary>
      /// The number of times either the fast path was retried due to the 
      /// spin count or the shared waiter went to sleep.
      /// </summary>
      /// <remarks>
      /// This number is usually much higher than AcqShrd, and indicates 
      /// a good spin count if AcqShrdSlp is very small.
      /// </remarks>
      public int AcqShrdCont;
      /// <summary>
      /// The number of times exclusive waiters have gone to sleep.
      /// </summary>
      /// <remarks>
      /// If this number is high and not much time is spent in the 
      /// lock, consider increasing the spin count.
      /// </remarks>
      public int AcqExclSlp;
      /// <summary>
      /// The number of times shared waiters have gone to sleep.
      /// </summary>
      /// <remarks>
      /// If this number is high and not much time is spent in the 
      /// lock, consider increasing the spin count.
      /// </remarks>
      public int AcqShrdSlp;
      /// <summary>
      /// The highest number of exclusive waiters at any one time.
      /// </summary>
      public int PeakExclWtrsCount;
      /// <summary>
      /// The highest number of shared waiters at any one time.
      /// </summary>
      public int PeakShrdWtrsCount;
   }
}
