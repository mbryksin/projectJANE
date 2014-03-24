namespace AST

type Type(name : string, dimension : int) =
    member x.Name      = name
    member x.Dimension = dimension
