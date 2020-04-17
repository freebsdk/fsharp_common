namespace fsharp_common
open System






module DateTimeUtil =
    
    let CurUnixTimeMSec() =
        DateTimeOffset.Now.ToUnixTimeMilliseconds()
        
        
        
        
        
    let UnixTimeMSec2DateTime(unix_time_msec : int64) =
        DateTimeOffset.FromUnixTimeMilliseconds(unix_time_msec).LocalDateTime
                
    
    
    let DateTime2UnixTimeMSec(date_time : DateTime) =
        DateTimeOffset(date_time).ToUnixTimeMilliseconds()
        
       
        
        
    let DateTime2Str(date_time : DateTime) =         
        date_time.ToString("yyyy-MM-dd HH:mm:ss")
        
        
                
        
    let CurDateTimeStr() =
        CurUnixTimeMSec()
        |> UnixTimeMSec2DateTime
        |> DateTime2Str 
        
        