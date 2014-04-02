namespace AST

[<AbstractClass>]
type Primary(pos) =
    inherit Expression(pos)

type Constructor(typeName : string, arguments : Arguments, pos : Position) =
    inherit Primary(pos)
    member x.TName     = typeName
    member x.Arguments = arguments

type Identifier(name : string, pos : Position) =
    inherit Primary(pos)
    member x.Name = name

type Member(name : string, suffix : Suffix, pos : Position) =
    inherit Primary(pos)
    member x.Name   = name
    member x.Suffix = suffix