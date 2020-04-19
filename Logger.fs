namespace fsharp_common





module Logger =

    
    let LogStart(msg : string)=
        printf "[Log] %s" msg
    
    
    let LogEnd(msg : string)=
        printf "%s\r\n" msg
    
    
    let Log(msg : string)=
        printfn "[Log] %s" msg


    
    let Error(msg : string)=
        printfn "[Error] %s" msg
    
    
        
    let Exception (msg : string) =
        printfn "[Exception] %s" msg
