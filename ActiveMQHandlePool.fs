namespace fsharp_common

open System
open System.Collections.Generic





type ActiveMQHandlePool() =
    
    let list_lock_ = Object();
    let list_ = List< ActiveMQHandle >()
    let allocated_ = Dictionary< Guid, ActiveMQHandle >()

    
    
    
    
    member _.Init(init_pool_size : int32) (connect_info : ActiveMQConnectInfo) =
        ("Initialize ActiveMQ Pool ({0} handles) ... ", init_pool_size) |> String.Format |> Logger.LogStart 
        LockGuard.lock list_lock_ (fun() ->
            for i in 0 .. init_pool_size do
                let handle = ActiveMQHandle()
                connect_info |> handle.Open
                handle |> list_.Add
        )
        "ok" |> Logger.LogEnd
    
    
        
    member _.RemainCount =
        LockGuard.lock list_lock_ (fun() ->
            list_.Count    
        )
        
        
        
        
    member _.Acquire () =
        LockGuard.lock list_lock_ (fun() ->
            if list_.Count <= 0 then
                "Not enough ActiveMQ handle." |> Logger.Error 
                None
            else    
                let handle = list_.[0]
                0 |> list_.RemoveAt
                
                (handle.Guid, handle) |> allocated_.Add
                handle |> Some 
        )
        
        
    
    
    member _.Release (handle : ActiveMQHandle) =
        LockGuard.lock list_lock_ (fun() ->
            if (handle.Guid |> allocated_.ContainsKey) then
                handle.Guid |> allocated_.Remove |> ignore
                handle |> list_.Add
            else
                failwith "Invalid handle"
        )