namespace JaneRuntime

open System
open AST

type ConsoleEvent() =
    static let e = new DelegateEvent<EventHandler<String>>()
    [<CLIEvent>]
    static member Event = e.Publish
    static member TriggerEvent(args) = e.Trigger([|box null; box args|])

type JaneConsole =

    static member writeLine(v : Val) =
        let rec writeValue vl =
            match vl with
            | Null          -> "null"
            | Int content   -> content.ToString()
            | Bool content  -> content.ToString()
            | Str content   -> content.ToString()
            | Char content  -> content.ToString()
            | Float content -> content.ToString()
            | Array content -> 
                            let arrayValues = Array.map (fun (v : Val) -> writeValue v) content
                            "{" + Array.fold (fun acc s ->  acc + s + " ") "" arrayValues + "}"
            | Object (fields, className) -> className.ToString() + fields.ToString() 
            | _        -> "error"
        let s = writeValue v
        ConsoleEvent.TriggerEvent(s)
        s
        