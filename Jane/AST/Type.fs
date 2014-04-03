namespace AST

type Type(name : string, dimension : int, pos : Position) =
    inherit Node(pos)
    member x.Name      = name
    member x.Dimension = dimension
    
    override x.ToString() = sprintf "%s%s" name (String.replicate dimension "[]")
