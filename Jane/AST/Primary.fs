namespace AST

type Primary =
    inherit Expression

type Constructor(typeName : string, arguments : Arguments) =
    interface Primary
    member x.TName     = typeName
    member x.Arguments = arguments

type Identifier(name : string) =
    interface Primary
    member x.Name = name

type Member(name : string, suffix : Suffix) =
    interface Primary
    member x.Name   = name
    member x.Suffix = suffix