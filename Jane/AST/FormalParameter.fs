namespace AST

type FormalParameter(parameterType : Type, name : string) =
    member x.ParameterType = parameterType
    member x.Name          = name
