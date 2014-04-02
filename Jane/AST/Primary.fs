namespace AST

type Primary =
    inherit Expression

type Constructor(typeName : string, arguments : Arguments) =
    member x.TName     = typeName
    member x.Arguments = arguments
    interface Primary with
        member this.Interpret() = new Val() // later

type Identifier(name : string) =
    member x.Name = name
    interface Primary with
        member this.Interpret() = new Val() // do this

type Member(name : string, suffix : Suffix) =
    member x.Name   = name
    member x.Suffix = suffix
    interface Primary with
        member this.Interpret() = new Val() // later