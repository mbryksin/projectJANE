namespace AST

[<AbstractClass>]
type ClassMember(isStatic : bool, name : string) =
    member x.Name     = name
    member x.IsStatic = isStatic    

type ClassMethod(isStatic : bool, returnType : Type, name : string, formalParameters : FormalParameter list, body : Block) =
    inherit ClassMember(isStatic, name)
    member x.ReturnType       = returnType
    member x.FormalParameters = formalParameters
    member x.Body             = body

type ClassVoidMethod(isStatic : bool, name : string, formalParameters : FormalParameter list, body : Block) =
    inherit ClassMember(isStatic, name)
    member x.FormalParameters = formalParameters
    member x.Body             = body

type ClassField(isStatic : bool, isFinal : bool, fieldType : Type, name : string, expression : Expression) =
    inherit ClassMember(isStatic, name)
    member x.Type       = fieldType
    member x.Expression = expression
    member x.IsFinal    = isFinal 
