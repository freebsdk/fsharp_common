namespace fsharp_common
open System




type AMQChannelType =
    | Queue
    | Topic




type AMQRole =
    | Producer
    | Consumer
    



type AMQDeliveryMode =
    | Persistent
    | NonPersistent





type ActiveMQConnectInfo (host_adrs : string, port : int32, channel_type : AMQChannelType, channel : string) =
    let host_adrs_ = host_adrs
    let port_ = port
    let channel_type_ = channel_type
    let channel_ = channel

    
    
    
    member _.ConnectURI =
        ("activemq:tcp://{0}:{1}", host_adrs_, port_) |> String.Format |> Uri

    
    
    
    
    static member ToChannelType (channel_type : string) =
        match channel_type with
        | "queue" -> Queue
        | "topic" -> Topic
        | _ -> ("invalid channel_type > ({0})", channel_type)
               |> String.Format
               |> failwith  

    
    
    member _.HostAdrs = host_adrs_
    member _.Port = port_
    member _.ChannelType = channel_type
    member _.Channel = channel  
