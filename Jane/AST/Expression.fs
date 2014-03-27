namespace AST

type PrimaryOrMemberCall =
    interface
    end

type Primary =
    inherit PrimaryOrMemberCall

type Constructor(typeName : string, arguments : Arguments) =
    interface Primary
    member x.TypeName  = typeName
    member x.Arguments = arguments

and Identifier(id : string) =
    interface Primary
    member x.Id = id

and Literal =
    inherit Primary

and NullLiteral() =
    interface Literal

and CharLiteral(content : char) =
    interface Literal
    member x.Get = content

and StringLiteral(content : string) =
    interface Literal
    member x.Get = content

and IntegerLiteral(content : int64) =
    interface Literal
    member x.Get = content

and BooleanLiteral(content : bool) =
    interface Literal
    member x.Get = content

and FloatLiteral(content : float) =
    interface Literal
    member x.Get = content



and Initializer =
    interface
    end

and ArrayInitializer(elements : Initializer list) =
    interface Initializer
    member x.Elements = elements

and Suffix =
    interface
    end

and Arguments(arguments : Expression list) =
    interface Suffix
    member x.Arguments = arguments

and ArrayElement(index : Expression) =
    interface Suffix
    member x.Index = index



and MemberCall(first : Primary, rest : (string * Suffix option) list) =
    interface PrimaryOrMemberCall
    interface Statement
    member x.First = first
    member x.Rest  = rest

and ExpressionNOT(unaryOperation : UnarySign option, expression : PrimaryOrMemberCall) =
    member x.UnaryOperation = unaryOperation
    member x.Primary        = expression

and ExpressionMUL(hasNot : bool, expression : ExpressionNOT) =
    member x.HasNot     = hasNot
    member x.Expression = expression

and ExpressionADD(first : ExpressionMUL, rest : (Multiplication * ExpressionMUL) option) =
    member x.First = first
    member x.Rest  = rest

and ExpressionCOMP(first : ExpressionADD, rest : (Addition * ExpressionADD) list) =
    member x.First = first
    member x.Rest  = rest

and ExpressionINST(first : ExpressionCOMP, rest : (Comparison * ExpressionCOMP) option) =
    member x.First = first
    member x.Rest  = rest

and ExpressionEQ(instObject : ExpressionINST, instType : Type option) =
    member x.Object = instObject
    member x.Type   = instType

and ExpressionAND(first : ExpressionEQ, rest : (Equality * ExpressionEQ) option) =
    member x.First = first
    member x.Rest  = rest

and ExpressionOR(first : ExpressionAND, rest : ExpressionAND list) =
    member x.First = first
    member x.Rest  = rest

and Expression(first : ExpressionOR, rest : ExpressionOR list) =
    interface Initializer
    interface Primary
    member x.First = first
    member x.Rest  = rest

