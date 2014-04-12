namespace AST

[<AbstractClass>]
type Literal(pos) =
    inherit Primary(pos)

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


type NullLiteral(pos : Position) =
    inherit Literal(pos)
    
    override x.ToString() = "null"

    override x.Interpret(context : Variable list) = Null

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type CharLiteral(content : char, pos : Position) =
    inherit Literal(pos)
    member x.Get = content

    override x.ToString() = sprintf "%A" content

    override x.Interpret(context : Variable list) = Char content

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type StringLiteral(content : string, pos : Position) =
    inherit Literal(pos)
    member x.Get = content

    override x.ToString() = sprintf "%A" content

    override x.Interpret(context : Variable list) = Str content

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type IntegerLiteral(content : int64, pos : Position) =
    inherit Literal(pos)
    member x.Get = content

    override x.ToString() = sprintf "%d" content

    override x.Interpret(context : Variable list) = Int content

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type BooleanLiteral(content : bool, pos : Position) =
    inherit Literal(pos)
    member x.Get = content

    override x.ToString() = sprintf "%A" content

    override x.Interpret(context : Variable list) = Bool content

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type FloatLiteral(content : float,pos : Position) =
    inherit Literal(pos)
    member x.Get = content

    override x.ToString() = sprintf "%A" content

    override x.Interpret(context : Variable list) = Float content
