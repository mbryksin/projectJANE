namespace AST

type Type(name : string, dimension : int, pos : Position) =
    inherit Node(pos)
    member x.Name      = name
    member x.Dimension = dimension
