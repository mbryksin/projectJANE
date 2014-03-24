namespace AST

[<AbstractClass>]
type InterfaceMember(isStatic : bool, name : string) =
    member x.Name     = name
    member x.IsStatic = isStatic    

type InterfaceMethod(isStatic : bool, returnType : Type, name : string, formalParameters : FormalParameter list) =
    inherit InterfaceMember(isStatic, name)
    member x.ReturnType       = returnType
    member x.FormalParameters = formalParameters

type InterfaceVoidMethod(isStatic : bool, name : string, formalParameters : FormalParameter list) =
    inherit InterfaceMember(isStatic, name)
    member x.FormalParameters = formalParameters

type InterfaceField(isStatic : bool, isFinal : bool, fieldType : Type, name : string) =
    inherit InterfaceMember(isStatic, name)
    member x.Type    = fieldType
    member x.IsFinal = isFinal
