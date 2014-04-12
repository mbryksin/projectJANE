namespace AST

type Variable(name : string, varType : Type, value : Val) =
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
        let value =
            match x.Val with
            | Int content   -> content.ToString()
            | Bool content  -> content.ToString()
            | Str content   -> content.ToString()
            | Char content  -> content.ToString()
            | Float content -> content.ToString()
            | _        -> "error"
        start + value



