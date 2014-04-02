namespace AST

type FormalParameter(parameterType : Type, name : string, pos : Position) =
    inherit Node(pos)
    member x.Type          = parameterType
    member x.Name          = name

    override x.ToString() = sprintf "%A %s" parameterType name
