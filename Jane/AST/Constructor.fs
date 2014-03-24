namespace AST

type ClassConstructor(formalParameters : FormalParameter list, body : Block) =
    member x.FormalParameters = formalParameters
    member x.Body             = body