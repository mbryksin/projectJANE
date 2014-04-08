namespace AST

[<AbstractClass>]
type Primary(pos) =
    inherit Expression(pos)

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


type Constructor(typeName : string, arguments : Arguments, pos : Position) =
    inherit Primary(pos)
    member x.Name      = typeName
    member x.Arguments = arguments

    override x.ToString() = sprintf "new %s%A" typeName arguments

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type Identifier(name : string, pos : Position) =
    inherit Primary(pos)
    member x.Name = name

    override x.ToString() = name

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type Member(name : string, suffix : Suffix, pos : Position) =
    inherit Primary(pos)
    member x.Name   = name
    member x.Suffix = suffix

    override x.ToString() = sprintf "%s%A" name suffix