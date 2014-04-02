namespace AST

type FormalParameter(parameterType : Type, name : string, pos : Position) =
    inherit Node(pos)
    member x.ParameterType = parameterType
    member x.Name          = name
