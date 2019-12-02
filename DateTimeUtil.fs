namespace fsharp_common
open System






module DateTimeUtil =
    
    let CurUnixTimeMSec() =
        DateTimeOffset.Now.ToUnixTimeMilliseconds()
        
        
        
        
        
    let UnixTimeMSec2DateTime(unixTimeMSec : int64) =
        DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMSec).DateTime
                
    
    
    
    let DateTime2UnixTimeMSec(dateTime : DateTime) =
        DateTimeOffset(dateTime).ToUnixTimeMilliseconds()