namespace fsharp_common

open System.Collections.Generic





type ActiveMQHandlePoolSet() =
    
    static let dic_ = Dictionary< string, ActiveMQHandlePool>()
    

    
        
    static member RegisterPool (channel : string) (pool : ActiveMQHandlePool) =
        (channel, pool) |> dic_.Add 
            
            
            
            
    static member GetPool (pool_key : string) =
        match pool_key |> dic_.TryGetValue with
        | true, value -> value |> Some
        | _ -> None