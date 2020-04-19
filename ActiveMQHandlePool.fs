namespace fsharp_common

open System
open System.Collections.Generic





type ActiveMQHandlePool() =
    
    static let list_lock_ = Object();
    static let list_ = List< ActiveMQHandle >()
    static let allocated_ = Dictionary< Guid, ActiveMQHandle >()

    
    
    
    
    static member Init(init_pool_size : int32) (conn_info : ActiveMQConnectInfo) =
        LockGuard.lock list_lock_ (fun() ->
            for i in 0 .. init_pool_size do
                let handle = ActiveMQHandle()
                conn_info |> handle.Open
                handle |> list_.Add
        )
    
    
        
    static member RemainCount =
        LockGuard.lock list_lock_ (fun() ->
            list_.Count    
        )
        
        
        
        
    static member Acquire () =
        LockGuard.lock list_lock_ (fun() ->
            if list_.Count <= 0 then
                None
            else    
                let handle = list_.[0]
                0 |> list_.RemoveAt
                
                (handle.Guid, handle) |> allocated_.Add
                handle |> Some 
        )
        
        
    
    
    static member Release (handle : ActiveMQHandle) =
        LockGuard.lock list_lock_ (fun() ->
            if (handle.Guid |> allocated_.ContainsKey) then
                handle.Guid |> allocated_.Remove |> ignore
                handle |> list_.Add
            else
                failwith "Invalid handle"
        )