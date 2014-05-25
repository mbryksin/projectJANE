namespace AST 

type Val = 
    | Empty
    | Int       of int64
    | Float     of float
    | Str       of string
    | Char      of char
    | Bool      of bool
    | Return    of Val
    | Continue
    | Array     of Val array
    | ClassOrField  of string
    | MethodVal of string * Val list // id + elements
    | Object    of Variable list * string //Fields + ClassName
    | Err of string * Position
    | Null

and  Variable(name : string, varType : Type, value : Val) =
    let name = name
    let varType = varType 
    let mutable value = value 

    member x.Name = name
    member x.Val = value
    member x.Type = varType

    member x.Assign(assignValue : Val) = 
        value <- assignValue

    override x.ToString() = 
        let start = name + "="
        let valVar = x.Val
        let rec value vl =
            match vl with
            | Null          -> "null"
            | Int content   -> content.ToString()
            | Bool content  -> content.ToString()
            | Str content   -> content.ToString()
            | Char content  -> content.ToString()
            | Float content -> content.ToString()
            | Array content -> 
                            let arrayValues = Array.map (fun (v : Val) -> value v) content
                            "{" + Array.fold (fun acc s ->  acc + s + " ") "" arrayValues + "}"
            | Object (fields, className) -> className.ToString() + fields.ToString() 
            | _        -> "error"
        start + value valVar