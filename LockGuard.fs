namespace fsharp_common
open System.Threading



module LockGuard = 

  let lock (lockobj:obj) f =
      Monitor.Enter lockobj
      try
          f()
      finally
            Monitor.Exit lockobj

