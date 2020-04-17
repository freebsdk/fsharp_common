namespace fsharp_common

open System
open Apache.NMS





type ActiveMQHandle () =
    
    let mutable conn_ : IConnection = null
    let mutable sess_ : ISession = null
    let mutable dest_ : IDestination = null
    
    let mutable producer_ : IMessageProducer = null
    let mutable consumer_ : IMessageConsumer = null
    
    

        
    let getDestination (conn_info : ActiveMQConnectInfo) =
        match conn_info.ChannelType with
        | Queue -> conn_info.Channel |> sess_.GetQueue :> IDestination
        | Topic -> conn_info.Channel |> sess_.GetTopic :> IDestination

    
    
    
    let receiveByMode (blocking : bool) =
        match blocking with
        | true -> consumer_.Receive()
        | false -> consumer_.ReceiveNoWait()
    
    
    
    
    member _.CreateProducer (delivery_mode : MsgDeliveryMode) (time_out_sec : float) =
        producer_ <- dest_ |> sess_.CreateProducer
        producer_.DeliveryMode <- delivery_mode
        producer_.RequestTimeout <- time_out_sec |> TimeSpan.FromSeconds

    
    
    
    member _.CreateConsumer() =
        consumer_ <- dest_ |> sess_.CreateConsumer
    
    
    

        
    member _.Open(conn_info : ActiveMQConnectInfo) =
        let factory = conn_info.ConnectURI |> NMSConnectionFactory
        conn_ <- factory.CreateConnection() 
        sess_ <- conn_.CreateSession()
        dest_ <- conn_info |> getDestination
        conn_.Start()
    
    
    
    
    member _.Close() =
        if conn_ <> null then
            
            // optional
            if producer_ <> null then
                producer_.Dispose()
                producer_ <- null
                
            // optional
            if consumer_ <> null then
                consumer_.Dispose()
                consumer_ <- null
            
            dest_.Dispose()
            sess_.Dispose()
            conn_.Dispose()
            
            dest_ <- null
            sess_ <- null
            conn_ <- null
            
    
    
    
    member _.Send (message : string) =
        if producer_ = null then
            failwith "The producer has not been initialized."
            
        message
        |> sess_.CreateTextMessage
        |> producer_.Send
    
    
           
           
    
    member _.Receive (blocking : bool) =
        if consumer_ = null then
            failwith "The consumer has not been initialized."
            
        let message = blocking |> receiveByMode :?> ITextMessage
        if message <> null then
            message.Text |> Some
        else
            None
