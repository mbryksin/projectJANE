namespace AST

[<AbstractClass>]
type ProgramMember(name : string) =
    member x.Name = name 

type Interface(name : string, ancestors : Interface list, members : InterfaceMember list) =
    inherit ProgramMember(name)
    member x.Ancestors = ancestors
    member x.Members   = members

type Class(name : string, classConstructor : ClassConstructor, members : ClassMember list) =
    inherit ProgramMember(name)
    member x.Members     = members
    member x.Constructor = classConstructor
