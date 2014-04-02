namespace AST

[<AbstractClass>]
type InterfaceMember(isStatic : bool, name : string, pos : Position) =
    inherit Node(pos)
    member x.Name     = name
    member x.IsStatic = isStatic    

type InterfaceMethod(isStatic : bool, returnType : Type, name : string, formalParameters : FormalParameter list, pos : Position) =
    inherit InterfaceMember(isStatic, name, pos)
    member x.ReturnType       = returnType
    member x.FormalParameters = formalParameters

type InterfaceVoidMethod(isStatic : bool, name : string, formalParameters : FormalParameter list, pos : Position) =
    inherit InterfaceMember(isStatic, name, pos)
    member x.FormalParameters = formalParameters

type InterfaceField(isStatic : bool, isFinal : bool, fieldType : Type, name : string, pos : Position) =
    inherit InterfaceMember(isStatic, name, pos)
    member x.Type    = fieldType
    member x.IsFinal = isFinal
