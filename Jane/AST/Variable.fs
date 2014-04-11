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

    override x.ToString() = name + "=" + value.Int.Value.ToString()  //for test


