namespace fsharp_common
open System
open System.Collections.Generic
open System.Threading





type MessageQueueCompletionPort() =

    static let dic_lock_ = Object()
    static let dic_ = Dictionary< string, MQCPInfo >()
    
   
    
    static member SendRequestAwaitResponse (request_packet : MQCPPacket) (time_out_msec : int32) mq_send_fun =
        let new_mqcp_info = {
            auto_reset_event_ = new AutoResetEvent(false)
            response_packet_ = None
        }
        
        LockGuard.lock dic_lock_ (fun() -> dic_.Add(request_packet.req_guid_, new_mqcp_info))
        request_packet |> mq_send_fun
        
        let success = time_out_msec |> new_mqcp_info.auto_reset_event_.WaitOne
        if success then
            LockGuard.lock dic_lock_ (fun() ->
                match request_packet.req_guid_ |> dic_.TryGetValue with
                | true, mqcp_info ->
                    request_packet.req_guid_ |> dic_.Remove |> ignore
                    mqcp_info.response_packet_.Value
                | _ ->
                    failwith "impossible_case"     
                )
        else
            failwith "request_timeout"
    
    
    
    
    static member RegisterResponse(response_packet : MQCPPacket) =
        LockGuard.lock dic_lock_ (fun() ->
            match dic_.TryGetValue response_packet.req_guid_ with
            | true, mqcp_info ->
                let new_mqcp_info = {
                    auto_reset_event_ = mqcp_info.auto_reset_event_
                    response_packet_ = response_packet |> Some
                }
                
                dic_.[response_packet.req_guid_] = new_mqcp_info |> ignore
                mqcp_info.auto_reset_event_.Set() |> ignore
            | _ ->
                printfn "The request was not registered. > Request guid : [%s]" response_packet.req_guid_
            )
        