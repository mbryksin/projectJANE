namespace AST

[<AbstractClass>]
type ClassMember(isStatic : bool, name : string, pos : Position) =
    inherit Node(pos)
    member x.Name     = name
    member x.IsStatic = isStatic    

type ClassMethod(isStatic : bool, returnType : Type, name : string, formalParameters : FormalParameter list, body : Block, pos : Position) =
    inherit ClassMember(isStatic, name, pos)
    member x.ReturnType       = returnType
    member x.FormalParameters = formalParameters
    member x.Body             = body

type ClassVoidMethod(isStatic : bool, name : string, formalParameters : FormalParameter list, body : Block, pos : Position) =
    inherit ClassMember(isStatic, name, pos)
    member x.FormalParameters = formalParameters
    member x.Body             = body

type ClassField(isStatic : bool, isFinal : bool, fieldType : Type, name : string, expression : Expression, pos : Position) =
    inherit ClassMember(isStatic, name, pos)
    member x.Type       = fieldType
    member x.Expression = expression
    member x.IsFinal    = isFinal 

type ClassConstructor(formalParameters : FormalParameter list, body : Block, pos : Position) =
    inherit Node(pos)
    member x.FormalParameters = formalParameters
    member x.Body             = body