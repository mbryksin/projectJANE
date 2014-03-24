namespace AST

type ExpressionPrimitive =
    interface 
    end

type ExpressionChar(content : char) =
    interface ExpressionPrimitive
    member x.Get = content

type ExpressionString(content : string) =
    interface ExpressionPrimitive
    member x.Get = content

type ExpressionInt(content : int) =
    interface ExpressionPrimitive
    member x.Get = content

type ExpressionShort(content : int16) =
    interface ExpressionPrimitive
    member x.Get = content

type ExpressionBoolean(content : bool) =
    interface ExpressionPrimitive
    member x.Get = content

type ExpressionFloat(content : float32) =
    interface ExpressionPrimitive
    member x.Get = content

type ExpressionDouble(content : float) =
    interface ExpressionPrimitive
    member x.Get = content