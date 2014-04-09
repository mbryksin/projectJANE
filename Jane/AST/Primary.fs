namespace AST

[<AbstractClass>]
type Primary(pos) =
    inherit Expression(pos)

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


type Constructor(typeName : string, arguments : Arguments, pos : Position) =
    inherit Primary(pos)
    member x.TName     = typeName
    member x.Arguments = arguments

    override x.ToString() = sprintf "new %s%A" typeName arguments

    override x.Interpret() = new Val() // later

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type Identifier(name : string, pos : Position) =
    inherit Primary(pos)
    member x.Name = name

    override x.ToString() = name
 
    override x.Interpret() = new Val() // do this

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type Member(name : string, suffix : Suffix, pos : Position) =
    inherit Primary(pos)
    member x.Name   = name
    member x.Suffix = suffix

    override x.ToString() = sprintf "%s%A" name suffix

    override x.Interpret() = new Val() // do this
