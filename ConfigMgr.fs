namespace fsharp_common

open System
open System.Collections.Generic
open System.IO
open System.Text
open Newtonsoft.Json




type ConfigMgr() =
    
    static let dic_ = Dictionary< string, string >()


    
    
    static let checkIfFileExists (path : string) = 
        if path |> File.Exists = false then
            Logger.Error(String.Format("Cannot found the config file. > {0}", path))
        path
    
    
    
    
    static let loadFileConfig (path: string) =
        try
            (path, Encoding.UTF8) |> File.ReadAllText
        with
        | ex ->
            Logger.Error (String.Format("Loading failed. : {0}",ex.Message))
            failwith "loading_failed"
                
                
                
                
    static let insertElemToDic (replace : bool) (elem : KeyValuePair<string,string>) =
        match elem.Key |> dic_.TryGetValue  with
        | true, _ ->
                if replace then 
                    dic_.[elem.Key] <- elem.Value
        | _ ->
            (elem.Key, elem.Value) |> dic_.Add
        
    
    
    
    static let insertCfgObjToDic  (replace : bool) (cfg_obj : Dictionary<string,string>) =
        for elem in cfg_obj do
            elem |> (replace |> insertElemToDic)  
        
            


    static member LoadFromFile (replace : bool) (path : string)  =
        try
            path
            |> checkIfFileExists
            |> loadFileConfig
            |> JsonConvert.DeserializeObject<Dictionary<string,string>>
            |> (replace |> insertCfgObjToDic)
        with
        | ex ->
            Logger.Exception (ex.Message)
            Environment.Exit(-1)
            
    
            
    
    static member GetStrOrException(key : string) =
        match key |> dic_.TryGetValue with
        | true, value -> value
        | _ -> ("Does not exist config property > key({0})",key)
               |> String.Format
               |> failwith
        
    
        
        
    static member GetInt32OrException(key : string) =
        key |> ConfigMgr.GetStrOrException |> int32

    

    
    static member GetFloatOrException(key : string) =
        key |> ConfigMgr.GetStrOrException |> float32
