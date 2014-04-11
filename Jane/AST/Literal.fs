namespace AST

[<AbstractClass>]
type Literal(pos) =
    inherit Primary(pos)

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


type NullLiteral(pos : Position) =
    inherit Literal(pos)
    
    override x.ToString() = "null"

    override x.Interpret(context : Variable list) = new Val(())

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type CharLiteral(content : char, pos : Position) =
    inherit Literal(pos)
    member x.Get = content

    override x.ToString() = sprintf "%A" content

    override x.Interpret(context : Variable list) = new Val(content)

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type StringLiteral(content : string, pos : Position) =
    inherit Literal(pos)
    member x.Get = content

    override x.ToString() = sprintf "%A" content

    override x.Interpret(context : Variable list) = new Val(content)

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type IntegerLiteral(content : int64, pos : Position) =
    inherit Literal(pos)
    member x.Get = content

    override x.ToString() = sprintf "%d" content

    override x.Interpret(context : Variable list) = new Val(content)

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type BooleanLiteral(content : bool, pos : Position) =
    inherit Literal(pos)
    member x.Get = content

    override x.ToString() = sprintf "%A" content

    override x.Interpret(context : Variable list) = new Val(content)

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type FloatLiteral(content : float,pos : Position) =
    inherit Literal(pos)
    member x.Get = content

    override x.ToString() = sprintf "%A" content

    override x.Interpret(context : Variable list) = new Val(content)
