namespace fsharp_common
open System.Threading



// Message queue completion port records



type MQCPAddress = {
    ip_ : string;
    port_ : uint16;
}




type MQCPPacket = {
    req_guid_ : string
    from_adrs_ : MQCPAddress
    to_adrs_ : MQCPAddress
    error_ : string
    payload_ : string
    time_ : int64
}




type MQCPInfo = {
    auto_reset_event_ : AutoResetEvent
    response_packet_ : Option<MQCPPacket>
}

