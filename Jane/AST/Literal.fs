namespace AST

type Literal =
    inherit Primary

type NullLiteral() =
    interface Literal with 
        member this.Interpret() = new Val(())
    
type CharLiteral(content : char) =
    interface Literal with 
        member this.Interpret() = new Val(content)
    member x.Get = content

type StringLiteral(content : string) =
    interface Literal with 
        member this.Interpret() = new Val(content)
    member x.Get = content

type IntegerLiteral(content : int64) =
    interface Literal with 
        member this.Interpret() = new Val(content)
    member x.Get = content

type BooleanLiteral(content : bool) =
    interface Literal with 
        member this.Interpret() = new Val(content)
    member x.Get = content

type FloatLiteral(content : float) =
    interface Literal with 
        member this.Interpret() = new Val(content)
    member x.Get = content