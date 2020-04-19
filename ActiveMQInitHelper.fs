namespace fsharp_common
open System




module ActiveMQInitHelper =
    
    
let InitActiveMQPool(channel : string) =
    
    let host = ("ActiveMQ::{0}::Host", channel)
                    |> String.Format
                    |> ConfigMgr.GetStrOrException
                    
    let port = ("ActiveMQ::{0}::Port", channel)
               |> String.Format
               |> ConfigMgr.GetInt32OrException
               
    let init_connect_count = ("ActiveMQ::{0}::InitConnectCount", channel)
                             |> String.Format
                             |> ConfigMgr.GetInt32OrException
                             
    let channel_type = ("ActiveMQ::{0}::ChannelType", channel)
                       |> String.Format
                       |> ConfigMgr.GetStrOrException
                       |> ActiveMQConnectInfo.ToChannelType
    
    let connect_info = ActiveMQConnectInfo (host, port, channel_type, channel)
    
    let pool = ActiveMQHandlePool()
    (init_connect_count, connect_info) ||> pool.Init
    (channel, pool) ||> ActiveMQHandlePoolSet.RegisterPool
