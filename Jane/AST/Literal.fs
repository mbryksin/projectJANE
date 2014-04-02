namespace AST

[<AbstractClass>]
type Literal(pos) =
    inherit Primary(pos)

type NullLiteral(pos : Position) =
    inherit Literal(pos)

type CharLiteral(content : char, pos : Position) =
    inherit Literal(pos)
    member x.Get = content

type StringLiteral(content : string, pos : Position) =
    inherit Literal(pos)
    member x.Get = content

type IntegerLiteral(content : int64, pos : Position) =
    inherit Literal(pos)
    member x.Get = content

type BooleanLiteral(content : bool, pos : Position) =
    inherit Literal(pos)
    member x.Get = content

type FloatLiteral(content : float,pos : Position) =
    inherit Literal(pos)
    member x.Get = content
