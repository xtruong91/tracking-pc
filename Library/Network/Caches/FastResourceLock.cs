using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Network.Caches
{
   /// <summary>
   /// Provides a fast resource (reader-writer) lock.
   /// </summary>
   /// <remarks>
   /// There are three types of acquire methods in this lock:
   /// 
   /// Normal methods (AcquireExclusive, AcquireShared) are preferred 
   /// for general purpose use.
   /// Busy wait methods (SpinAcquireExclusive, SpinAcquireShared) are 
   /// preferred if very little time is spent while the lock is acquired. 
   /// However, these do not give exclusive acquires precedence over 
   /// shared acquires.
   /// Try methods (TryAcquireExclusive, TryAcquireShared) can be used to 
   /// quickly test if the lock is available.
   /// 
   /// Note that all three types of functions can be used concurrently 
   /// in the same class instance.
   ///      // Details
   //
   // Resource lock value width: 32 bits.
   // Lock owned (either exclusive or shared): L (1 bit).
   // Exclusive waking: W (1 bit).
   // Shared owners count: SC (10 bits).
   // Shared waiters count: SW (10 bits).
   // Exclusive waiters count: EW (10 bits).
   //
   // Acquire exclusive:
   // {L=0,W=0,SC=0,SW,EW=0} -> {L=1,W=0,SC=0,SW,EW=0}
   // {L=0,W=1,SC=0,SW,EW} or {L=1,W,SC,SW,EW} ->
   //     {L,W,SC,SW,EW+1},
   //     wait on event,
   //     {L=0,W=1,SC=0,SW,EW} -> {L=1,W=0,SC=0,SW,EW}
   //
   // Acquire shared:
   // {L=0,W=0,SC=0,SW,EW=0} -> {L=1,W=0,SC=1,SW,EW=0}
   // {L=1,W=0,SC>0,SW,EW=0} -> {L=1,W=0,SC+1,SW,EW=0}
   // {L=1,W=0,SC=0,SW,EW=0} or {L,W=1,SC,SW,EW} or
   //     {L,W,SC,SW,EW>0} -> {L,W,SC,SW+1,EW},
   //     wait on event,
   //     retry.
   //
   // Release exclusive:
   // {L=1,W=0,SC=0,SW,EW>0} ->
   //     {L=0,W=1,SC=0,SW,EW-1},
   //     release one exclusive waiter.
   // {L=1,W=0,SC=0,SW,EW=0} ->
   //     {L=0,W=0,SC=0,SW=0,EW=0},
   //     release all shared waiters.
   //
   // Note that we never do a direct acquire when W=1 
   // (i.e. L=0 if W=1), so here we don't have to check 
   // the value of W.
   //
   // Release shared:
   // {L=1,W=0,SC>1,SW,EW} -> {L=1,W=0,SC-1,SW,EW}
   // {L=1,W=0,SC=1,SW,EW=0} -> {L=0,W=0,SC=0,SW,EW=0}
   // {L=1,W=0,SC=1,SW,EW>0} ->
   //     {L=0,W=1,SC=0,SW,EW-1},
   //     release one exclusive waiter.
   //
   // Again, we don't need to check the value of W.
   //
   // Convert exclusive to shared:
   // {L=1,W=0,SC=0,SW,EW} ->
   //     {L=1,W=0,SC=1,SW=0,EW},
   //     release all shared waiters.
   //
   // Convert shared to exclusive:
   // {L=1,W=0,SC=1,SW,EW} ->
   //     {L=1,W=0,SC=0,SW,EW}
   //
   /// </remarks>
   public class FastResourceLock : IDisposable
   {
      #region Constants

      // Lock owned: 1 bit.
      private const int LockOwned = 0x1;

      // Exclusive waking: 1 bit.
      private const int LockExclusiveWaking = 0x2;

      // Shared owners count: 10 bits.
      private const int LockSharedOwnersShift = 2;
      private const int LockSharedOwnersMask = 0x3ff;
      private const int LockSharedOwnersIncrement = 0x4;

      // Shared waiters count: 10 bits.
      private const int LockSharedWaitersShift = 12;
      private const int LockSharedWaitersMask = 0x3ff;
      private const int LockSharedWaitersIncrement = 0x1000;

      // Exclusive waiters count: 10 bits.
      private const int LockExclusiveWaitersShift = 22;
      private const int LockExclusiveWaitersMask = 0x3ff;
      private const int LockExclusiveWaitersIncrement = 0x400000;

      private const int ExclusiveMask = LockExclusiveWaking | (LockExclusiveWaitersMask << LockExclusiveWaitersShift);
      #endregion

      // The number of times to spin before going to sleep.
      private static readonly int SpinCount = NativeMethods.SpinCount;
      private int _value;
      private IntPtr _sharedWakeEvent;
      private IntPtr _exclusiveWakeEvent;
#if ENABLE_STATISTICS
        private int _acqExclCount = 0;
        private int _acqShrdCount = 0;
        private int _acqExclContCount = 0;
        private int _acqShrdContCount = 0;
        private int _acqExclSlpCount = 0;
        private int _acqShrdSlpCount = 0;
        private int _peakExclWtrsCount = 0;
        private int _peakShrdWtrsCount = 0;
#endif
      /// <summary>
      /// Creates a FastResourceLock.
      /// </summary>
      public FastResourceLock()
      {
         _value = 0;

#if !DEFER_EVENT_CREATION
         _sharedWakeEvent = NativeMethods.CreateSemaphore(IntPtr.Zero, 0, int.MaxValue, null);
         _exclusiveWakeEvent = NativeMethods.CreateSemaphore(IntPtr.Zero, 0, int.MaxValue, null);
#endif
      }
      ~FastResourceLock()
      {
         this.Dispose(false);
      }

      private void Dispose(bool disposing)
      {
         if (_sharedWakeEvent != IntPtr.Zero)
         {
            NativeMethods.CloseHandle(_sharedWakeEvent);
            _sharedWakeEvent = IntPtr.Zero;
         }

         if (_exclusiveWakeEvent != IntPtr.Zero)
         {
            NativeMethods.CloseHandle(_exclusiveWakeEvent);
            _exclusiveWakeEvent = IntPtr.Zero;
         }
      }

      /// <summary>
      /// Disposes resources associated with the FastResourceLock.
      /// </summary>
      public void Dispose()
      {
         this.Dispose(true);
         GC.SuppressFinalize(this);
      }

      /// <summary>
      /// Gets the number of exclusive waiters.
      /// </summary>
      public int ExclusiveWaiters
      {
         get
         {
            return (_value >> LockExclusiveWaitersShift) & LockExclusiveWaitersMask;
         }
      }

      /// <summary>
      /// Gets whether the lock is owned in either 
      /// exclusive or shared mode.
      /// </summary>
      public bool Owned
      {
         get
         {
            return (_value & LockOwned) != 0;
         }
      }
      /// <summary>
      /// Gets the number of shared owners.
      /// </summary>
      public int SharedOwners
      {
         get
         {
            return (_value >> LockSharedOwnersShift) & LockSharedOwnersMask;
         }
      }
      /// <summary>
      /// Gets the number of shared waiters.
      /// </summary>
      public int SharedWaiters
      {
         get
         {
            return (_value >> LockSharedWaitersShift) & LockSharedWaitersMask;
         }
      }
      /// <summary>
      /// Acquires the lock in exclusive mode, blocking 
      /// if necessary.
      /// </summary>
      /// <remarks>
      /// Exclusive acquires are given precedence over shared 
      /// acquires.
      /// </remarks>
      public void AcquireExclusive()
      {
         int value;
         int i = 0;

#if ENABLE_STATISTICS
            Interlocked.Increment(ref _acqExclCount);

#endif
         while (true)
         {
            value = _value;

            // Case 1: lock not owned AND an exclusive waiter is not waking up.
            // Here we don't have to check if there are exclusive waiters, because 
            // if there are the lock would be owned, and we are checking that anyway.
            if ((value & (LockOwned | LockExclusiveWaking)) == 0)
            {
#if RIGOROUS_CHECKS
                    System.Diagnostics.Trace.Assert(((value >> LockSharedOwnersShift) & LockSharedOwnersMask) == 0);
                    System.Diagnostics.Trace.Assert(((value >> LockExclusiveWaitersShift) & LockExclusiveWaitersMask) == 0);

#endif
               if (Interlocked.CompareExchange(
                   ref _value,
                   value + LockOwned,
                   value
                   ) == value)
                  break;
            }
            // Case 2: lock owned OR lock not owned and an exclusive waiter is waking up. 
            // The second case means an exclusive waiter has just been woken up and is 
            // going to acquire the lock. We have to go to sleep to make sure we don't 
            // steal the lock.
            else if (i >= SpinCount)
            {
#if DEFER_EVENT_CREATION
               // This call must go *before* the next operation. Otherwise, 
               // we will have a race condition between potential releasers 
               // and us.
               this.EnsureEventCreated(ref _exclusiveWakeEvent);

#endif
               if (Interlocked.CompareExchange(
                   ref _value,
                   value + LockExclusiveWaitersIncrement,
                   value
                   ) == value)
               {
#if ENABLE_STATISTICS
                        Interlocked.Increment(ref _acqExclSlpCount);

                        int exclWtrsCount = (value >> LockExclusiveWaitersShift) & LockExclusiveWaitersMask;

                        Interlocked2.Set(
                            ref _peakExclWtrsCount,
                            (p) => p < exclWtrsCount,
                            (p) => exclWtrsCount
                            );

#endif
                  // Go to sleep.
                  if (NativeMethods.WaitForSingleObject(
                      _exclusiveWakeEvent,
                      Timeout.Infinite
                      ) != NativeMethods.WaitObject0)
                     UtilsBreak("Utils.MsgFailedToWaitIndefinitely");

                  // Acquire the lock. 
                  // At this point *no one* should be able to steal the lock from us.
                  do
                  {
                     value = _value;
#if RIGOROUS_CHECKS

                            System.Diagnostics.Trace.Assert((value & LockOwned) == 0);
                            System.Diagnostics.Trace.Assert((value & LockExclusiveWaking) != 0);
#endif
                  } while (Interlocked.CompareExchange(
                      ref _value,
                      value + LockOwned - LockExclusiveWaking,
                      value
                      ) != value);

                  break;
               }
            }

#if ENABLE_STATISTICS
                Interlocked.Increment(ref _acqExclContCount);
#endif
            i++;
         }
      }

      /// <summary>
      /// Acquires the lock in shared mode, blocking 
      /// if necessary.
      /// </summary>
      /// <remarks>
      /// Exclusive acquires are given precedence over shared 
      /// acquires.
      /// </remarks>
      public void AcquireShared()
      {
         int value;
         int i = 0;

#if ENABLE_STATISTICS
            Interlocked.Increment(ref _acqShrdCount);

#endif
         while (true)
         {
            value = _value;

            // Case 1: lock not owned AND no exclusive waiter is waking up AND 
            // there are no shared owners AND there are no exclusive waiters
            if ((value & (
                LockOwned |
                (LockSharedOwnersMask << LockSharedOwnersShift) |
                ExclusiveMask
                )) == 0)
            {
               if (Interlocked.CompareExchange(
                   ref _value,
                   value + LockOwned + LockSharedOwnersIncrement,
                   value
                   ) == value)
                  break;
            }
            // Case 2: lock is owned AND no exclusive waiter is waking up AND 
            // there are shared owners AND there are no exclusive waiters
            else if (
                (value & LockOwned) != 0 &&
                ((value >> LockSharedOwnersShift) & LockSharedOwnersMask) != 0 &&
                (value & ExclusiveMask) == 0
                )
            {
               if (Interlocked.CompareExchange(
                   ref _value,
                   value + LockSharedOwnersIncrement,
                   value
                   ) == value)
                  break;
            }
            // Other cases.
            else if (i >= SpinCount)
            {
#if DEFER_EVENT_CREATION
               this.EnsureEventCreated(ref _sharedWakeEvent);

#endif
               if (Interlocked.CompareExchange(
                   ref _value,
                   value + LockSharedWaitersIncrement,
                   value
                   ) == value)
               {
#if ENABLE_STATISTICS
                        Interlocked.Increment(ref _acqShrdSlpCount);

                        int shrdWtrsCount = (value >> LockSharedWaitersShift) & LockSharedWaitersMask;

                        Interlocked2.Set(
                            ref _peakShrdWtrsCount,
                            (p) => p < shrdWtrsCount,
                            (p) => shrdWtrsCount
                            );

#endif
                  // Go to sleep.
                  if (NativeMethods.WaitForSingleObject(
                      _sharedWakeEvent,
                      Timeout.Infinite
                      ) != NativeMethods.WaitObject0)
                     UtilsBreak("Utils.MsgFailedToWaitIndefinitely");

                  // Go back and try again.
                  continue;
               }
            }

#if ENABLE_STATISTICS
                Interlocked.Increment(ref _acqShrdContCount);
#endif
            i++;
         }
      }

      public static void UtilsBreak(string logMessage)
      {
         System.Diagnostics.Debugger.Log(0, "Error", logMessage);
         System.Diagnostics.Debugger.Break();
      }

      /// <summary>
      /// Converts the ownership mode from exclusive to shared.
      /// </summary>
      /// <remarks>
      /// Exclusive acquires are not given a chance to acquire 
      /// the lock before this function does - as a result, 
      /// this function will never block.
      /// </remarks>
      public void ConvertExclusiveToShared()
      {
         int value;
         int sharedWaiters;

         while (true)
         {
            value = _value;
#if RIGOROUS_CHECKS

                    System.Diagnostics.Trace.Assert((value & LockOwned) != 0);
                    System.Diagnostics.Trace.Assert((value & LockExclusiveWaking) == 0);
                    System.Diagnostics.Trace.Assert(((value >> LockSharedOwnersShift) & LockSharedOwnersMask) == 0);
#endif

            sharedWaiters = (value >> LockSharedWaitersShift) & LockSharedWaitersMask;

            if (Interlocked.CompareExchange(
                ref _value,
                (value + LockSharedOwnersIncrement) & ~(LockSharedWaitersMask << LockSharedWaitersShift),
                value
                ) == value)
            {
               if (sharedWaiters != 0)
                  NativeMethods.ReleaseSemaphore(_sharedWakeEvent, sharedWaiters, IntPtr.Zero);

               break;
            }
         }
      }

#if DEFER_EVENT_CREATION
      /// <summary>
      /// Checks if the specified event has been created, and 
      /// if not, creates it.
      /// </summary>
      /// <param name="handle">A reference to the event handle.</param>
      private void EnsureEventCreated(ref IntPtr handle)
      {
         IntPtr eventHandle;

         if(Thread.VolatileRead(ref handle) != IntPtr.Zero)
            return;

         eventHandle = NativeMethods.CreateSemaphore(IntPtr.Zero, 0, int.MaxValue, null);

         if(Interlocked.CompareExchange(ref handle, eventHandle, IntPtr.Zero) != IntPtr.Zero)
            NativeMethods.CloseHandle(eventHandle);
      }
#endif

      /// <summary>
      /// Gets statistics information for the lock.
      /// </summary>
      /// <returns>A structure containing statistics.</returns>
      public Statistics GetStatistics()
      {
#if ENABLE_STATISTICS
            return new Statistics()
            {
                AcqExcl = _acqExclCount,
                AcqShrd = _acqShrdCount,
                AcqExclCont = _acqExclContCount,
                AcqShrdCont = _acqShrdContCount,
                AcqExclSlp = _acqExclSlpCount,
                AcqShrdSlp = _acqShrdSlpCount,
                PeakExclWtrsCount = _peakExclWtrsCount,
                PeakShrdWtrsCount = _peakShrdWtrsCount
            };
#else
         return new Statistics();
#endif
      }

      /// <summary>
      /// Releases the lock in exclusive mode.
      /// </summary>
      public void ReleaseExclusive()
      {
         int value;

         while (true)
         {
            value = _value;
#if RIGOROUS_CHECKS

                System.Diagnostics.Trace.Assert((value & LockOwned) != 0);
                System.Diagnostics.Trace.Assert((value & LockExclusiveWaking) == 0);
                System.Diagnostics.Trace.Assert(((value >> LockSharedOwnersShift) & LockSharedOwnersMask) == 0);
#endif

            // Case 1: if we have exclusive waiters, release one.
            if (((value >> LockExclusiveWaitersShift) & LockExclusiveWaitersMask) != 0)
            {
               if (Interlocked.CompareExchange(
                   ref _value,
                   value - LockOwned + LockExclusiveWaking - LockExclusiveWaitersIncrement,
                   value
                   ) == value)
               {
                  NativeMethods.ReleaseSemaphore(_exclusiveWakeEvent, 1, IntPtr.Zero);

                  break;
               }
            }
            // Case 2: if we have shared waiters, release all of them.
            else
            {
               int sharedWaiters;

               sharedWaiters = (value >> LockSharedWaitersShift) & LockSharedWaitersMask;

               if (Interlocked.CompareExchange(
                   ref _value,
                   value & ~(LockOwned | (LockSharedWaitersMask << LockSharedWaitersShift)),
                   value
                   ) == value)
               {
                  if (sharedWaiters != 0)
                     NativeMethods.ReleaseSemaphore(_sharedWakeEvent, sharedWaiters, IntPtr.Zero);

                  break;
               }
            }
         }
      }
      /// <summary>
      /// Releases the lock in shared mode.
      /// </summary>
      public void ReleaseShared()
      {
         int value;
         int sharedOwners;

         while (true)
         {
            value = _value;
#if RIGOROUS_CHECKS

                System.Diagnostics.Trace.Assert((value & LockOwned) != 0);
                System.Diagnostics.Trace.Assert((value & LockExclusiveWaking) == 0);
                System.Diagnostics.Trace.Assert(((value >> LockSharedOwnersShift) & LockSharedOwnersMask) != 0);
#endif

            sharedOwners = (value >> LockSharedOwnersShift) & LockSharedOwnersMask;

            // Case 1: there are multiple shared owners.
            if (sharedOwners > 1)
            {
               if (Interlocked.CompareExchange(
                   ref _value,
                   value - LockSharedOwnersIncrement,
                   value
                   ) == value)
                  break;
            }
            // Case 2: we are the last shared owner AND there are exclusive waiters.
            else if (((value >> LockExclusiveWaitersShift) & LockExclusiveWaitersMask) != 0)
            {
               if (Interlocked.CompareExchange(
                   ref _value,
                   value - LockOwned + LockExclusiveWaking - LockSharedOwnersIncrement - LockExclusiveWaitersIncrement,
                   value
                   ) == value)
               {
                  NativeMethods.ReleaseSemaphore(_exclusiveWakeEvent, 1, IntPtr.Zero);

                  break;
               }
            }
            // Case 3: we are the last shared owner AND there are no exclusive waiters.
            else
            {
               if (Interlocked.CompareExchange(
                   ref _value,
                   value - LockOwned - LockSharedOwnersIncrement,
                   value
                   ) == value)
                  break;
            }
         }
      }

      /// <summary>
      /// Acquires the lock in exclusive mode, busy waiting 
      /// if necessary.
      /// </summary>
      /// <remarks>
      /// Exclusive acquires are *not* given precedence over shared 
      /// acquires for busy wait methods.
      /// </remarks>
      public void SpinAcquireExclusive()
      {
         int value;

         while (true)
         {
            value = _value;

            if ((value & (LockOwned | LockExclusiveWaking)) == 0)
            {
               if (Interlocked.CompareExchange(
                   ref _value,
                   value + LockOwned,
                   value
                   ) == value)
                  break;
            }

            if (NativeMethods.SpinEnabled)
               Thread.SpinWait(8);
            else
               Thread.Sleep(0);
         }
      }

      /// <summary>
      /// Acquires the lock in shared mode, busy waiting 
      /// if necessary.
      /// </summary>
      /// <remarks>
      /// Exclusive acquires are *not* given precedence over shared 
      /// acquires for busy wait methods.
      /// </remarks>
      public void SpinAcquireShared()
      {
         int value;

         while (true)
         {
            value = _value;

            if ((value & ExclusiveMask) == 0)
            {
               if ((value & LockOwned) == 0)
               {
                  if (Interlocked.CompareExchange(
                      ref _value,
                      value + LockOwned + LockSharedOwnersIncrement,
                      value
                      ) == value)
                     break;
               }
               else if (((value >> LockSharedOwnersShift) & LockSharedOwnersMask) != 0)
               {
                  if (Interlocked.CompareExchange(
                      ref _value,
                      value + LockSharedOwnersIncrement,
                      value
                      ) == value)
                     break;
               }
            }

            if (NativeMethods.SpinEnabled)
               Thread.SpinWait(8);
            else
               Thread.Sleep(0);
         }
      }

      /// <summary>
      /// Converts the ownership mode from shared to exclusive, 
      /// busy waiting if necessary.
      /// </summary>
      public void SpinConvertSharedToExclusive()
      {
         int value;

         while (true)
         {
            value = _value;

            // Can't convert if there are other shared owners.
            if (((value >> LockSharedOwnersShift) & LockSharedOwnersMask) == 1)
            {
               if (Interlocked.CompareExchange(
                   ref _value,
                   value - LockSharedOwnersIncrement,
                   value
                   ) == value)
                  break;
            }

            if (NativeMethods.SpinEnabled)
               Thread.SpinWait(8);
            else
               Thread.Sleep(0);
         }
      }

      /// <summary>
      /// Attempts to acquire the lock in exclusive mode.
      /// </summary>
      /// <returns>Whether the lock was acquired.</returns>
      public bool TryAcquireExclusive()
      {
         int value;

         value = _value;

         if ((value & (LockOwned | LockExclusiveWaking)) != 0)
            return false;

         return Interlocked.CompareExchange(
             ref _value,
             value + LockOwned,
             value
             ) == value;
      }

      /// <summary>
      /// Attempts to acquire the lock in shared mode.
      /// </summary>
      /// <returns>Whether the lock was acquired.</returns>
      public bool TryAcquireShared()
      {
         int value;

         value = _value;

         if ((value & ExclusiveMask) != 0)
            return false;

         if ((value & LockOwned) == 0)
         {
            return Interlocked.CompareExchange(
                ref _value,
                value + LockOwned + LockSharedOwnersIncrement,
                value
                ) == value;
         }
         else if (((value >> LockSharedOwnersShift) & LockSharedOwnersMask) != 0)
         {
            return Interlocked.CompareExchange(
                ref _value,
                value + LockSharedOwnersIncrement,
                value
                ) == value;
         }
         else
         {
            return false;
         }
      }

      /// <summary>
      /// Attempts to convert the ownership mode from shared 
      /// to exclusive.
      /// </summary>
      /// <returns>Whether the lock was converted.</returns>
      public bool TryConvertSharedToExclusive()
      {
         int value;

         while (true)
         {
            value = _value;
#if RIGOROUS_CHECKS

                    System.Diagnostics.Trace.Assert((value & LockOwned) != 0);
                    System.Diagnostics.Trace.Assert((value & LockExclusiveWaking) == 0);
                    System.Diagnostics.Trace.Assert(((value >> LockSharedOwnersShift) & LockSharedOwnersMask) != 0);
#endif

            // Can't convert if there are other shared owners.
            if (((value >> LockSharedOwnersShift) & LockSharedOwnersMask) != 1)
               return false;

            if (Interlocked.CompareExchange(
                ref _value,
                value - LockSharedOwnersIncrement,
                value
                ) == value)
               return true;
         }
      }
   }
}
