namespace AST

type Literal =
    inherit Primary

type NullLiteral() =
    interface Literal

type CharLiteral(content : char) =
    interface Literal
    member x.Get = content

type StringLiteral(content : string) =
    interface Literal
    member x.Get = content

type IntegerLiteral(content : int64) =
    interface Literal
    member x.Get = content

type BooleanLiteral(content : bool) =
    interface Literal
    member x.Get = content

type FloatLiteral(content : float) =
    interface Literal
    member x.Get = content
