namespace fsharp_common
open System
open StackExchange.Redis





type RedisConnectInfo (host : string, port : uint16, db_idx) =
    
    let host_ = host
    let port_ = port
    let db_idx_ = db_idx


    member _.ConnectStr =
        ("{0}:{1}", host_, port_) |> String.Format

    member _.DBIdx = db_idx_






type RedisHandle() =
    
    let mutable redis_ = null
    let mutable conn_ = null
    let mutable sub_ = null
    
    
    
    
    member _.Open(conn_info : RedisConnectInfo) =
        redis_ <- conn_info.ConnectStr |> ConnectionMultiplexer.Connect
        conn_ <- redis_.GetDatabase(conn_info.DBIdx, null)
        sub_ <- redis_.GetSubscriber()
        
        
        
        
    member _.Close() =
        if redis_ <> null then
            redis_.Close()
            sub_ <- null
            conn_ <- null
            redis_ <- null


                
    member _.Context =
        conn_
        
        
        
    member _.Subscriber =
        sub_
