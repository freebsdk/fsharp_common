namespace fsharp_common





module Logger =

    
    let Log(msg : string)=
        printfn "[Log] %s" msg


    
    let Error(msg : string)=
        printfn "[Error] %s" msg
    
    
        
    let Exception (msg : string) =
        printfn "[Exception] %s" msg
