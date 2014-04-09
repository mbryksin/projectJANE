namespace AST

[<AbstractClass>]
type Primary(pos) =
    inherit Expression(pos)

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


type Constructor(typeName : ID, arguments : Arguments, pos : Position) =
    inherit Primary(pos)
    member x.Name      = typeName
    member x.Arguments = arguments

    override x.ToString() = sprintf "new %A%A" typeName arguments

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type Identifier(name : ID) =
    inherit Primary(name.Position)
    member x.Name = name

    override x.ToString() = name.Value

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type Member(name : ID, suffix : Suffix, pos : Position) =
    inherit Primary(pos)
    member x.Name   = name
    member x.Suffix = suffix

    override x.ToString() = sprintf "%A%A" name suffix